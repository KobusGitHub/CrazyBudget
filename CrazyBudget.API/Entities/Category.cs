namespace CrazyBudget.API.Entities;

public class Category
{
    public Category()
    {
        IsDeleted = false;
        IsFavourite = false;
    }
    public int Id { get; set; }
    public bool IsDeleted { get; set; }
    public Guid UserId { get; set; }
    public bool IsFavourite { get; set; }
    public string CategoryName { get; set; }
    public double BudgetAmount { get; set; }

    public User User { get; set; }
    public List<Expense> Expenses { get; set; }
}
