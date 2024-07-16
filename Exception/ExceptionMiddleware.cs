using DatingApp.Error;
using System.Net;
using System.Text.Json;

namespace DatingApp.ExceptionTest
{
    public class ExceptionMiddleware(RequestDelegate next,ILogger<ExceptionMiddleware> logger,IHostEnvironment host)
    {
        public async Task InvokeAsync(HttpContext httpContent)
        {
            try
            {
                await next(httpContent);
            }
            catch(Exception ex)
            {
                logger.LogError(ex, ex.Message);
                httpContent.Response.ContentType = "application/json";
                httpContent.Response.StatusCode =(int)HttpStatusCode.InternalServerError;

                var resonse = host.IsDevelopment()
                ? new APIException(httpContent.Response.StatusCode, ex.Message, ex.StackTrace)
                :  new APIException(httpContent.Response.StatusCode, ex.Message, "internal server error");
                var option = new JsonSerializerOptions
                {
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase
                };
                var json=JsonSerializer.Serialize(resonse, option);
                await httpContent.Response.WriteAsync(json);
            }
        }
    }
}
