using System.Reflection;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MovieSvc.Application.Common.Behaviour;
using MovieSvc.Application.Helpers;
using MovieSvc.Application.Interface;
using MovieSvc.Extensions;
using Serilog;

var builder = WebApplication.CreateBuilder(args);
//var app = builder.Build();

Main(builder);
// app.MapGet("/", () => "Welcome to Movies API");
//
// app.Run();


void Main(WebApplicationBuilder builder)
{
    
    WebApplication app = builder.Build();
    
    var configuration = BuildAppJson(app.Environment.EnvironmentName);
    AppSettings appSettings = configuration.GetSection("AppSettings").Get<AppSettings>();
    builder.Services.AddSingleton<AppSettings>(appSettings);
    
    ConfigureServices(builder, app);
    
    
    Log.Logger = new LoggerConfiguration()
        .ReadFrom.Configuration(configuration)
        .CreateLogger();

    try
    {
        Log.Information("App now starting...");
        app.Run();
    }
    catch (Exception ex)
    {
        Log.Fatal(ex, "App not started");
    }
    finally
    {
        Log.CloseAndFlush();
    }
}


IConfigurationRoot BuildAppJson(string enviromentName)
{
    IConfigurationRoot config = new ConfigurationBuilder()
        .AddJsonFile($"appsettings.{enviromentName}.json")
        .AddEnvironmentVariables()
        .AddCommandLine(args)
        .Build();

    return config;
}

void ConfigureServices(WebApplicationBuilder builder, WebApplication app)
{
    
    builder.Services.Configure<ApiBehaviorOptions>(x =>
    {
        x.SuppressModelStateInvalidFilter = true;
    });
    
    builder.Services.AddControllers(options =>
    {
        options.Filters.Add(new ApiExceptionFilter());
        options.Filters.Add(new CustomModelValidate());
    });


    builder.Services.AddApplicationServices();
    AddDbContext(builder, "");

    
    app.UseRouting();
    app.UseEndpoints(endpoint =>
    {
        endpoint.MapGet("/", async ctx =>
        {
            ctx.Response.StatusCode = 200;
            await ctx.Response.WriteAsync("Welcome To MOVIES API");
        });
        endpoint.MapControllers();
    });
}



void AddDbContext(WebApplicationBuilder builder, string conectionString)
{
    //_builder.Services.AddTransient<PayPromptDbContext>();
    builder.Services.AddDbContext<DbContext>(x =>
    {
        x.UseSqlServer(conectionString);
    });
}