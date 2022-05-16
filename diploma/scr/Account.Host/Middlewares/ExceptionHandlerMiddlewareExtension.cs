using Microsoft.AspNetCore.Builder;

namespace Account.Host.Middlewares
{
    /// <summary>
    /// Класс расширения для <see cref="ExceptionHandlerMiddleware"/>.
    /// </summary>
    internal static class ExceptionHandlerMiddlewareExtension
    {
        /// <summary>
        /// Добавление <see cref="ExceptionHandlerMiddleware"/> в пайплайн приложения.
        /// </summary>
        /// <param name="builder"> <see cref="IApplicationBuilder"/>. </param>
        /// <returns> <see cref="IApplicationBuilder"/>. </returns>
        public static IApplicationBuilder UseExceptionHandlerMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<ExceptionHandlerMiddleware>();
        }
    }
}