namespace CrazyBudget.API.Services.Categories;

public class CreateCategoryModel
{
    public bool IsFavourite { get; set; }
    public string CategoryName { get; set; }
    public double BudgetAmount { get; set; }

}
