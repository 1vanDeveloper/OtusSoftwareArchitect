using System.ComponentModel.DataAnnotations;

namespace Identity.Models.Account
{
    /// <summary>
    /// Verify code DTO-model
    /// </summary>
    public record VerifyCodeDto
    {
        /// <summary>
        /// Selected provider
        /// </summary>
        [Required]
        public string Provider { get; init; }

        /// <summary>
        /// Received code
        /// </summary>
        [Required]
        public string Code { get; init; }
    }
}