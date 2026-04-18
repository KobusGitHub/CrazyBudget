using System.Security.Authentication;
using CrazyBudget.API.Interfaces;
using CrazyBudget.API.Options;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace CrazyBudget.API.Services.Auth;

public class AuthenticateUserService: IAuthenticateUserService
{
    private readonly IAuthService authService;
    private readonly IAppDbContext dbContext;
    private readonly IOptions<AuthOptions> authOptions;

    public AuthenticateUserService(IAuthService authService, IAppDbContext dbContext, IOptions<AuthOptions> authOptions)
    {
        this.authService = authService;
        this.dbContext = dbContext;
        this.authOptions = authOptions;
    }

    public async Task<IssuedTokenModel> AuthenticateUser(AuthenticateUserModel model)
    {
        var user = await this.dbContext.Users
            .Include(i => i.UserRoles)
            .Include(ii => ii.UserLogins)
            .FirstOrDefaultAsync(x => x.Username == model.Username);
        if (user == null)
        {
            throw new Exception("Invalid username or password");
        }


        if(await authService.ValidateCredentials(user, model.Password))
        {
            var issuedToken = await authService.IssueToken(user);
            await RecordSuccessfulLogin(user);
            
            IssuedTokenModel issuedTokenModel = new IssuedTokenModel
            {
                Token = issuedToken.Token,
                ExpiresIn = issuedToken.ExpiresIn
            };

            return issuedTokenModel;
        }

        await RecordFailedLogin(user);

        throw new AuthenticationException("Authentication Failed");


    }

    private async Task RecordSuccessfulLogin(Entities.User user)
    {
        user.FailedLoginAttempts = 0;
        user.LockoutExpiryDate = null;

        user.UserLogins.Add(new Entities.UserLogin
        {
            UserId = user.Id,
            DateTimeIn = DateTime.Now,
        });
        await dbContext.SaveChangesAsync();
    }

    private async Task RecordFailedLogin(Entities.User user)
    {
        user.FailedLoginAttempts += 1;
        if (user.FailedLoginAttempts >= this.authOptions.Value.PasswordAttemptBeforeLockout)
        {
            user.LockoutExpiryDate = DateTime.Now.AddSeconds(this.authOptions.Value.LogoutTimeInSeconds);
        }
        await dbContext.SaveChangesAsync();
    }
}
