using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;

namespace Identity.Services
{
    /// <summary>
    /// Login service
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface ILoginService<T>
    {
        /// <summary>
        /// Validate user login and password
        /// </summary>
        /// <param name="user"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        Task<bool> ValidateCredentials(T user, string password);

        /// <summary>
        /// Find user by name
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        Task<T> FindByUsername(string user);

        /// <summary>
        /// Sign in
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        Task SignIn(T user);

        /// <summary>
        /// Sign in async
        /// </summary>
        /// <param name="user"></param>
        /// <param name="properties"></param>
        /// <param name="authenticationMethod"></param>
        /// <returns></returns>
        Task SignInAsync(T user, AuthenticationProperties properties, string authenticationMethod = null);
    }
}