namespace Core.Entities
{
    public class Expense : BaseTransaction
    {
        public ExpenseCategory Category { get; set; }

        public int CategoryId { get; set; }

        public string Description { get; set; }
    }
}