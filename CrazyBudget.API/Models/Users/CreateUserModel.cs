using CrazyBudget.API.Entities;

namespace CrazyBudget.API.Models.Users;

public class CreateUserModel
{
    public string Username { get; set; }
    public string Firstname { get; set; }   
    public string Lastname { get; set; }
    public string Password { get; set; }
    public string Email { get; set; }
    public RoleEnum[] Roles { get; set; }


}
