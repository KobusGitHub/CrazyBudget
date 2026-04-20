using CrazyBudget.API.Models.Users;

namespace CrazyBudget.API.Services.Users;

public interface IResetPasswordService
{
    Task ResetPassword(ResetPasswordModel resetPasswordModel);
}
