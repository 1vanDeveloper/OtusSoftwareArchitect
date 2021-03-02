using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Account.Domain.Services;
using Account.Host.Extensions;
using Account.Host.Models;
using Microsoft.AspNetCore.Authentication;
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
        /// <returns> новый пользователь </returns>
        /// <response code="200"> Пользователь успешно создан </response>
        /// <response code="400"> Неверные входные данные. </response>
        /// <response code="500"> Ошибка сервера. </response>
        [HttpPost]
        [ProducesResponseType(typeof(UserDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorDto), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorDto), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<UserDto>> CreateUserAsync([FromBody] UserParamsDto user)
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
                
                var dbUser = await _userService.CreateUserAsync(newUser);
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
        public async Task<ActionResult<UserDto>> GetUserAsync(string userName)
        {
            var currentUserName = HttpContext.User.Claims.GetUserName();
            if (currentUserName != userName)
            {
                return Forbid();
            }
            
            if (string.IsNullOrWhiteSpace(userName))
            {
                return NotFound(new ErrorDto
                {
                    Code = 404,
                    Message = "Unrecognized user name"
                });
            }

            try
            {
                var dbUser = await _userService.GetUserAsync(userName);
                return Ok(new UserDto(dbUser));
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
        ///     Редактирование параметров пользователя.
        /// </summary>
        /// <param name="userName"> логин пользователя </param>
        /// <param name="user"> параметры пользователя </param>
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
        public async Task<ActionResult> UpdateUserAsync(string userName, [FromBody] UserParamsDto user)
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
                var oldDbUser = await _userService.GetUserAsync(userName);
                dbUser.Id = oldDbUser.Id;
                dbUser.UserName = oldDbUser.UserName;
                
                await _userService.UpdateUserAsync(dbUser);
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
        public async Task<ActionResult> RemoveUserAsync(string userName)
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
                await _userService.RemoveUserAsync(userName);
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