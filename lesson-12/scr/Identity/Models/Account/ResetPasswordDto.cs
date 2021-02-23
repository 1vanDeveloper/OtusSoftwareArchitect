using System.ComponentModel.DataAnnotations;

namespace Identity.Models.Account
{
    /// <summary>
    /// Reset password DTO-model
    /// </summary>
    public record ResetPasswordDto
    {
        /// <summary>
        /// User email address
        /// </summary>
        [Required]
        [EmailAddress]
        public string Email { get; init; }

        /// <summary>
        /// User password
        /// </summary>
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; init; }

        /// <summary>
        /// User confirm password
        /// </summary>
        [DataType(DataType.Password)]
        [Compare(nameof(Password))]
        public string ConfirmPassword { get; init; }

        /// <summary>
        /// Received code
        /// </summary>
        public string Code { get; init; }
    }
}