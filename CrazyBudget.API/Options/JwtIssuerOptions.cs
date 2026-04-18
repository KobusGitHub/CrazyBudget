using Microsoft.IdentityModel.Tokens;

namespace CrazyBudget.API.Options;

public class JwtIssuerOptions
{
    public string Issuer { get; set; }

    public string Subject { get; set; }

    public string Audience { get; set; }

    public DateTime NotBefore => DateTime.UtcNow;

    public DateTime IssuedAt => DateTime.UtcNow;

    public TimeSpan ValidFor { get; set; } = TimeSpan.FromDays(7);

    public DateTime Expiration => IssuedAt.Add(ValidFor);

    public Func<Task<string>> JtiGenerator => () => Task.FromResult(Guid.NewGuid().ToString());

    public SigningCredentials SigningCredentials { get; set; }

    public string SecretKey { get; set; }   
}
