namespace AdventureTrip
{
    using Customer.API.Core;
    using Customer.API.HealthCheckQueuePublisher;
    using Customer.API.Middlewares;
    using Customer.API.Persistence;
    using Customer.API.QueueMessage;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Diagnostics.HealthChecks;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Abstractions;
    using Microsoft.AspNetCore.Mvc.ApiExplorer;
    using Microsoft.AspNetCore.Mvc.Formatters;
    using Microsoft.AspNetCore.Mvc.Versioning;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Diagnostics.HealthChecks;
    using Microsoft.Extensions.Hosting;
    using Microsoft.OpenApi.Models;
    using Newtonsoft.Json.Linq;
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Reflection;
    using System.Threading.Tasks;

    public class Startup
    {

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddResponseCaching();

            services.AddMvc(
                setupActions =>
                {
                    // adding swagger documentation to response type to all the controllers
                    setupActions.Filters.Add(
                        new ProducesResponseTypeAttribute(StatusCodes.Status504GatewayTimeout));

                    // to return response type as xml alsoapart from json
                    setupActions.OutputFormatters.Add(new XmlSerializerOutputFormatter());

                    // in case a different response type is requested the output will return media type not accepted
                    setupActions.ReturnHttpNotAcceptable = true;

                    // response caching profile creation
                    setupActions.CacheProfiles.Add("30SecondsCacheProfile", new CacheProfile()
                    {
                        Duration = 30
                    });

                }
                );

            // postpresql addition
            services.AddEntityFrameworkNpgsql().AddDbContext<CustomerContext>
                (
                opt =>
                opt.UseNpgsql(Configuration.GetConnectionString("PostGreConnection"))
                );

            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<IUser, CustomerRepository>();

            //for api versioning
            services.AddVersionedApiExplorer(
                setup =>
                {
                    setup.GroupNameFormat = "'v'VV";
                    //setup.SubstituteApiVersionInUrl = true;
                });

            services.AddApiVersioning(setupAction =>
            {
                setupAction.AssumeDefaultVersionWhenUnspecified = true;
                setupAction.DefaultApiVersion = new ApiVersion(1, 0);
                setupAction.ReportApiVersions = true;
                setupAction.ApiVersionReader = ApiVersionReader.Combine(
                    new HeaderApiVersionReader("x-version"),
                    new QueryStringApiVersionReader("ver"));
            });

            var apiVersionDescriptionProvider = services.BuildServiceProvider().GetService<IApiVersionDescriptionProvider>();

