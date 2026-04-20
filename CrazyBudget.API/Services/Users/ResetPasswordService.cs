using CrazyBudget.API.Entities;
using CrazyBudget.API.Interfaces;
using CrazyBudget.API.Models.Users;
using Microsoft.EntityFrameworkCore;

namespace CrazyBudget.API.Services.Users;

public class ResetPasswordService: IResetPasswordService
{
    private readonly IAppDbContext dbContext;
    private readonly IEncryptor encryptor;
    public ResetPasswordService(IAppDbContext dbContext, IEncryptor encryptor)
    {
            this.dbContext = dbContext;
            this.encryptor = encryptor; 
    }
    public async Task ResetPassword(ResetPasswordModel resetPasswordModel)
    {

        var user = await this.dbContext.Users.FirstOrDefaultAsync(u => u.Id == resetPasswordModel.UserId);
        if(user == null) {
            throw new Exception("User not found");
        }

        var passwordHash = encryptor.GenerateHash(resetPasswordModel.Password, user.Salt);

        user.PasswordHash = passwordHash;
        await this.dbContext.SaveChangesAsync();

    }
}
