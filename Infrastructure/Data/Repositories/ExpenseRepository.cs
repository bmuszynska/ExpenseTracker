using Core.Entities;
using Core.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Data.Repositories
{
    public class ExpenseRepository : IExpenseRepository
    {
        private readonly TrackerContext _context;

        public ExpenseRepository(TrackerContext context)
        {
            _context = context;
        }

        public async Task<Expense> AddExpenseAsync(Expense expense)
        {
            await _context.Expenses.AddAsync(expense);
            var result = await _context.SaveChangesAsync();
            return result > 0 ? expense : null;
        }

        public async Task<bool> DeleteExpenseAsync(Expense expense)
        {
            _context.Expenses.Remove(expense);
            var result = await _context.SaveChangesAsync();
            return result > 0 ? true : false;
        }

        public async Task<Expense> GetExpenseByIdAsync(int id)
        {
            return await _context.Expenses.Include(x => x.Category).FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<IEnumerable<Expense>> GetExpensesAsync()
        {
            return await _context.Expenses.Include(x => x.Category).ToListAsync();
        }

        public async Task<Expense> UpdateExpenseAsync(Expense expense)
        {
            var expenseOldValue = await GetExpenseByIdAsync(expense.Id);
            _context.Expenses.Entry(expenseOldValue).CurrentValues.SetValues(expense);
            var result = await _context.SaveChangesAsync();
            return result > 0 ? expense : null;
        }
    }
}