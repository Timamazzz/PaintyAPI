using System.ComponentModel.DataAnnotations;

namespace PaintyAPI.Models;

public class Jwt
{
    [Required]
    public string? AccessToken { get; set; }
    [Required]
    public string? RefreshToken { get; set; }

}