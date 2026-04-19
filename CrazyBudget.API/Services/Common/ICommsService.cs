using CrazyBudget.API.Models.Users;

namespace CrazyBudget.API.Services.Common;

public interface ICommsService
{
    Task SendEmail(CommsModel model);
}
