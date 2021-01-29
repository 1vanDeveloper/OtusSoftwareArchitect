using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using OtusUserApp.Domain.Services;
using OtusUserApp.Host.Models;

namespace OtusUserApp.Host.Controllers
{
    /// <summary>
    /// Контроллер управления пользоватеями
    /// </summary>
    [ApiController]
    [Produces("application/json")]
    [Route("user")]
    public class UserController : Controller
    {
        private readonly ILogger<UserController> _logger;
        private readonly IUserService _userService;
        
        public UserController(ILogger<UserController> logger, IUserService userService)
        {
            _userService = userService;
            _logger = logger;
        }

        /// <summary>
        ///     Создание пользователя.
        /// </summary>
        /// <param name="user"> параметры нового пользователя </param>
        /// <returns> новый пользователь </returns>
        /// <response code="200"> Пользователь успешно создан </response>
        /// <response code="400"> Неверные входные данные. </response>
        /// <response code="404"> Не найдена группа. </response>
        [HttpPost]
        [ProducesResponseType(typeof(UserDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorDto), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<UserDto>> CreateUserAsync([FromBody] UserParamsDto user)
        {
            throw new NotImplementedException();
        }
        
        /// <summary>
        ///     Получение пользователя.
        /// </summary>
        /// <param name="userId"> идентификатор пользователя </param>
        /// <returns> пользователь </returns>
        /// <response code="200"> Пользователь успешно создан </response>
        /// <response code="400"> Неверные входные данные. </response>
        /// <response code="404"> Не найдена группа. </response>
        [HttpGet("{userId}")]
        [ProducesResponseType(typeof(UserDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorDto), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<UserDto>> GetUserAsync(long userId)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        ///     Редактирование параметров пользователя.
        /// </summary>
        /// <param name="userId"> идентификатор пользователя </param>
        /// <param name="user"> параметры пользователя </param>
        /// <returns> результат выполнения операции. </returns>
        /// <response code="200"> Пользователь успешно создан </response>
        /// <response code="400"> Неверные входные данные. </response>
        /// <response code="404"> Не найдена группа. </response>
        [HttpPut("{userId}")]
        [ProducesResponseType(typeof(UserDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorDto), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorDto), StatusCodes.Status404NotFound)]
        public async Task<ActionResult> UpdateUserAsync(long userId, [FromBody] UserParamsDto user)
        {
            throw new NotImplementedException();
        }
        
        /// <summary>
        ///     Удаление пользователя.
        /// </summary>
        /// <param name="userId"> идентификатор пользователя </param>
        /// <returns> результат выполнения операции. </returns>
        /// <response code="200"> Пользователь успешно создан </response>
        /// <response code="400"> Неверные входные данные. </response>
        /// <response code="404"> Не найдена группа. </response>
        [HttpDelete("{userId}")]
        [ProducesResponseType(typeof(UserDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorDto), StatusCodes.Status404NotFound)]
        public async Task<ActionResult> RemoveUserAsync(long userId)
        {
            throw new NotImplementedException();
        }
    }
}