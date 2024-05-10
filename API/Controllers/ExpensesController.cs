using Core.Entities;
using Core.Interfaces;
using Infrastructure.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ExpenseTracker.Controllers
{
    public class ExpensesController : BaseApiController
    {
        private readonly TrackerContext _context;
        private readonly IExpenseRepository _repository;

        public ExpensesController(TrackerContext context, IExpenseRepository repository)
        {
            _context = context;
            _repository = repository;
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
            var expense = await _repository.GetExpenseByIdAsync(id);

            if (expense == null)
            {
                return BadRequest("Expense with given id not found");
            }
            return expense;
        }

        [HttpGet]
        public async Task<ActionResult<List<Expense>>> GetExpenses()
        {
            var expenses = await _repository.GetExpensesAsync();
            return expenses.ToList();
        }

        [HttpPut]
        public async Task<ActionResult<Expense>> UpdateExpense(Expense expense)
        {
            var expenseCheck = await _context.Expenses.FirstOrDefaultAsync(x => x.Id == expense.Id);

            if (expenseCheck == null)
            {
                return BadRequest("Expense with given id does not exists, so it cannot be updated");
            }

            var result = await _repository.UpdateExpenseAsync(expense);

            if (result == null)
            {
                return BadRequest("Failed to update expense");
            }

            return expense;
        }
    }
}