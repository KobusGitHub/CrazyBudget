namespace CrazyBudget.API.Services.Categories;

public interface ICategoryService
{
    public Task<int> CreateCategory(CreateCategoryModel model);
    public Task<bool> DeleteCategory(DeleteCategoryModel model);
    public Task<int> UpdateCategory(UpdateCategoryModel model);
    public Task<CategoryModel> GetCategoryById(int id);
}
