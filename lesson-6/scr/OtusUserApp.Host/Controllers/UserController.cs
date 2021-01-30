using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
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
        private readonly IUserService _userService;

        /// <inheritdoc />
        public UserController(IUserService userService)
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
        public async Task<ActionResult<UserDto>> CreateUserAsync([FromBody] UserCreationDto user)
        {
            if (user == default)
            {
                return BadRequest(new ErrorDto
                {
                    Code = 400,
                    Message = "Unrecognized user structure"
                });
            }
            
            try
            {
                var dbUser = await _userService.CreateUserAsync(user.ConvertToUser());
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
        /// <param name="userId"> идентификатор пользователя </param>
        /// <returns> пользователь </returns>
        /// <response code="200"> Пользователь успешно найден </response>
        /// <response code="400"> Неверные входные данные. </response>
        /// <response code="404"> Не найден пользователь. </response>
        /// <response code="500"> Ошибка сервера. </response>
        [HttpGet("{userId}")]
        [ProducesResponseType(typeof(UserDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorDto), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ErrorDto), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<UserDto>> GetUserAsync(long userId)
        {
            if (userId <= 0)
            {
                return BadRequest(new ErrorDto
                {
                    Code = 400,
                    Message = "Unrecognized user id"
                });
            }

            try
            {
                var dbUser = await _userService.GetUserAsync(userId);
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
        /// <param name="userId"> идентификатор пользователя </param>
        /// <param name="user"> параметры пользователя </param>
        /// <returns> результат выполнения операции. </returns>
        /// <response code="200"> Пользователь успешно отредактирован </response>
        /// <response code="400"> Неверные входные данные. </response>
        /// <response code="404"> Не найден пользователь. </response>
        /// <response code="500"> Ошибка сервера. </response>
        [HttpPut("{userId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorDto), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ErrorDto), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> UpdateUserAsync(long userId, [FromBody] UserParamsDto user)
        {
            if (user == default || userId <= 0)
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
                dbUser.Id = userId;
                
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
        /// <param name="userId"> идентификатор пользователя </param>
        /// <returns> результат выполнения операции. </returns>
        /// <response code="200"> Пользователь успешно удален </response>
        /// <response code="400"> Неверные входные данные. </response>
        /// <response code="404"> Не найден пользователь. </response>
        /// <response code="500"> Ошибка сервера. </response>
        [HttpDelete("{userId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorDto), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ErrorDto), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> RemoveUserAsync(long userId)
        {
            if (userId <= 0)
            {
                return BadRequest(new ErrorDto
                {
                    Code = 400,
                    Message = "Unrecognized user id"
                });
            }

            try
            {
                await _userService.RemoveUserAsync(userId);
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