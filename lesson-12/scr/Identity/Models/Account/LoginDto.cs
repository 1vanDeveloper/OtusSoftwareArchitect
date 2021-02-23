using System.ComponentModel.DataAnnotations;

namespace Identity.Models.Account
{
    /// <summary>
    /// Login DTO-model
    /// </summary>
    public record LoginDto
    {
        /// <summary>
        /// User email address
        /// </summary>
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        /// <summary>
        /// User password
        /// </summary>
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}