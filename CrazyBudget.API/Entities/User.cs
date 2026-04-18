namespace CrazyBudget.API.Entities;

public class User
{
    public Guid Id { get; set; }

    public string CreatedBy { get; set; }
    public DateTime CreatedDate { get; set; }
    public string LastModifiedBy { get; set; }
    public DateTime? LastModifiedDate { get; set; }

    public string Username { get; set; }
    public string Firstname { get; set; }
    public string Lastname { get; set; }
    public string Email { get; set; }
    public int? FailedLoginAttempts { get; set; }
    public DateTime? LockoutExpiryDate { get; set; }
    public byte[] Salt { get; set; }

    public string PasswordHash { get; set; }
    public bool IsActive { get; set; }

    public IList<UserRole> UserRoles { get; set; }
    public IList<UserLogin> UserLogins { get; set; }
    //public IList<PasswordResetRequest> PasswordResetRequests { get; set; }

}
