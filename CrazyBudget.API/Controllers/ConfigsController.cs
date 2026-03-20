using CrazyBudget.API.Data;
using CrazyBudget.API.Entities;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CrazyBudget.API.Controllers;

[ApiController]
[Route("Configs")]
public class ConfigsController : ControllerBase
{
    private readonly AppDbContext appDbContext;

    public ConfigsController(AppDbContext appDbContext)
    {
        this.appDbContext = appDbContext;
    }

    [HttpGet(Name = "GetConfigs")]
    public ActionResult<IEnumerable<Config>> GetConfigs()
    {
        var dd = this.appDbContext.Configs.ToList();
        return Ok(dd);
    }
}
