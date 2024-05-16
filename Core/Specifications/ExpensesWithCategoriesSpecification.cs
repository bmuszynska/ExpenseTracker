using Core.Entities;

namespace Core.Specifications
{
    public class ExpensesWithCategoriesSpecification : BaseSpecification<Expense>
    {
        public ExpensesWithCategoriesSpecification(ExpenseSpecParams expenseParams)
    : base(x =>
        (string.IsNullOrEmpty(expenseParams.Search) || x.Description.ToLower().Contains(expenseParams.Search)) &&
        (!expenseParams.CategoryId.HasValue || x.CategoryId == expenseParams.CategoryId) &&
        (!expenseParams.Month.HasValue || x.Date.Month == expenseParams.Month)
    )
        {
            AddInclude(x => x.Category);
            AddOrderBy(x => x.Date);
            ApplyPaging(expenseParams.PageSize * (expenseParams.PageIndex - 1), expenseParams.PageSize);

            if (!string.IsNullOrEmpty(expenseParams.Sort))
            {
                switch (expenseParams.Sort)
                {
                    case "dateDesc":
                        AddOrderByDescending(x => x.Date);
                        break;

                    case "amountAsc":
                        AddOrderBy(x => x.Amount);
                        break;

                    case "amountDesc":
                        AddOrderByDescending(x => x.Amount);
                        break;

                    case "categoryAsc":
                        AddOrderBy(x => x.CategoryId);
                        break;

                    case "categoryDesc":
                        AddOrderByDescending(x => x.CategoryId);
                        break;

                    case "dateAsc":
                    default:
                        AddOrderBy(x => x.Date);
                        break;
                }
            }
        }

        public ExpensesWithCategoriesSpecification(int id) : base(x => x.Id == id)
        {
            AddInclude(x => x.Category);
        }
    }
}