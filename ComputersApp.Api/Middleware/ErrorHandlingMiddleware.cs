using ComputersApp.Application.Exceptions;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Authentication;
using System.Text.Json;
using System.Threading.Tasks;

namespace ComputersApp.Api.Middleware
{
    public class ErrorHandlingMiddleware
    {
        private static Dictionary<Type, HttpStatusCode> _exceptions;
        private readonly RequestDelegate _next;

        public ErrorHandlingMiddleware(RequestDelegate next)
        {
            _exceptions = new Dictionary<Type, HttpStatusCode>();
            _exceptions.Add(typeof(BadRequestException), HttpStatusCode.BadRequest);
            _exceptions.Add(typeof(NotFoundException), HttpStatusCode.NotFound);
            _exceptions.Add(typeof(InvalidCredentialException), HttpStatusCode.Forbidden);
            _exceptions.Add(typeof(SecurityTokenException), HttpStatusCode.Forbidden);
            _next = next;
        }

        public async Task Invoke(HttpContext context, IWebHostEnvironment env)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(context, ex, env);
            }
        }

        private static Task HandleExceptionAsync(HttpContext context, Exception exception, IWebHostEnvironment env)
        {
            HttpStatusCode status;
            string message;
            var stackTrace = string.Empty;
            var exceptionType = exception.GetType();
            if (_exceptions.ContainsKey(exceptionType))
            {
                status = _exceptions[exceptionType];
                message = exception.Message;
            }
            else
            {
                status = HttpStatusCode.InternalServerError;
                message = exception.Message;
                if (env.IsEnvironment("Development"))
                    stackTrace = exception.StackTrace;
            }

            var result = JsonSerializer.Serialize(new { error = message, stackTrace });
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)status;
            return context.Response.WriteAsync(result);
        }
    }
}
