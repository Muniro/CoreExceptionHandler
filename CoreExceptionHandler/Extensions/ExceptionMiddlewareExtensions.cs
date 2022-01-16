using Microsoft.AspNetCore.Builder;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using System.Net;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using CoreExceptionHandler.Models;
using Contracts;

namespace CoreExceptionHandler.Extensions
{
    public static class ExceptionMiddlewareExtensions
    {
        public static void ConfigurationExceptionHandler(this IApplicationBuilder  app, ILoggerManager logger)
        {
            app.UseExceptionHandler(appError =>
            {
                appError.Run(async context =>
                {

                    context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                    context.Response.ContentType = "application/json";

                    var contextFeature = context.Features.Get<ExceptionHandlerFeature>();
                    if(contextFeature != null)
                    {

                        logger.LogError($"Something went wrong: {contextFeature.Error}");
                        await context.Response.WriteAsync(new ErrorDetails
                        {
                            StatucCode= context.Response.StatusCode,
                            Message= "Internal Server Error"
                        }.ToString());

                    }
                });
            });
        }
    }
}
