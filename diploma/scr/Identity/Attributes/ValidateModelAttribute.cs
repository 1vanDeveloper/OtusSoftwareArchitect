using System.Linq;
using Identity.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Identity.Attributes
{
    /// <summary>
    ///     Атрибут валидации моделей.
    /// </summary>
    internal class ValidateModelAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            if (context.ModelState.IsValid)
            {
                return;
            }
            
            var message = string.Join("\n", context.ModelState.Values
                .SelectMany(v => v.Errors)
                .Select(e => e.ErrorMessage));

            var details = new ErrorDto
            {
                Code = StatusCodes.Status400BadRequest,
                Message = message
            };

            context.Result = new BadRequestObjectResult(details);
        }
    }
}