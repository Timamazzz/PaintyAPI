namespace PaintyAPI.Models.User;

public class UserRegister
{
    public string? UserName { get; set; }
    public string? Password { get; set; }
}

public class UserAuth
{
    public string? UserName { get; set; }
    public string? Password { get; set; }
}

public class UserResponse
{
    public int? Id { get; set; }
    public string? UserName { get; set; }
    public ICollection<Friend>? Friends { get; set; }
}

public class Friend
{
    public int Id { get; set; }
    public string? UserName { get; set; }
    public string? Password { get; set; }
}