using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Identity.Models;
using Identity.Models.Account;
using Identity.Services;
using IdentityModel;
using IdentityServer4;
using IdentityServer4.Services;
using IdentityServer4.Stores;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Identity.Controllers
{
    /// <summary>
    /// Account management controller
    /// </summary>
    [ApiController]
    [Produces("application/json")]
    [Route("account")]
    public class AccountController : Controller
    {
        private readonly ILoginService<ApplicationUser> _loginService;
        private readonly IIdentityServerInteractionService _interaction;
        private readonly IClientStore _clientStore;
        private readonly ILogger<AccountController> _logger;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IConfiguration _configuration;

        /// <inheritdoc />
        public AccountController(
            ILoginService<ApplicationUser> loginService,
            IIdentityServerInteractionService interaction,
            IClientStore clientStore,
            ILogger<AccountController> logger,
            UserManager<ApplicationUser> userManager,
            IConfiguration configuration)
        {
            _loginService = loginService;
            _interaction = interaction;
            _clientStore = clientStore;
            _logger = logger;
            _userManager = userManager;
            _configuration = configuration;
        }
        
        /// <summary>
        /// Handle postback from username/password login
        /// </summary>
        /// <returns> Login result </returns>
        /// <response code="200"> Success </response>
        /// <response code="400"> Filed </response>
        /// <response code="500"> Server errors </response>
        [HttpPost("login")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorDto), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorDto), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Login(LoginDto model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(new ErrorDto
                    {
                        Code = 400,
                        Message = "Invalid input data."
                    });
                }
            
                var user = await _loginService.FindByUsername(model.Email);
                if (!await _loginService.ValidateCredentials(user, model.Password))
                {
                    return BadRequest(new ErrorDto
                    {
                        Code = 400,
                        Message = "Invalid username or password."
                    });
                }

                var tokenLifetime = _configuration.GetValue("TokenLifetimeMinutes", 120);
                var props = new AuthenticationProperties
                {
                    ExpiresUtc = DateTimeOffset.UtcNow.AddMinutes(tokenLifetime),
                    AllowRefresh = true
                };

                await _loginService.SignInAsync(user, props);
                return Ok();
            }
            catch (Exception e)
            {
                _logger.LogError(e, "LOGIN ERROR: {ExceptionMessage}", e.Message);
                return StatusCode(500, new ErrorDto
                {
                    Code = 500,
                    Message = e.Message
                });
            }
        }
        
        /// <summary>
        /// Handle logout page postback
        /// </summary>
        /// <returns> Logout result </returns>
        /// <response code="200"> Success </response>
        /// <response code="500"> Server errors </response>
        [HttpPost("logout")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorDto), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Logout(LogoutDto model)
        {
            try
            {
                var idp = User?.FindFirst(JwtClaimTypes.IdentityProvider)?.Value;
                if (idp != null && idp != IdentityServerConstants.LocalIdentityProvider)
                {
                    model.LogoutId ??= await _interaction.CreateLogoutContextAsync();
                    try
                    {
                        // hack: try/catch to handle social providers that throw
                        await HttpContext.SignOutAsync(idp, new AuthenticationProperties());
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, "LOGOUT ERROR: {ExceptionMessage}", ex.Message);
                    }
                }

                // delete authentication cookie
                await HttpContext.SignOutAsync();
                await HttpContext.SignOutAsync(IdentityConstants.ApplicationScheme);

                // set this so UI rendering sees an anonymous user
                HttpContext.User = new ClaimsPrincipal(new ClaimsIdentity());

                // get context information (client name, post logout redirect URI and iframe for federated signout)
                var logout = await _interaction.GetLogoutContextAsync(model.LogoutId);

                return Ok();
            }
            catch (Exception e)
            {
                _logger.LogError(e, "LOGOUT ERROR: {ExceptionMessage}", e.Message);
                return StatusCode(500, new ErrorDto
                {
                    Code = 500,
                    Message = e.Message
                });
            }
        }
        
        /// <summary>
        /// Register new account
        /// </summary>
        /// <param name="model"></param>
        /// <returns> Register result </returns>
        /// <response code="200"> Success </response>
        /// <response code="400"> Filed </response>
        /// <response code="500"> Server errors </response>
        [HttpPost("register")]
        [AllowAnonymous]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorDto), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorDto), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Register(RegisterDto model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new ErrorDto
                {
                    Code = 400,
                    Message = "Invalid input data."
                });
            }
            
            var user = new ApplicationUser
            {
                UserName = model.Email,
                Email = model.Email,
            };
            
            var result = await _userManager.CreateAsync(user, model.Password);
            if (result.Errors.Any())
            {
                return BadRequest(result);
            }

            return Ok(result);
        }
    }
}