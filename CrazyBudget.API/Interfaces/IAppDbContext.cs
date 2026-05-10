using CrazyBudget.API.Entities;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;

namespace CrazyBudget.API.Interfaces;

public interface IAppDbContext
{
    public DbSet<Config> Configs { get; set; }
    public DbSet<User> Users { get; set; }
    public DbSet<Role> Roles { get; set; }
    public DbSet<UserRole> UserRoles { get; set; }
    public DbSet<UserLogin> UserLogins { get; set; }
    public DbSet<SmsExpense> SmsExpenses { get; set; }
    public DbSet<Expense> Expenses { get; set; }
    public DbSet<Category> Categories { get; set; }
    public Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}
