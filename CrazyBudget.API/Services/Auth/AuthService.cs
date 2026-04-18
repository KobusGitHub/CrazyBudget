using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Principal;
using CrazyBudget.API.Entities;
using CrazyBudget.API.Interfaces;
using CrazyBudget.API.Options;
using Microsoft.Extensions.Options;
using Microsoft.Identity.Client;

namespace CrazyBudget.API.Services.Auth;

public class AuthService: IAuthService
{
    private readonly IEncryptor encryptor;
    private readonly IOptions<JwtIssuerOptions> jwtOptions;

    public AuthService(IEncryptor encryptor, IOptions<JwtIssuerOptions> jwtOptions)
    {
        this.encryptor = encryptor;
        this.jwtOptions = jwtOptions;
        ThrowIfInvalidOptions(jwtOptions.Value);
    }

    public async Task<bool> ValidateCredentials(User user, string password)
    {
        var passwordHash = encryptor.GenerateHash(password, user.Salt);
        return passwordHash == user.PasswordHash ? await Task.FromResult(true) : await Task.FromResult(false);
    }

    public async Task<IssuedToken> IssueToken(User user)
    {
        var identity = await GetClaimIdentity(user);

        var claims = new List<Claim>
        {
            new Claim(JwtRegisteredClaimNames.Sub, user.Username),
            new Claim(JwtRegisteredClaimNames.Jti, await jwtOptions.Value.JtiGenerator()),
            new Claim(JwtRegisteredClaimNames.Iat, ToUnixEpochDate(jwtOptions.Value.IssuedAt).ToString(), ClaimValueTypes.Integer64),

            identity.FindFirst("uid"),
            identity.FindFirst("username"),
            identity.FindFirst("firstname"),
            identity.FindFirst("surname")
        };

        claims.AddRange(identity.Claims.Where(s => s.Type == "roles"));

        var jwt = new JwtSecurityToken(
                issuer: jwtOptions.Value.Issuer,
                audience: jwtOptions.Value.Audience,
                claims: claims,
                notBefore: jwtOptions.Value.NotBefore,
                expires: jwtOptions.Value.Expiration,
                signingCredentials: jwtOptions.Value.SigningCredentials);

        return new IssuedToken
        {
            Token = new JwtSecurityTokenHandler().WriteToken(jwt),
            ExpiresIn = (int)jwtOptions.Value.ValidFor.TotalSeconds
        };
    }



    private async Task<ClaimsIdentity> GetClaimIdentity(User user)
    {
        var identity = new ClaimsIdentity(new GenericIdentity(user.Username, "Token"), new[]
        {
            new Claim("uid", user.Id.ToString()),
            new Claim("username", user.Username),
            new Claim("firstname", user.Firstname),
            new Claim("surname", user.Lastname)
        });

        foreach (var role in user.UserRoles.Select(s => s.Role))
        {
            identity.AddClaim(new Claim("roles", role.Name));
        }

        return await Task.FromResult(identity);
    }

    public static void ThrowIfInvalidOptions(JwtIssuerOptions options)
    {
        if (options == null) throw new ArgumentNullException(nameof(options));
        if (string.IsNullOrEmpty(options.Issuer)) throw new ArgumentException("Issuer must be provided.");
        if (string.IsNullOrEmpty(options.Audience)) throw new ArgumentException("Audience must be provided.");
        if (options.ValidFor <= TimeSpan.Zero) throw new ArgumentException("ValidFor must be a positive timespan.");
        if (options.SigningCredentials == null) throw new ArgumentException("SigningCredentials must be provided.");
        if (string.IsNullOrEmpty(options.SecretKey)) throw new ArgumentException("SecretKey must be provided.");
    }

    private static long ToUnixEpochDate(DateTime date) => (long)Math.Round((date.ToUniversalTime() - new DateTimeOffset(1970, 1, 1, 0, 0, 0, TimeSpan.Zero)).TotalSeconds);
}

