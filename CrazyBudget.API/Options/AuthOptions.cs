namespace CrazyBudget.API.Options;

public class AuthOptions
{
    public int PasswordAttemptBeforeLockout { get; set; }
    public int LogoutTimeInSeconds { get; set; }
    public string AllowOrigins { get; set; }
    public bool UseCors { get; set; }
}
