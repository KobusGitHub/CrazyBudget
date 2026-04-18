using CrazyBudget.API.Data;
using CrazyBudget.API.Entities;
using CrazyBudget.API.Interfaces;
using CrazyBudget.API.Models.Users;
using CrazyBudget.API.Services.Auth;
using CrazyBudget.API.Services.Users;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CrazyBudget.API.Controllers;


[Route("Users")]
[Authorize]
public class UsersController : ControllerBase
{
    private readonly IAppDbContext appDbContext;
    private readonly ICreateUserService createUserService;
    private readonly IGetUserService getUserService;
    private readonly IAuthenticateUserService authenticateUserService;

    public UsersController(IAppDbContext appDbContext, ICreateUserService createUserService, IGetUserService getUserService, IAuthenticateUserService authenticateUserService)
    {
        this.appDbContext = appDbContext;
        this.createUserService = createUserService;
        this.getUserService = getUserService;
        this.authenticateUserService = authenticateUserService;
    }

    [HttpPost]
    [Produces(typeof(UserModel))]
    public async Task<ActionResult<UserModel>> CreateUser(CreateUserModel createModel)
    {

        var userId = await this.createUserService.CreateUser(createModel);

        var userModel = await this.getUserService.GetUserById(userId);

        return Ok(userModel);
    }

    [HttpGet(Name = "GetUserById")]
    public async Task<ActionResult<UserModel>> GetUserById(Guid userId)
    {
        var dd = await this.getUserService.GetUserById(userId);
        return Ok(dd);
    }


    [HttpPost("AuthenticateUser")]
    [Produces(typeof(IssuedTokenModel))]
    [AllowAnonymous]
    public async Task<ActionResult<IssuedTokenModel>> AuthenticateUser(AuthenticateUserModel model)
    {

        var itm = await this.authenticateUserService.AuthenticateUser(model);

        return Ok(itm);
    }
}
