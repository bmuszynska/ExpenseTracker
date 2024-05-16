using Core.Entities;
using Core.Interfaces;
using Core.Specifications;
using ExpenseTracker.Controllers;
using Microsoft.AspNetCore.Mvc;
using System.Xml.Linq;

namespace API.Controllers
{
    public class ExpenseCategoriesController : BaseApiController
    {
        private readonly IGenericRepository<ExpenseCategory> _categoryRepository;
        private readonly IGenericRepository<Expense> _expensesRepo;

        public ExpenseCategoriesController(IGenericRepository<ExpenseCategory> categoryRepository, IGenericRepository<Expense> expensesRepo)
        {
            _categoryRepository = categoryRepository;
            _expensesRepo = expensesRepo;
        }

        [HttpPost]
        public async Task<ActionResult<ExpenseCategory>> CreateCategory(string name)
        {
            var expenseCategory = new ExpenseCategory { Name = name };
            /* var result =await*/
            _categoryRepository.Add(expenseCategory);
            _categoryRepository.Save();

            /* if (result == null)
                      {
                 return BadRequest("Failed to create expense.");
             }*/

            return Ok(expenseCategory);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCategory(int id)
        {
            var category = await _categoryRepository.GetByIdAsync(id);

            if (category == null)
            {
                return BadRequest("Category with given id does not exsist, so it cannot be removed");
            }

            var expenseParams = new ExpenseSpecParams() { CategoryId = id };
            var expenseSpec = new ExpensesWithCategoriesSpecification(expenseParams);
            var entitiesWithCategoryToBeRemoved = await _expensesRepo.ListAsync(expenseSpec);

            if (entitiesWithCategoryToBeRemoved.Any())
            {
                return BadRequest("Category with given id has expenses attached to it, so it cannot be removed");
            }

            _categoryRepository.Delete(category);
            _categoryRepository.Save();

            /*if (!result)
                    {
                return BadRequest("Failed to delete expense.");
            }
        */
            return Ok();
        }

        [HttpGet]
        public async Task<IEnumerable<ExpenseCategory>> GetAllCategories()
        {
            return await _categoryRepository.ListAllAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ExpenseCategory>> GetCategoryById(int id)
        {
            var category = await _categoryRepository.GetByIdAsync(id);
            if (category == null)
            {
                return NotFound();
            }
            return category;
        }

        [HttpPut]
        public async Task<IActionResult> UpdateCategory(ExpenseCategory category)
        {
            var existingCategory = await _categoryRepository.GetByIdAsync(category.Id);

            if (existingCategory == null)
            {
                return NotFound("Failed to find category with given id, cannot update");
            }

            _categoryRepository.Update(existingCategory, category);
            _categoryRepository.Save();

            return Ok();
        }
    }
}