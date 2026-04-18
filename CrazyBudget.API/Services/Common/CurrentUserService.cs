using System.Security.Claims;

namespace CrazyBudget.API.Services.Common;

public class CurrentUserService: ICurrentUserService
{
    private readonly IHttpContextAccessor httpContextAccessor;

    public CurrentUserService(IHttpContextAccessor httpContextAccessor)
    {
        this.httpContextAccessor = httpContextAccessor;
    }

    public string Username
    {
        get
        {
            return httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier) ?? "Anonymous";
        }
    }

    public Guid? UserId
    {
        get
        {
            var userId = Guid.Empty;
            if(httpContextAccessor.HttpContext != null)
            {
                if(httpContextAccessor.HttpContext.User.FindFirst("uid") != null)
                {
                    userId = new Guid(httpContextAccessor.HttpContext.User.FindFirst("uid").Value);
                }
            }

            return userId;
        }
    }
}