            //swagger addition
            services.AddSwaggerGen(setupAction =>
            {
                setupAction.AddSecurityDefinition("basicAuth", new Microsoft.OpenApi.Models.OpenApiSecurityScheme()
                {
                    Type = Microsoft.OpenApi.Models.SecuritySchemeType.Http,
                    Scheme = "basic",
                    Description = "Enter username and password token to validate the api"
                });

                setupAction.AddSecurityRequirement(new Microsoft.OpenApi.Models.OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type= ReferenceType.SecurityScheme,
                                Id="basicAuth" }
                        }, new List<string>() }
                });

                // api versioning
                setupAction.DocInclusionPredicate((documentName, apiDescription) =>
                {
                    var actionApiVersionModel = apiDescription.ActionDescriptor
                    .GetApiVersionModel(ApiVersionMapping.Explicit | ApiVersionMapping.Implicit);

                    if (actionApiVersionModel == null)
                    {
                        return true;
                    }

                    if (actionApiVersionModel.DeclaredApiVersions.Any())
                    {
                        //actionApiVersionModel.DeclaredApiVersions.Any(v =>
                        //Console.WriteLine(v));

                        // note: add 'v' for version as suffix to api specification name
                        return actionApiVersionModel.DeclaredApiVersions.Any(v =>
                        $"AdventripOpenAPISpecificationv{v.ToString()}" == documentName);
                    }

                    return actionApiVersionModel.ImplementedApiVersions.Any(v =>
                        $"AdventripOpenAPISpecificationv{v.ToString()}" == documentName);
                });

                foreach (var description in apiVersionDescriptionProvider.ApiVersionDescriptions)
                {
                    setupAction.SwaggerDoc(
                    $"AdventripOpenAPISpecification{description.GroupName}",
                    new Microsoft.OpenApi.Models.OpenApiInfo()
                    {
                        Title = "Adventrip API",
                        Version = description.ApiVersion.ToString(),
                        Description = "First Customer API build using Swagger/OPENAPI",
                        License = new Microsoft.OpenApi.Models.OpenApiLicense()
                        {
                            Name = "MIT",
                            Url = new Uri("http://google.com"),
                        },
                    });
                }




                //setupAction.SwaggerDoc(
                //    "AddressOpenAPISpecification",
                //    new Microsoft.OpenApi.Models.OpenApiInfo()
                //    {
                //        Title = "Address API",
                //        Version = "1",
                //        Description = "First Address API build using Swagger/OPENAPI",
                //        License = new Microsoft.OpenApi.Models.OpenApiLicense()
                //        {
                //            Name = "MIT",
                //            Url = new Uri("http://google.com"),
                //        },
                //    });

                //enable xml generation first from project properties -> build and then change change the path to just
                //projectname.xml

                string xmlCommentsFileName = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                string xmlCommentsFileFullPath = Path.Join(AppContext.BaseDirectory, xmlCommentsFileName);
                setupAction.IncludeXmlComments(xmlCommentsFileFullPath);
            });

            services.AddHealthChecks()
                .AddNpgSql(Configuration.GetConnectionString("PostGreConnection"), failureStatus: HealthStatus.Unhealthy, tags: new[] { "ready" })
                // if api is dependend on other api can check for that api health status using below code
                .AddUrlGroup(new Uri(
                    Configuration.GetSection("URL").GetSection("BaseURL").Value),
                    "Web API health check",
                    HealthStatus.Degraded,
                    timeout: new TimeSpan(0, 0, 5),
                    tags: new[] { "ready" }
                    );

            // usually not required, in needs to customize publisher check then use below
            services.Configure<HealthCheckPublisherOptions>(options =>
            {
                options.Delay = TimeSpan.FromSeconds(5);
            });

            services.AddSingleton<IHealthCheckPublisher, HealthCheckQueuePublisher>();
            services.AddTransient<IQueueMessage, RabbitMQQueueMessage>();

            //caching
            services.AddMemoryCache();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IApiVersionDescriptionProvider apiVersionDescriptionProvider)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            //use hsts if strict redirect to https is required
            //app.UseHsts();

            app.UseMiddleware<ExceptionMiddleware>();

            app.UseResponseCaching();

            app.UseHttpsRedirection();

            //swagger
            app.UseApiVersioning();
            app.UseSwagger(); // to generate json format api
            app.UseSwaggerUI(setupAction =>
            {
                foreach (var description in apiVersionDescriptionProvider.ApiVersionDescriptions)
                {
                    setupAction.SwaggerEndpoint($"/swagger/AdventripOpenAPISpecification{description.GroupName}/swagger.json", description.GroupName.ToUpperInvariant());
                }


                //setupAction.SwaggerEndpoint("/swagger/AddressOpenAPISpecification/swagger.json", "Address API");

                //custom swagger UI html page
                setupAction.IndexStream = () => this.GetType().Assembly
                 .GetManifestResourceStream("Customer.API.EmbeddedAssets.index.html");
                setupAction.RoutePrefix = ""; // to mark the initial page as swagger page like localhost:8080/index.html
            }); // to generate UI for swagger json data

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                //form endpoint and also to customize httpstatus code since unhealty will also return 200 as status code
                endpoints.MapHealthChecks("/health/ready", new HealthCheckOptions
                {
                    ResultStatusCodes =
                    {
                        [HealthStatus.Healthy] = StatusCodes.Status200OK,
                        [HealthStatus.Degraded] = StatusCodes.Status500InternalServerError,
                        [HealthStatus.Unhealthy] = StatusCodes.Status503ServiceUnavailable
                    },
                    ResponseWriter = WriteHealthCheckReadyResponse,
                    Predicate = (check) => check.Tags.Contains("ready"), // show tags which are ready
                    AllowCachingResponses = false
                });
                // endpoints can also be configured for autorization and cors;
                // .RequireAuthorization("HealthCheckPolicy").RequireCors()

                // contains only overall status of the heath so its faster
                endpoints.MapHealthChecks("/health/live", new HealthCheckOptions
                {
                    ResponseWriter = WriteHealthCheckLiveResponse,
                    Predicate = (check) => check.Tags.Contains("ready"), // show tags which are not ready
                    AllowCachingResponses = false
                });
            });
        }

        private Task WriteHealthCheckLiveResponse(HttpContext httpContext, HealthReport result)
        {
            httpContext.Response.ContentType = "application/json";

            var json = new JObject(
                new JProperty("OverallStatus", result.Status.ToString()),
                new JProperty("TotalChecksDuration", result.TotalDuration.TotalSeconds.ToString("0.0.00")));

            return httpContext.Response.WriteAsync(json.ToString(Newtonsoft.Json.Formatting.Indented));
        }

        // to customize http response of health status as json
        private Task WriteHealthCheckReadyResponse(HttpContext httpContext, HealthReport result)
        {
            httpContext.Response.ContentType = "application/json";

            var json = new JObject(
                new JProperty("OverallStatus", result.Status.ToString()),
                new JProperty("TotalChecksDuration", result.TotalDuration.TotalSeconds.ToString("0.0.00")),
                new JProperty("DependencyHealtChecks", new JObject(result.Entries.Select(dicItem =>
                    new JProperty(dicItem.Key, new JObject(
                        new JProperty("Status", dicItem.Value.Status.ToString()),
                        new JProperty("Duration", dicItem.Value.Duration.TotalSeconds.ToString("0.0.00")),
                         new JProperty("Exception", dicItem.Value.Exception?.Message)
                        ))
                )))
                );

            return httpContext.Response.WriteAsync(json.ToString(Newtonsoft.Json.Formatting.Indented));
        }
    }
}
