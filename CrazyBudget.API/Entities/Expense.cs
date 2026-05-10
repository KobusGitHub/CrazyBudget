namespace CrazyBudget.API.Entities;

public class Expense
{
    public int Id { get; set;  }
    public bool IsDeleted { get; set; }
    public Guid UserId { get; set; }
    public int BudgetMonth { get; set; }
    public int BudgetYear { get; set; }
    public int CategoryId { get; set; }
    public DateTime RecordDateUtc { get; set; }
    public double ExpenseValue { get; set; }
    public string Comment { get; set; }
    public Category Category { get; set; }
    public User User { get; set; }
    public SmsExpense SmsExpense { get; set; }
}
