namespace CrazyBudget.API.Entities;

public class Role
{

    public Role()
    {
        this.UserRoles = new List<UserRole>();
    }
    public RoleEnum Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public IList<UserRole> UserRoles { get; set; }
}
