using System;
using System.Net;
using System.Threading.Tasks;
using Billing.Host.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Billing.Host.Middlewares
{
    /// <summary>
    ///     Мидлвара для перехвата ошибок.
    /// </summary>
    internal class ExceptionHandlerMiddleware
    {
        private readonly JsonSerializerSettings _defaultSerializerSettings;
        private readonly ILogger<ExceptionHandlerMiddleware> _logger;
        private readonly RequestDelegate _next;

        public ExceptionHandlerMiddleware(RequestDelegate next,
            ILogger<ExceptionHandlerMiddleware> logger)
        {
            _logger = logger;
            _next = next;
            _defaultSerializerSettings = new JsonSerializerSettings
            {
                ContractResolver = new DefaultContractResolver
                {
                    NamingStrategy = new SnakeCaseNamingStrategy(),
                },
                Formatting = Formatting.Indented,
            };
        }

        public async Task InvokeAsync(HttpContext httpContext)
        {
            try
            {
                await _next(httpContext);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Internal server error");
                await HandleExceptionAsync(httpContext);
            }
        }

        private Task HandleExceptionAsync(HttpContext context,
            HttpStatusCode httpStatusCode = HttpStatusCode.BadRequest,
            string errorMessage = "Bad request")
        {
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int) httpStatusCode;
            var details = new ErrorDto
            {
                Code = context.Response.StatusCode,
                Message = errorMessage
            };
            var serializedDto = JsonConvert.SerializeObject(details, _defaultSerializerSettings);

            return context.Response.WriteAsync(serializedDto);
        }
    }
}