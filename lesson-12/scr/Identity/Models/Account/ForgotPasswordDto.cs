using System.ComponentModel.DataAnnotations;

namespace Identity.Models.Account
{
    /// <summary>
    /// Forgot password DTO-model
    /// </summary>
    public record ForgotPasswordDto
    {
        /// <summary>
        /// User email address
        /// </summary>
        [Required]
        [EmailAddress]
        public string Email { get; init; }
    }
}