using CrazyBudget.API.Data;
using CrazyBudget.API.Entities;
using CrazyBudget.API.Interfaces;
using CrazyBudget.API.Models.Users;
using CrazyBudget.API.Services.Auth;
using CrazyBudget.API.Services.Categories;
using CrazyBudget.API.Services.Common;
using CrazyBudget.API.Services.Users;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CrazyBudget.API.Controllers;


[Route("Users")]
[Authorize]
public class CategoriesController : ControllerBase
{
   
    private readonly ICategoryService categoryService;

    public CategoriesController(ICategoryService categoryService)
    {
        this.categoryService = categoryService;
    }

    [HttpPost("CreateCategory")]
    [Produces(typeof(Task<CategoryModel>))]
    public async Task<ActionResult<CategoryModel>> CreateCategory(CreateCategoryModel createModel)
    {

        var categoryId = await this.categoryService.CreateCategory(createModel);

        var categoryModel = await this.categoryService.GetCategoryById(categoryId);

        return Ok(categoryModel);
    }


    [HttpPost("UpdateCategory")]
    [Produces(typeof(Task<CategoryModel>))]
    public async Task<ActionResult<CategoryModel>> UpdateCategory(UpdateCategoryModel model)
    {

        var categoryId = await this.categoryService.UpdateCategory(model);

        var categoryModel = await this.categoryService.GetCategoryById(categoryId);

        return Ok(categoryModel);
    }

    [HttpPost("DeleteCategory")]
    [Produces(typeof(Task<bool>))]
    public async Task<ActionResult<bool>> DeleteCategory(DeleteCategoryModel model)
    {

        var success = await this.categoryService.DeleteCategory(model);

      
        return Ok(success);
    }

    [HttpPost("GetCategoryById")]
    [Produces(typeof(Task<CategoryModel>))]
    public async Task<ActionResult<CategoryModel>> GetCategoryById(int categoryId)
    {

        var model = await this.categoryService.GetCategoryById(categoryId);


        return Ok(model);
    }
}
