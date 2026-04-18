namespace CrazyBudget.API.Services.Auth;

public class IssuedTokenModel
{
    public string Token { get; set; }
    public int ExpiresIn { get; set; }
}
