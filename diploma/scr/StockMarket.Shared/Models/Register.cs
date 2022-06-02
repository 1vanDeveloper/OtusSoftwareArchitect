using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace StockMarket.Shared.Models;

public class Register
{
    [EmailAddress]
    [Required]
    [JsonPropertyName("email")]
    public string EmailAddress { get; set; }
    
    [Required]
    [StringLength(100, MinimumLength = 8)]
    [JsonPropertyName("password")]
    public string Password { get; set; }
    
    [Required]
    [StringLength(100, MinimumLength = 8)]
    [JsonPropertyName("confirmPassword")]
    public string ConfirmPassword { get; set; }
}