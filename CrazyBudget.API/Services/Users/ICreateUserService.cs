using CrazyBudget.API.Models.Users;

namespace CrazyBudget.API.Services.Users;

public interface ICreateUserService
{
    Task<Guid> CreateUser(CreateUserModel createModel);
}
