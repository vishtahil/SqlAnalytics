using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Serialization;
using Microsoft.AspNetCore.Diagnostics;
using SqlAnalytics.Api.Middleware;
using SqlAnalytics.Api.Configuration;

namespace SqlAnalytics.Api
{
  public class Startup
  {
    public Startup(IHostingEnvironment env)
    {
      var builder = new ConfigurationBuilder()
          .SetBasePath(env.ContentRootPath)
          .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
          .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
         .AddJsonFile("Content/resource.json", optional: false, reloadOnChange: true)
          .AddEnvironmentVariables();
      Configuration = builder.Build();
    }

    public IConfigurationRoot Configuration { get; }

    // This method gets called by the runtime. Use this method to add services to the container.
    public void ConfigureServices(IServiceCollection services)
    {
      // Add framework services.
      services.AddMvc()
          .AddJsonOptions(options => options.SerializerSettings.ContractResolver = new DefaultContractResolver()); ;

      services.Configure<ContentConfiguration>(Configuration.GetSection("ContentConfiguration"));
    }

    // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
    public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
    {
      loggerFactory.AddConsole();
      loggerFactory.AddDebug();

      app.UseCors(cors =>
        cors
        .AllowAnyHeader()
        .AllowAnyMethod()
        .AllowAnyOrigin()
      );

      app.Use(async (context, next) =>
      {
        await next();
        if (context.Response.StatusCode == 404)
        {
          Console.WriteLine("passing to client");
          context.Request.Path = "/";
          await next();
        }
      });

      app.UseFileServer(enableDirectoryBrowsing: false);

      app.UseMiddleware(typeof(ErrorHandlingMiddleware));
      app.UseMvc();

    }
  }
}
