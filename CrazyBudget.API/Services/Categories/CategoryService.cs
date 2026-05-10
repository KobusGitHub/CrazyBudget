using CrazyBudget.API.Entities;
using CrazyBudget.API.Interfaces;
using CrazyBudget.API.Services.Common;

namespace CrazyBudget.API.Services.Categories;

public class CategoryService: ICategoryService
{
    private readonly IAppDbContext dbContext;
    private readonly ICurrentUserService currentUserService;
    public CategoryService(IAppDbContext dbContext, ICurrentUserService currentUserService)
    {
        this.dbContext = dbContext;
        this.currentUserService = currentUserService;
    }

    public async Task<int> CreateCategory(CreateCategoryModel model)
    {
        if (string.IsNullOrEmpty(model.CategoryName))
        {
            throw new ArgumentException("Category name cannot be empty");
        }

        var entity = new Category
        {
            UserId = this.currentUserService.UserId.Value,
            CategoryName = model.CategoryName,
            BudgetAmount = model.BudgetAmount,
            IsFavourite = model.IsFavourite
        };
        this.dbContext.Categories.Add(entity);
        await this.dbContext.SaveChangesAsync();

        return entity.Id;
    }


    public async Task<bool> DeleteCategory(DeleteCategoryModel model)
    {
        if (model.CategoryId == 0)
        {
            throw new ArgumentException("Category invalid");
        }

        var entity = this.dbContext.Categories.FirstOrDefault(x => x.Id == model.CategoryId && x.UserId == this.currentUserService.UserId);
        if (entity == null)
        {
            throw new ArgumentException("Category not found");
        }

        entity.IsDeleted = true;

        await this.dbContext.SaveChangesAsync();

        return true;
    }

    public async Task<int> UpdateCategory(UpdateCategoryModel model)
    {
        if (model.CategoryId == 0)
        {
            throw new ArgumentException("Category invalid");
        }
        var entity = this.dbContext.Categories.FirstOrDefault(x => x.Id == model.CategoryId && x.UserId == this.currentUserService.UserId);
        if (entity == null)
        {
            throw new ArgumentException("Category not found");
        }
        entity.CategoryName = model.CategoryName;
        entity.BudgetAmount = model.BudgetAmount;
        entity.IsFavourite = model.IsFavourite;
        await this.dbContext.SaveChangesAsync();

        return entity.Id;
    }

    public async Task<CategoryModel> GetCategoryById(int id)
    {
        var ent = this.dbContext.Categories.FirstOrDefault(x => x.Id == id && !x.IsDeleted && x.UserId == this.currentUserService.UserId);
        if (ent == null)
        {
            throw new ArgumentException("Category not found");
        }

        return new CategoryModel
        {
            CategoryName = ent.CategoryName,
            BudgetAmount = ent.BudgetAmount,
            IsFavourite = ent.IsFavourite
        };


    }

}
