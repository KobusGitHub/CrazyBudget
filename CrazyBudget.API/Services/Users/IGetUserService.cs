using CrazyBudget.API.Models.Users;

namespace CrazyBudget.API.Interfaces;

public interface IGetUserService
{
    Task<UserModel> GetUserById(Guid userId);
}
