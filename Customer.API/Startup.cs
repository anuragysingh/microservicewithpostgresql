namespace WebApplication1
{
    using System;
    using System.IO;
    using System.Reflection;
    using Customer.API.Core;
    using Customer.API.Persistence;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Formatters;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Hosting;

    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
        public IConfiguration Configuration { get; }
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers(
                setupActions =>
                {
                    // adding swagger documentation to response type to all the controllers
                    setupActions.Filters.Add(
                        new ProducesResponseTypeAttribute(StatusCodes.Status504GatewayTimeout));

                    // to return response type as xml alsoapart from json
                    setupActions.OutputFormatters.Add(new XmlSerializerOutputFormatter());

                    // in case a different response type is requested the output will return media type not accepted
                    setupActions.ReturnHttpNotAcceptable = true;
                }
                );
            services.AddEntityFrameworkNpgsql().AddDbContext<CustomerContext>
                (
                opt =>
                opt.UseNpgsql(Configuration.GetConnectionString("PostGreConnection"))
                );

            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<IUser, CustomerRepository>();

            //swagger addition
            services.AddSwaggerGen(setupAction =>
            {
                setupAction.SwaggerDoc(
                    "CustomerOpenAPISpecification",
                    new Microsoft.OpenApi.Models.OpenApiInfo()
                    {
                        Title = "Customer API",
                        Version = "1",
                        Description = "First Customer API build using Swagger/OPENAPI",
                        License = new Microsoft.OpenApi.Models.OpenApiLicense()
                        {
                            Name = "MIT",
                            Url = new Uri("http://google.com"),
                        },
                    }
                    );

                //enable xml generation first from project properties -> build and then change change the path to just
                //projectname.xml

                string xmlCommentsFileName = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                string xmlCommentsFileFullPath = Path.Join(AppContext.BaseDirectory, xmlCommentsFileName);
                setupAction.IncludeXmlComments(xmlCommentsFileFullPath);
            });
            
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            //swagger
            app.UseSwagger(); // to generate json format api
            app.UseSwaggerUI(setupAction =>
            {
                setupAction.SwaggerEndpoint("/swagger/CustomerOpenAPISpecification/swagger.json", "Customer API");
                setupAction.RoutePrefix = ""; // to mark the initial page as swagger page like localhost:8080/index.html
            }); // to generate UI for swagger json data

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
