# microservicewithpostgresql packages
Its a micorservice based application which uses .NET core and PostGreSQL

Following Nuget packages to be installed:

# 1. PostGreSQL:
     a. Npgsql.EntityFrameworkCore.PostgreSQL
     b. Microsoft.EntityFrameworkCore.Design
     c. Microsoft.EntityFrameworkCore.Tools

# 2. Swagger:
     a. Swashbuckle.AspNetCore
     b. Microsoft.AspNetCore.Mvc.Api.Analyzers - for web api analysis
        In project enable this
        ````
         <PropertyGroup>
          <IncludeOpenAPIAnalyzers>true</IncludeOpenAPIAnalyzers>
         </PropertyGroup>
        ````
     c. Microsoft.AspNetCore.Mvc.Versioning - A service API versioning library for Microsoft ASP.NET Core.
     d. Microsoft.AspNetCore.Mvc.Versioning.ApiExplorer - ASP.NET Core MVC API explorer functionality for discovering metadata such as the list of API-versioned controllers and actions, and their URLs and allowed HTTP methods.

# 3. Health checks:
     a. AspNetCore.HealthChecks.NpgSql - HealthChecks.NpgSql is a health check for Postgress Sql.
     b. AspNetCore.HealthChecks.Uris - HealthChecks.Uris is a simple health check package for Uri groups.
     
# 4. Hosting in IIS
     https://docs.microsoft.com/en-us/aspnet/core/host-and-deploy/iis/?view=aspnetcore-3.1#install-the-net-core-hosting-bundle

# 5. SQL related
     a. script-migration - for generating sql scripts (for prod)
     b. add-migration - for generating new migration
     c. update-database - to reflect latest migration in db
     d. remove-migration - for removing changes
     
# 6. Logging using Serilog along with changes in appsettings.json file
     a. Refer to: https://github.com/serilog/serilog-aspnetcore and https://github.com/serilog/serilog-sinks-file (for file rolling)
     b. Serilog.AspNetCore - Serilog support for ASP.NET Core logging
     c. Serilog.Settings.Configuration - Microsoft.Extensions.Configuration (appsettings.json) support for Serilog.
     d. Serilog.Sinks.File - Write Serilog events to text files in plain or JSON format.
