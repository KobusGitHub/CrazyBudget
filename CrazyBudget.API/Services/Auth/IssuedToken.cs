namespace CrazyBudget.API.Services.Auth;

public class IssuedToken
{
    public string Token { get; set; }
    public int ExpiresIn { get; set; }
}
