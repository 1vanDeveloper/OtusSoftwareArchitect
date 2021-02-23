using System.ComponentModel.DataAnnotations;

namespace Identity.Models.Account
{
    /// <summary>
    /// Register DTO-model
    /// </summary>
    public record RegisterDto
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
    }
}