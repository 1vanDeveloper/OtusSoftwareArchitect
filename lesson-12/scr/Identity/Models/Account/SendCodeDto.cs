using System.Collections.Generic;

namespace Identity.Models.Account
{
    /// <summary>
    /// Send code DTO-model
    /// </summary>
    public record SendCodeDto
    {
        /// <summary>
        /// Providers list
        /// </summary>
        public ICollection<string> Providers { get; init; }
    }
}