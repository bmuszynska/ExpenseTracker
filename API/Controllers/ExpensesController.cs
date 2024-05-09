using Core.Entities;
using Infrastructure.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ExpenseTracker.Controllers
{
    public class ExpensesController : BaseApiController
    {
        private readonly TrackerContext _context;

        public ExpensesController(TrackerContext context)
        {
            _context = context;
        }

        [HttpPost]
        public async Task<ActionResult<Expense>> CreateExpense(Expense expense)
        {
            await _context.Expenses.AddAsync(expense);
            await _context.SaveChangesAsync();
            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<Expense>> DeleteExpense(int id)
        {
            var expense = await _context.Expenses.FirstOrDefaultAsync(x => x.Id == id);

            if (expense != null)
            {
                _context.Expenses.Remove(expense);
                var result = await _context.SaveChangesAsync();
                return Ok();
            }

            return BadRequest("Expense with given id does not exsist");
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Expense>> GetExpense(int id)
        {
            var expense = await _context.Expenses.FirstOrDefaultAsync(x => x.Id == id);
            return expense;
        }

        [HttpGet]
        public async Task<ActionResult<List<Expense>>> GetExpenses()
        {
            var expenses = await _context.Expenses.ToListAsync();
            return expenses;
        }

        [HttpPut]
        public async Task<ActionResult<Expense>> UpdateExpense(Expense expense)
        {
            var expenseToUpdate = await _context.Expenses.FirstOrDefaultAsync(x => x.Id == expense.Id);

            if (expenseToUpdate != null)
            {
                _context.Expenses.Entry(expenseToUpdate).CurrentValues.SetValues(expense);
                await _context.SaveChangesAsync();
                return Ok();
            }

            return BadRequest("Expense with given id does not exists");
        }
    }
}