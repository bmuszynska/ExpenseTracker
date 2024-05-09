using Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Infrastructure.Data
{
    public class ExpenseTrackerContextSeed
    {

        public static async Task SeedAsync(TrackerContext context)
        {
            if (!context.Budgets.Any())
            {
                var budgetsData = File.ReadAllText("../Infrastructure/Data/SeedData/budgets.json");
                var budgets = JsonSerializer.Deserialize<List<Budget>>(budgetsData);
                context.Budgets.AddRange(budgets);
            }

            if (!context.ExpenseCategories.Any())
            {
                var expenseCategoriesData = File.ReadAllText("../Infrastructure/Data/SeedData/expenseCategories.json");
                var expenseCategories = JsonSerializer.Deserialize<List<ExpenseCategory>>(expenseCategoriesData);
                context.ExpenseCategories.AddRange(expenseCategories);
            }

            if (!context.Expenses.Any())
            {
                var expensesData = File.ReadAllText("../Infrastructure/Data/SeedData/expenses.json");
                var expenses = JsonSerializer.Deserialize<List<Expense>>(expensesData);
                context.Expenses.AddRange(expenses);
            }

            if (!context.Incomes.Any())
            {
                var incomesData = File.ReadAllText("../Infrastructure/Data/SeedData/incomes.json");
                var incomes = JsonSerializer.Deserialize<List<Income>>(incomesData);
                context.Incomes.AddRange(incomes);
            }


            if (context.ChangeTracker.HasChanges()) await context.SaveChangesAsync();

        }
    }
}
