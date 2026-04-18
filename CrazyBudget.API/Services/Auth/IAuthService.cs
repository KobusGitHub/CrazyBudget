using CrazyBudget.API.Entities;

namespace CrazyBudget.API.Services.Auth;

public interface IAuthService
{
    Task<bool> ValidateCredentials(User user, string password);
    Task<IssuedToken> IssueToken(User user);
}
