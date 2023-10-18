namespace Domain.Models;

public class User
{
    public int? Id { get; set; }
    public string? UserName { get; set; }
    public string? Password { get; set; }
    public ICollection<User>? Friends { get; set; }
    public string? RefreshToken { get; set; }
    public DateTime RefreshTokenExpiryTime { get; set; }
}