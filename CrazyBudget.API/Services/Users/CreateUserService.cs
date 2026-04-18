using CrazyBudget.API.Data;
using CrazyBudget.API.Entities;
using CrazyBudget.API.Interfaces;
using CrazyBudget.API.Models.Users;
using CrazyBudget.API.Services.Common;
using Microsoft.EntityFrameworkCore;

namespace CrazyBudget.API.Services.Users;

public class CreateUserService: ICreateUserService
{
    private readonly IAppDbContext dbContext;
    private readonly IEncryptor encryptor;
    private readonly ICurrentUserService currentUserService;

    public CreateUserService(IAppDbContext dbContext, IEncryptor encryptor, ICurrentUserService currentUserService)
    {
        this.dbContext = dbContext;
        this.encryptor = encryptor;
        this.currentUserService = currentUserService;
    }

    public async Task<Guid> CreateUser(CreateUserModel createModel)
    {
        
        var user = await this.dbContext.Users.FirstOrDefaultAsync(x => x.Username == createModel.Username);
        if(user != null)
        {
            return user.Id;
        }

        var salt = this.encryptor.GenerateSalt();

        var entity = new User
        {
            IsActive = true,
            Username = createModel.Username,
            Firstname = createModel.Firstname,
            Lastname = createModel.Lastname,
            Email = createModel.Email,
            PasswordHash = this.encryptor.GenerateHash(createModel.Password, salt),
            Salt = salt,
            CreatedBy = currentUserService.Username,
            CreatedDate = DateTime.UtcNow,

        };
        this.dbContext.Users.Add(entity);
        AddRoles(createModel.Roles, entity);

        await this.dbContext.SaveChangesAsync();

        return entity.Id;
    }

    private void AddRoles(RoleEnum[] roles, User user)
    {
        foreach (var role in roles)
        {
            var roleEntity = this.dbContext.Roles.FirstOrDefault(x => x.Id == role);
            if (roleEntity != null)
            {
                user.UserRoles.Add(new UserRole
                {
                    Role = roleEntity,
                    User = user
                });
            }
        }
    }
}
