using CrazyBudget.API.Data;
using CrazyBudget.API.Entities;
using CrazyBudget.API.Interfaces;
using CrazyBudget.API.Models.Users;
using Microsoft.EntityFrameworkCore;

namespace CrazyBudget.API.Services.Users;

public class GetUserService: IGetUserService
{
    private readonly IAppDbContext dbContext;
    private readonly IEncryptor encryptor;

    public GetUserService(IAppDbContext dbContext, IEncryptor encryptor)
    {
        this.dbContext = dbContext;
        this.encryptor = encryptor;
    }

    public async Task<UserModel> GetUserById(Guid userId)
    {
        
        var userEnt = await this.dbContext.Users
            .Include(i => i.UserRoles)
            .ThenInclude(th => th.Role)
            .FirstOrDefaultAsync(x => x.Id == userId);

        if(userEnt == null)
        {
            throw new Exception("User not found");
        }

        var userModel = new UserModel()
        {
            Id = userId,
            Username = userEnt.Username,
            CreatedDate = userEnt.CreatedDate,
            CreatedBy = userEnt.CreatedBy,
            Email = userEnt.Email,
            FailedLoginAttempts = userEnt.FailedLoginAttempts,
            Firstname = userEnt.Firstname,
            IsActive = userEnt.IsActive,
            LastModifiedBy = userEnt.LastModifiedBy,
            LastModifiedDate = userEnt.LastModifiedDate,
            Lastname = userEnt.Lastname,
            LockoutExpiryDate = userEnt.LockoutExpiryDate,
            Roles = userEnt.UserRoles.Select(x => new RoleModel
            {
                Id = (int)x.Role.Id,
                Name = x.Role.Name
            }).ToList()

        };


        return userModel;
    }

}
