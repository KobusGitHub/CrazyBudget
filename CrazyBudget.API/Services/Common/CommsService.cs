using System.Net;
using System.Net.Mail;
using CrazyBudget.API.Interfaces;
using CrazyBudget.API.Models.Users;
using Microsoft.EntityFrameworkCore;

namespace CrazyBudget.API.Services.Common;

public class CommsService: ICommsService
{
    private readonly IAppDbContext dbContext;
    public CommsService(IAppDbContext dbContext)
    {
        this.dbContext = dbContext;
    }
    public async Task SendEmail(CommsModel model)
    {
        var configs = await dbContext.Configs.ToListAsync();

        // helper to safely retrieve a single config value and provide clear errors
        string GetConfigValue(string key)
        {
            var matches = configs.Where(x => x.ConfigSetting == key).ToList();
            if (matches.Count == 0)
                throw new InvalidOperationException($"Missing configuration setting '{key}'.");
            if (matches.Count > 1)
                throw new InvalidOperationException($"Multiple configuration entries found for '{key}'.");
            return matches[0].ConfigValue;
        }

        var mailClientHost = GetConfigValue("MailClientHost");
        var mailPortStr = GetConfigValue("MailPort");
        if (!int.TryParse(mailPortStr, out var mailPort))
            throw new InvalidOperationException($"Configuration value for 'MailPort' is not a valid integer: '{mailPortStr}'.");

        var mailUsername = GetConfigValue("MailUsername");
        var mailPassword = GetConfigValue("MailPassword");
        var mailFromAddress = GetConfigValue("MailFromAddress");
        var mailDisplayName = GetConfigValue("MailDisplayName");


        var fromAddress = new MailAddress(mailFromAddress, mailDisplayName);
        var toAddress = new MailAddress(model.Recipient);

        using var mail = new MailMessage(fromAddress, toAddress)
        {
            Subject = "Budget Update from CrazyBudget",
            Body = model.Message ?? "Hello, this is a test email sent from the CrazyBudget C# application.",
            IsBodyHtml = false
        };

        using var client = new SmtpClient(mailClientHost, mailPort)
        {
            EnableSsl = true,
            Credentials = new NetworkCredential(mailUsername, mailPassword)
        };

       

        await client.SendMailAsync(mail);
    }
}
