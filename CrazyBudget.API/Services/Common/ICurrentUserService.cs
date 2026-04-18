namespace CrazyBudget.API.Services.Common;

public interface ICurrentUserService
{
    Guid? UserId { get; }
    string Username { get; }
}
