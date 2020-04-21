# microservicewithpostgresql
Its a micorservice based application which uses .NET core and PostGreSQL

Following Nuget packages to be installed:
Npgsql.EntityFrameworkCore.PostgreSQL
Microsoft.EntityFrameworkCore.Design
Microsoft.EntityFrameworkCore.Tools

Swagger:
Swashbuckle.AspNetCore

StyleCop.Analyzers - for code documentation and correctness


Microsoft.AspNetCore.Mvc.Api.Analyzers - for web api analysis

In project enable this
<PropertyGroup>
 <IncludeOpenAPIAnalyzers>true</IncludeOpenAPIAnalyzers>
</PropertyGroup>


Microsoft.AspNetCore.Mvc.Versioning - A service API versioning library for Microsoft ASP.NET Core.

Microsoft.AspNetCore.Mvc.Versioning.ApiExplorer - ASP.NET Core MVC API explorer functionality for discovering metadata such as the list of API-versioned controllers and actions, and their URLs and allowed HTTP methods.
