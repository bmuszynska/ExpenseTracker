using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Moq;

namespace ExpenseTrackerTests.Controllers
{
    public class ExpenseCategoriesControllerTests
    {
        private readonly Mock<IGenericRepository<ExpenseCategory>> _mockExpenseCategoryRepository;
        private readonly Mock<IGenericRepository<Expense>> _mockExpenseRepository;

        public ExpenseCategoriesControllerTests()
        {
            _mockExpenseCategoryRepository = new Mock<IGenericRepository<ExpenseCategory>>();
            _mockExpenseRepository = new Mock<IGenericRepository<Expense>>();
            _controller = new ExpenseCategoriesController(_mockExpenseRepository, _mockExpenseCategoryRepository)
        }
    }
}