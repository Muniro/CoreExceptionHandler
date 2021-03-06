using Contracts;
using GlobalExceptionHandler.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace GlobalExceptionHandler.Extensions
{
    public static class ExceptionMiddlewareExtensions
    {
        public static void BCEConfigureExceptionHandler(this IApplicationBuilder app, ILoggerManager logger)
        {
            app.UseExceptionHandler(appException =>
            {
                appException.Run(async context =>
                {
                    context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                    context.Response.ContentType = "application/json";

                    var contextFeature = context.Features.Get<IExceptionHandlerFeature>();

                    if(contextFeature != null)
                    {
                        logger.LogError($"An exception has occured: {contextFeature.Error}");

                        await context.Response.WriteAsync(new ErrorDetails
                        {
                            StatusCode = context.Response.StatusCode,
                            Message = "Internal Server Error"
                        }.ToString());
                    }

                });
            });
        }

    }
}
