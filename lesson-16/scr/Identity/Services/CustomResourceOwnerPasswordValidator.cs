using System.Threading.Tasks;
using Identity.Models;
using IdentityModel;
using IdentityServer4.Validation;

namespace Identity.Services
{
    /// <inheritdoc />
    public class CustomResourceOwnerPasswordValidator: IResourceOwnerPasswordValidator
    {
        private readonly ILoginService<ApplicationUser> _loginService;
        
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="loginService"></param>
        public CustomResourceOwnerPasswordValidator(ILoginService<ApplicationUser> loginService)
        {
            _loginService = loginService;
        }

        /// <inheritdoc />
        public async Task ValidateAsync(ResourceOwnerPasswordValidationContext context)
        {
            var user = await _loginService.FindByUsername(context.UserName);
            
            if (user != null && await _loginService.ValidateCredentials(user, context.Password))
            {
                
                context.Result = new GrantValidationResult(user.Id, OidcConstants.AuthenticationMethods.Password);
            }
        }
    }
}