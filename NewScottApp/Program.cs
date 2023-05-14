using Base.API.Services.ApiVersioning;
using Base.API.Services.ExceptionHandling;
using Base.API.Services.Swagger;
using Base.Application;
using Base.Infrastructure;
using Microsoft.AspNetCore.Mvc.ApplicationModels;
using Microsoft.Extensions.FileProviders;
using NewScottApp.Application;
using NewScottApp.Getway.Helpers;
using NewScottApp.Infrastructure;
using NewScottApp.Setup;

var builder = WebApplication.CreateBuilder(args);

#region Appsettings & env variables

builder.Configuration
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appsettings.json", true, true) // Try reading from appsettings
    .AddEnvironmentVariables(); // then override the existing env variables

#endregion


#region .Net Services

builder.Services.AddControllersWithViews();
builder.Services.AddControllers(options =>
{
    options.Conventions.Add(new RouteTokenTransformerConvention(new SlugifyParameterTransformer()));
});

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddDatabaseDeveloperPageExceptionFilter();
builder.Services.AddHttpContextAccessor();

#endregion


#region Base Packages

builder.Services.AddApplication();
builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.AddBaseSwagger(builder.Configuration);
builder.Services.AddBaseApiVersioning();
builder.Services.AddExceptionHandling();

#endregion

#region Identity Solution

//builder.Services.AddScoped<IdentityDatabaseContext>();
//builder.Services.AddHealthChecks().AddDbContextCheck<IdentityDatabaseContext>();
builder.Services.AddIdentitySetup(builder.Configuration);
builder.Services.AddIdentityApplication();
builder.Services.AddIdentityInfrastructure(builder.Configuration);

#endregion

var app = builder.Build();

#region Using Base Packages

app.UseIdentitySetup();
app.UseExceptionHandling();
app.UseBaseSwagger(app.Configuration);

#endregion

string folderPath = Path.Combine(Directory.GetCurrentDirectory(), "uploads");
if (!Directory.Exists(folderPath))
{
    Directory.CreateDirectory(folderPath);
}

app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new PhysicalFileProvider(folderPath),
    RequestPath = "/uploads"
});

using (var scope = app.Services.CreateScope())
{
    await scope.MigrateDatabase();
    await scope.SeedDatabase();
}

app.Run();