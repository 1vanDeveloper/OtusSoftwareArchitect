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
       private readonly ILogger<AccountController> _logger;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IConfiguration _configuration;

        /// <inheritdoc />
        public AccountController(
            ILoginService<ApplicationUser> loginService,
            IIdentityServerInteractionService interaction,
            ILogger<AccountController> logger,
            UserManager<ApplicationUser> userManager,
            IConfiguration configuration)
        {
            _loginService = loginService;
            _interaction = interaction;
            _logger = logger;
            _userManager = userManager;
            _configuration = configuration;
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