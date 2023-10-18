using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.Models;

[Index(nameof(UserName), IsUnique = true)]
public class User
{
    [Required]
    public int Id { get; set; }
    [Required]
    public string? UserName { get; set; }
    [Required]
    public string? Password { get; set; }
    public ICollection<User>? Friends { get; set; }
    
    public string? RefreshToken { get; set; }
    public DateTime RefreshTokenExpiryTime { get; set; }
    
    public void AddFriend(User friend)
    {
        Friends ??= new List<User>();
        Friends.Add(friend);
    }
}