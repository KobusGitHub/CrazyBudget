using System.Collections.Generic;
using System.Text;
using CrazyBudget.API.Interfaces;
using CrazyBudget.API.Options;
using CrazyBudget.API.Services.Auth;
using CrazyBudget.API.Services.Categories;
using CrazyBudget.API.Services.Common;
using CrazyBudget.API.Services.Users;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
// Register EF Core DbContext and expose it via the IAppDbContext interface
builder.Services.AddDbContext<IAppDbContext, CrazyBudget.API.Data.AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"),
        sqlServerOptionsAction: sqlOptions =>
        {
            sqlOptions.EnableRetryOnFailure(
                maxRetryCount: 5,
                maxRetryDelay: TimeSpan.FromSeconds(10),
                errorNumbersToAdd: null);
        })
    // Enable sensitive data logging only in development
    .EnableSensitiveDataLogging(builder.Environment.IsDevelopment()));
builder.Services.AddScoped<ICreateUserService, CreateUserService>();
builder.Services.AddScoped<IGetUserService, GetUserService>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IAuthenticateUserService, AuthenticateUserService>();
// Register IHttpContextAccessor so services that depend on the current HTTP context can be resolved
builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<ICurrentUserService, CurrentUserService>();
builder.Services.AddScoped<ICommsService, CommsService>();
builder.Services.AddScoped<IResetPasswordService, ResetPasswordService>();
builder.Services.AddScoped<ICategoryService, CategoryService>();



// Configure JWT
var jwtSection = builder.Configuration.GetSection("JwtIssuerOptions");
var jwtIssuerOptions = jwtSection.Get<JwtIssuerOptions>() ?? throw new InvalidOperationException("Configuration section 'JwtIssuerOptions' is missing or invalid.");
if (string.IsNullOrEmpty(jwtIssuerOptions.SecretKey)) throw new InvalidOperationException("JwtIssuerOptions.SecretKey is not configured.");
SymmetricSecurityKey signingKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtIssuerOptions.SecretKey));

// Bind JwtIssuerOptions and set the computed SigningCredentials so services can inject IOptions<JwtIssuerOptions>
builder.Services.Configure<JwtIssuerOptions>(options =>
{
    jwtSection.Bind(options);
    options.SigningCredentials = new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256);
});

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(cfg =>
{
    // Require HTTPS in non-development environments
    cfg.RequireHttpsMetadata = !builder.Environment.IsDevelopment();
    cfg.SaveToken = true;
    cfg.TokenValidationParameters = new TokenValidationParameters()
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = signingKey,
        ValidateIssuer = !string.IsNullOrEmpty(jwtIssuerOptions.Issuer),
        ValidIssuer = jwtIssuerOptions.Issuer,
        ValidateAudience = !string.IsNullOrEmpty(jwtIssuerOptions.Audience),
        ValidAudience = jwtIssuerOptions.Audience,
        ValidateLifetime = true,
        ClockSkew = TimeSpan.FromMinutes(2)
    };

    cfg.Events = new JwtBearerEvents
    {
        OnMessageReceived = context =>
        {
            var accessToken = context.Request.Query["access_token"];
            var path = context.HttpContext.Request.Path;
            if (!string.IsNullOrEmpty(accessToken) && path.StartsWithSegments("/Hub"))
            {
                context.Token = accessToken;
            }

            return Task.CompletedTask;
        }
    };
});






// Register encryptor implementation for dependency injection
builder.Services.AddSingleton<IEncryptor, Encrypter>();

// JwtIssuerOptions already configured above (bound and SigningCredentials set)
builder.Services.Configure<AuthOptions>(builder.Configuration.GetSection("AuthOptions"));


builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "JWT Authorization header using the Bearer scheme. Enter 'Bearer' [space] and then your token.",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT"
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement()
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new List<string>()
        }
    });
});

WebApplication app = null;
try
{
    app = builder.Build();

    // Configure the HTTP request pipeline.
    //if (app.Environment.IsDevelopment())
    //{
    //    app.UseSwagger();
    //    app.UseSwaggerUI(c =>
    //    {
    //        c.SwaggerEndpoint("/swagger/v1/swagger.json", "CRAZY_BUDGET v1");
    //        c.RoutePrefix = ""; // Serve UI at /swagger/index.html
    //    });
    //}

    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "CRAZY_BUDGET v1");
        c.RoutePrefix = ""; // Serve UI at /swagger/index.html
    });



    app.UseHttpsRedirection();

    app.UseAuthentication();
    app.UseAuthorization();

    app.MapControllers();

    app.Run();
}
catch (Exception ex)
{
    // Print full exception to console so `dotnet run` and stdout logs show the real error
    Console.WriteLine("Unhandled exception during app startup:\n" + ex);
    throw;
}
