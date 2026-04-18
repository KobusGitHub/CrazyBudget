using CrazyBudget.API.Entities;

namespace CrazyBudget.API.Models.Users;

public class RoleModel
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public IList<UserModel> Users { get; set; }
}
