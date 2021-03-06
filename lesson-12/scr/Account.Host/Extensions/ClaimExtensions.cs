using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using IdentityModel;

namespace Account.Host.Extensions
{
    /// <summary>
    /// Класс расширений для <see cref="Claim"/>.
    /// </summary>
    public static class ClaimExtensions
    {
        /// <summary>
        /// Получение логина пользователя.
        /// </summary>
        /// <param name="claims"> Claims. </param>
        /// <returns> Значение claim'а. </returns>
        public static string GetUserName(this IEnumerable<Claim> claims)
        {
            var userIdClaim = claims.First(claim => claim.Type == JwtClaimTypes.PreferredUserName);

            return userIdClaim.Value;
        }
    }
}