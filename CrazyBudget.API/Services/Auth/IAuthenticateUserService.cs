namespace CrazyBudget.API.Services.Auth;

public interface IAuthenticateUserService
{
    Task<IssuedTokenModel> AuthenticateUser(AuthenticateUserModel model);
}
