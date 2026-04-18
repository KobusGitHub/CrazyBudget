namespace CrazyBudget.API.Entities;

public class UserRole
{
    public int Id { get; set; }
    public string CreatedBy { get; set; }
    public DateTime CreatedDate { get; set; }
    public string LastModifiedBy { get; set; }
    public DateTime? LastModifiedDate { get; set; }

    public Guid UserId { get; set; }
    public RoleEnum RoleId { get; set; }
    public Role Role { get; set; }
    public User User { get; set; }


}

public enum RoleEnum
{
    Administrator = 100,
    User = 200
}
