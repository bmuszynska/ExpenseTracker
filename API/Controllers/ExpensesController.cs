using Core.Entities;
using Microsoft.AspNetCore.Mvc;

namespace ExpenseTracker.Controllers
{
    public class ExpensesController : BaseApiController
    {
        [HttpGet]
        public ActionResult<Expense> GetExpenses()
        {
            return Ok();
        }
    }
}