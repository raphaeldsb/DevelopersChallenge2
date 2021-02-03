using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using System;
using System.Net;
using System.Threading.Tasks;

namespace BankTransactionConciliationAPI.MIddlewares
{
    public class ExceptionHandlerMiddleware
    {
        private readonly RequestDelegate Next;

        public ExceptionHandlerMiddleware(RequestDelegate next)
        {
            this.Next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await this.Next(context);
            }
            catch (Exception)
            {
                context.Response.ContentType = "application/json";
                context.Response.StatusCode = (int) HttpStatusCode.InternalServerError;

                context.Response.WriteAsync("{\n    \"message\" : \"Oops, an error occurred!\"\n}").Wait();
                context.Response.Body.Position = 0;
            }
        }
    }

    public static class ExceptionHandlerMiddlewareExtension
    {
        public static void UseCustomExceptionHandler(this IApplicationBuilder app)
        {
            app.UseMiddleware<ExceptionHandlerMiddleware>();
        }
    }
}
