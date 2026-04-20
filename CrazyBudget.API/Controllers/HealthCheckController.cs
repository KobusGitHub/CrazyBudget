using CrazyBudget.API.Data;
using CrazyBudget.API.Entities;
using CrazyBudget.API.Interfaces;
using CrazyBudget.API.Models.Users;
using CrazyBudget.API.Services.Auth;
using CrazyBudget.API.Services.Common;
using CrazyBudget.API.Services.Users;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CrazyBudget.API.Controllers;


[Route("HealthCheck")]
[Authorize]
public class HealthCheckController : ControllerBase
{
    private readonly IAppDbContext appDbContext;


    public HealthCheckController(IAppDbContext appDbContext)
    {
        this.appDbContext = appDbContext;
    }

    [HttpGet("GetVersionNumber")]
    [Produces(typeof(string))]
    [AllowAnonymous]
    public async Task<ActionResult<string>> GetVersionNumber()
    {

        var configs = await appDbContext.Configs.Where(x => x.ConfigSetting == "VersionNumber").FirstOrDefaultAsync();
       

        return Ok(configs.ConfigValue);
    }

    
}
