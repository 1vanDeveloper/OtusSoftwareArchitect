using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Account.Domain.Services;
using Account.Host.Extensions;
using Account.Host.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Account.Host.Controllers
{
    /// <summary>
    /// Контроллер управления пользоватеями
    /// </summary>
    [ApiController]
    [Produces("application/json")]
    [Route("account")]
    [Authorize]
    public class AccountController : Controller
    {
        private readonly IUserService _userService;

        /// <inheritdoc />
        public AccountController(IUserService userService)
        {
            _userService = userService;
        }

        /// <summary>
        ///     Создание пользователя.
        /// </summary>
        /// <param name="user"> параметры нового пользователя </param>
        /// <param name="cancellationToken"></param>
        /// <returns> новый пользователь </returns>
        /// <response code="200"> Пользователь успешно создан </response>
        /// <response code="400"> Неверные входные данные. </response>
        /// <response code="500"> Ошибка сервера. </response>
        [HttpPost]
        [ProducesResponseType(typeof(UserDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorDto), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorDto), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<UserDto>> CreateUserAsync([FromBody] UserParamsDto user, CancellationToken cancellationToken)
        {
            var userName = HttpContext.User.Claims.GetUserName();
            if (user == new UserDto())
            {
                return BadRequest(new ErrorDto
                {
                    Code = 400,
                    Message = "Unrecognized user structure"
                });
            }
            
            try
            {
                var newUser = user.ConvertToUser();
                newUser.UserName = userName;
                
                var dbUser = await _userService.CreateUserAsync(newUser, cancellationToken);
                return Ok(new UserDto(dbUser));
            }
            catch (Exception e)
            {
                return new ObjectResult(new ErrorDto { Code = 500, Message = e.Message })
                {
                    StatusCode = 500
                };
            }
        }

        /// <summary>
        ///     Получение пользователя.
        /// </summary>
        /// <param name="userName"> логин пользователя </param>
        /// <param name="cancellationToken"></param>
        /// <returns> пользователь </returns>
        /// <response code="200"> Пользователь успешно найден </response>
        /// <response code="400"> Неверные входные данные. </response>
        /// <response code="404"> Не найден пользователь. </response>
        /// <response code="500"> Ошибка сервера. </response>
        [HttpGet("{userName}")]
        [ProducesResponseType(typeof(UserDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorDto), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ErrorDto), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<UserDto>> GetUserAsync(string userName, CancellationToken cancellationToken)
        {
            if (string.IsNullOrWhiteSpace(userName))
            {
                return NotFound(new ErrorDto
                {
                    Code = 404,
                    Message = "Empty user name"
                });
            }
            
            var currentUserName = HttpContext.User.Claims.GetUserName();
            if (currentUserName != userName)
            {
                return Forbid();
            }
            
            try
            {
                var dbUser = await _userService.GetUserAsync(userName, cancellationToken);
                return Ok(new UserDto(dbUser));
            }
            catch (KeyNotFoundException)
            {
                return NotFound(new ErrorDto
                {
                    Code = 404,
                    Message = $"Unrecognized user name {userName}"
                });
            }
            catch (Exception e)
            {
                return new ObjectResult(new ErrorDto { Code = 500, Message = e.Message })
                {
                    StatusCode = 500
                };
            }
        }

        /// <summary>
        ///     Редактирование параметров пользователя.
        /// </summary>
        /// <param name="userName"> логин пользователя </param>
        /// <param name="user"> параметры пользователя </param>
        /// <param name="cancellationToken"></param>
        /// <returns> результат выполнения операции. </returns>
        /// <response code="200"> Пользователь успешно отредактирован </response>
        /// <response code="400"> Неверные входные данные. </response>
        /// <response code="404"> Не найден пользователь. </response>
        /// <response code="500"> Ошибка сервера. </response>
        [HttpPut("{userName}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorDto), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ErrorDto), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> UpdateUserAsync(string userName, [FromBody] UserParamsDto user, CancellationToken cancellationToken)
        {
            var currentUserName = HttpContext.User.Claims.GetUserName();
            if (currentUserName != userName)
            {
                return Forbid();
            }
            
            if (user == default || string.IsNullOrWhiteSpace(userName))
            {
                return BadRequest(new ErrorDto
                {
                    Code = 400,
                    Message = "Unrecognized user structure"
                });
            }

            try
            {
                var dbUser = user.ConvertToUser();
                var oldDbUser = await _userService.GetUserAsync(userName, cancellationToken);
                dbUser.Id = oldDbUser.Id;
                dbUser.UserName = oldDbUser.UserName;
                
                await _userService.UpdateUserAsync(dbUser, cancellationToken);
                return Ok();
            }
            catch (KeyNotFoundException)
            {
                return new NotFoundResult();
            }
            catch (Exception e)
            {
                return new ObjectResult(new ErrorDto { Code = 500, Message = e.Message })
                {
                    StatusCode = 500
                };
            }
        }

        /// <summary>
        ///     Удаление пользователя.
        /// </summary>
        /// <param name="userName"> логин пользователя </param>
        /// <param name="cancellationToken"></param>
        /// <returns> результат выполнения операции. </returns>
        /// <response code="200"> Пользователь успешно удален </response>
        /// <response code="400"> Неверные входные данные. </response>
        /// <response code="404"> Не найден пользователь. </response>
        /// <response code="500"> Ошибка сервера. </response>
        [HttpDelete("{userName}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorDto), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ErrorDto), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> RemoveUserAsync(string userName, CancellationToken cancellationToken)
        {
            if (string.IsNullOrWhiteSpace(userName))
            {
                return BadRequest(new ErrorDto
                {
                    Code = 400,
                    Message = "Unrecognized user id"
                });
            }

            try
            {
                await _userService.RemoveUserAsync(userName, cancellationToken);
                return Ok();
            }
            catch (KeyNotFoundException)
            {
                return new NotFoundResult();
            }
            catch (Exception e)
            {
                return new ObjectResult(new ErrorDto { Code = 500, Message = e.Message })
                {
                    StatusCode = 500
                };
            }
        }
    }
}