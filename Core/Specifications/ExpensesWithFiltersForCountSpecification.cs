using Core.Entities;

namespace Core.Specifications
{
    public class ExpensesWithFiltersForCountSpecification : BaseSpecification<Expense>
    {
        public ExpensesWithFiltersForCountSpecification(ExpenseSpecParams expenseParams)
    : base(x =>
        (string.IsNullOrEmpty(expenseParams.Search) || x.Description.ToLower().Contains(expenseParams.Search)) &&
        (!expenseParams.CategoryId.HasValue || x.CategoryId == expenseParams.CategoryId) &&
        (!expenseParams.Month.HasValue || x.Date.Month == expenseParams.Month)
    )
        { }
    }
}