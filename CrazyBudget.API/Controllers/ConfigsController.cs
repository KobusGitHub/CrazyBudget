using CrazyBudget.API.Interfaces;
using CrazyBudget.API.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CrazyBudget.API.Controllers;

[ApiController]
[Route("Configs")]
[Authorize]
public class ConfigsController : ControllerBase
{
    private readonly IAppDbContext appDbContext;

    public ConfigsController(IAppDbContext appDbContext)
    {
        this.appDbContext = appDbContext;
    }

    [HttpGet(Name = "GetConfigs")]
    public async Task<ActionResult<IEnumerable<Config>>> GetConfigs(CancellationToken cancellationToken)
    {
        var dd = await this.appDbContext.Configs.ToListAsync(cancellationToken);
        return Ok(dd);
    }
}
