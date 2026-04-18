namespace CrazyBudget.API.Entities;

public class UserLogin
{
    public int Id { get; set; }
    public DateTime DateTimeIn { get; set; }
    public Guid UserId { get; set; }
    public User User { get; set; }
}
