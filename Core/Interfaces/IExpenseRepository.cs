using Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Interfaces
{
    public interface IExpenseRepository : IBaseTransactionRepository
    {
        Task<Expense> AddExpenseAsync(Expense expense);

        Task<bool> DeleteExpenseAsync(Expense expense);

        Task<Expense> GetExpenseByIdAsync(int id);

        Task<IEnumerable<Expense>> GetExpensesAsync();

        Task<Expense> UpdateExpenseAsync(Expense expense);
    }
}