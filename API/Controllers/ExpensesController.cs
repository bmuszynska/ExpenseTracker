using API.Helpers;
using Core.Entities;
using Core.Interfaces;
using Core.Specifications;
using Microsoft.AspNetCore.Mvc;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace ExpenseTracker.Controllers
{
    public class ExpensesController : BaseApiController
    {
        private readonly IGenericRepository<Expense> _expensesRepo;
        private readonly IExpenseRepository _repository;

        public ExpensesController(IExpenseRepository repository, IGenericRepository<Expense> expensesRepo)
        {
            _repository = repository;
            _expensesRepo = expensesRepo;
        }

        [HttpPost]
        public async Task<ActionResult<Expense>> CreateExpense(Expense expense)
        {
            var expenseCheck = await _repository.GetExpenseByIdAsync(expense.Id);

            if (expenseCheck != null)
            {
                return BadRequest($"Expense with this id {expense.Id} already exisit");
            }

            var result = await _repository.AddExpenseAsync(expense);

            if (result == null)
            {
                return BadRequest("Failed to create expense.");
            }
            return expense;
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<bool>> DeleteExpense(int id)
        {
            var expense = await _repository.GetExpenseByIdAsync(id);

            if (expense == null)
            {
                return BadRequest("Expense with given id does not exsist, so it cannot be removed");
            }

            var result = await _repository.DeleteExpenseAsync(expense);

            if (!result)
            {
                return BadRequest("Failed to delete expense.");
            }

            return Ok();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Expense>> GetExpense(int id)
        {
            var expenseSpec = new ExpensesWithCategoriesSpecification(id);
            var expense = await _expensesRepo.GetEntityWithSpec(expenseSpec);

            if (expense == null)
            {
                return BadRequest("Expense with given id not found");
            }
            return expense;
        }

        [HttpGet]
        public async Task<ActionResult<Pagination<Expense>>> GetExpenses([FromQuery] ExpenseSpecParams expenseParams)
        {
            var spec = new ExpensesWithCategoriesSpecification(expenseParams);
            var countSpec = new ExpensesWithFiltersForCountSpecification(expenseParams);
            var totalItems = await _expensesRepo.CountAsync(countSpec);
            var expenses = await _expensesRepo.ListAsync(spec);
            return Ok(new Pagination<Expense>(expenseParams.PageIndex, expenseParams.PageSize, totalItems, expenses));
        }

        [HttpPut]
        public async Task<ActionResult<Expense>> UpdateExpense(Expense expense)
        {
            var expenseToUpdate = await _expensesRepo.GetEntityWithSpec(new ExpensesWithCategoriesSpecification(expense.Id));

            if (expenseToUpdate == null)
            {
                return BadRequest("Expense with given id does not exists, so it cannot be updated");
            }

            /* var result = await*/
            _expensesRepo.Update(expenseToUpdate, expense);
            _expensesRepo.Save();
            /* if (result == null)
             {
                 return BadRequest("Failed to update expense");
             }
            */
            return expense;
        }
    }
}