namespace CrazyBudget.API.Entities;

public class SmsExpense
{
    public SmsExpense()
    {
        IsDeleted = false;
    }

    public int Id { get; set; }
    public bool IsDeleted { get; set; }
    public Guid UserId { get; set; }
    public int? CategoryId { get; set; }
	public int BudgetMonth { get; set; }
    public int BudgetYear { get; set; }
    public string SmsId { get; set; }
    public string SmsBody { get; set; }
    public double SmsAmount { get; set; }
    public string Comment { get; set; }
    public int? ExpenseId { get; set; }

    public User User { get; set; }
    public Category Category { get; set; }
    public Expense Expense { get; set; }

}
