namespace CrazyBudget.API.Models.Users;

public class ResetPasswordModel
{
    public Guid UserId { get; set; }
    public string Password { get; set; }
}
