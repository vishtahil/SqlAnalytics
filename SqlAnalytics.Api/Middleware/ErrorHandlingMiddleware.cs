using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;


namespace SqlAnalytics.Api.Middleware
{
  /* http://stackoverflow.com/questions/38630076/asp-net-core-web-api-exception-handling */
  public class ErrorHandlingMiddleware
    {
    private readonly RequestDelegate next;

    public ErrorHandlingMiddleware(RequestDelegate next)
    {
      this.next = next;
    }

    public async Task Invoke(HttpContext context /* other scoped dependencies */)
    {
      try
      {
        await next(context);
      }
      catch (Exception ex)
      {
        await HandleExceptionAsync(context, ex);
      }
    }

    private static Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
      var code = HttpStatusCode.InternalServerError;
      var exceptionMessage = exception.Message;
      // 500 if unexpected
      //if (exception is MyNotFoundException) code = HttpStatusCode.NotFound;
      //else if (exception is MyUnauthorizedException) code = HttpStatusCode.Unauthorized;
      //else if (exception is MyException) code = HttpStatusCode.BadRequest;
      if (exception.GetType().ToString() == "System.Xml.XmlException")
      {
        exceptionMessage = $"Invalid Execution Plan XML. {exception.Message}";
      }
      else if(exception.GetType().ToString() == "System.Data.SqlClient.SqlException")
      {
        if (exceptionMessage.ToLower().Contains("could not open a connection to sql server"))
        {
          exceptionMessage = $"Invalid Sql Connection String. {exception.Message}";
        }
        else
        {
          exceptionMessage = $"Invalid Sql Statement. {exception.Message}";
        }
      }

      var result = JsonConvert.SerializeObject(new { error = exceptionMessage });
      context.Response.ContentType = "application/json";
      context.Response.StatusCode = (int)code;
      return context.Response.WriteAsync(result);
    }
  }
}
