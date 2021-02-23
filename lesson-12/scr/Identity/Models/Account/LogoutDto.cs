namespace Identity.Models.Account
{
    /// <summary>
    /// Logout DTO-model
    /// </summary>
    public record LogoutDto
    {
        /// <summary>
        /// Id of session
        /// </summary>
        public string LogoutId { get; set; }
    }
}