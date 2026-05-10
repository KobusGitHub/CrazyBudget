namespace CrazyBudget.API.Services.Categories;

public class UpdateCategoryModel
{
    public int CategoryId { get; set; }
    public bool IsFavourite { get; set; }
    public string CategoryName { get; set; }
    public double BudgetAmount { get; set; }
}
