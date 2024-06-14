using Xunit;
using Moq;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using API.Controllers;
using Core.Entities;
using Core.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.Specifications;

namespace API.Tests.Controllers
{
    public class ExpenseCategoriesControllerTests
    {
        private readonly Mock<IGenericRepository<ExpenseCategory>> _mockCategoryRepo;
        private readonly Mock<IGenericRepository<Expense>> _mockExpensesRepo;
        private readonly ExpenseCategoriesController _controller;

        public ExpenseCategoriesControllerTests()
        {
            _mockCategoryRepo = new Mock<IGenericRepository<ExpenseCategory>>();
            _mockExpensesRepo = new Mock<IGenericRepository<Expense>>();
            _controller = new ExpenseCategoriesController(_mockCategoryRepo.Object, _mockExpensesRepo.Object);
        }

        [Fact]
        public async Task CreateCategory_ShouldReturnOkResult_WithCreatedCategory()
        {
            // Arrange
            var categoryName = "Test Category";
            var expenseCategory = new ExpenseCategory { Name = categoryName };

            _mockCategoryRepo.Setup(repo => repo.AddAsync(It.IsAny<ExpenseCategory>())).Callback<ExpenseCategory>(category => { });
            _mockCategoryRepo.Setup(repo => repo.Save()).Verifiable();

            // Act
            var result = await _controller.CreateCategory(categoryName);

            // Assert
            var okResult = result.Result.Should().BeOfType<OkObjectResult>().Subject;
            var returnedCategory = okResult.Value.Should().BeAssignableTo<ExpenseCategory>().Subject;
            returnedCategory.Name.Should().Be(categoryName);

            _mockCategoryRepo.Verify(repo => repo.AddAsync(It.IsAny<ExpenseCategory>()), Times.Once);
            _mockCategoryRepo.Verify(repo => repo.Save(), Times.Once);
        }

        [Fact]
        public async Task DeleteCategory_ShouldReturnBadRequest_WhenCategoryDoesNotExist()
        {
            // Arrange
            int categoryId = 1;
            _mockCategoryRepo.Setup(repo => repo.GetByIdAsync(categoryId)).ReturnsAsync((ExpenseCategory)null);

            // Act
            var result = await _controller.DeleteCategory(categoryId);

            // Assert
            var badRequestResult = result.Should().BeOfType<BadRequestObjectResult>().Subject;
            badRequestResult.Value.Should().Be("Category with given id does not exsist, so it cannot be removed");
        }

        [Fact]
        public async Task DeleteCategory_ShouldReturnBadRequest_WhenCategoryHasExpenses()
        {
            // Arrange
            int categoryId = 1;
            var category = new ExpenseCategory { Id = categoryId };
            _mockCategoryRepo.Setup(repo => repo.GetByIdAsync(categoryId)).ReturnsAsync(category);

            var expenses = new List<Expense> { new Expense { CategoryId = categoryId } };
            _mockExpensesRepo.Setup(repo => repo.ListAsync(It.IsAny<ExpensesWithCategoriesSpecification>())).ReturnsAsync(expenses);

            // Act
            var result = await _controller.DeleteCategory(categoryId);

            // Assert
            var badRequestResult = result.Should().BeOfType<BadRequestObjectResult>().Subject;
            badRequestResult.Value.Should().Be("Category with given id has expenses attached to it, so it cannot be removed");
        }

        [Fact]
        public async Task DeleteCategory_ShouldReturnOkResult_WhenCategoryIsDeleted()
        {
            // Arrange
            int categoryId = 1;
            var category = new ExpenseCategory { Id = categoryId };
            _mockCategoryRepo.Setup(repo => repo.GetByIdAsync(categoryId)).ReturnsAsync(category);
            _mockExpensesRepo.Setup(repo => repo.ListAsync(It.IsAny<ExpensesWithCategoriesSpecification>())).ReturnsAsync(new List<Expense>());

            _mockCategoryRepo.Setup(repo => repo.Delete(category));
            _mockCategoryRepo.Setup(repo => repo.Save()).Verifiable();

            // Act
            var result = await _controller.DeleteCategory(categoryId);

            // Assert
            result.Should().BeOfType<OkResult>();
            _mockCategoryRepo.Verify(repo => repo.Delete(category), Times.Once);
            _mockCategoryRepo.Verify(repo => repo.Save(), Times.Once);
        }

        [Fact]
        public async Task GetAllCategories_ShouldReturnAllCategories()
        {
            // Arrange
            var categories = new List<ExpenseCategory>
            {
                new ExpenseCategory { Id = 1, Name = "Category1" },
                new ExpenseCategory { Id = 2, Name = "Category2" }
            };
            _mockCategoryRepo.Setup(repo => repo.ListAllAsync()).ReturnsAsync(categories);

            // Act
            var result = await _controller.GetAllCategories();

            // Assert
            result.Should().BeEquivalentTo(categories);
        }

        [Fact]
        public async Task GetCategoryById_ShouldReturnCategory_WhenCategoryExists()
        {
            // Arrange
            int categoryId = 1;
            var category = new ExpenseCategory { Id = categoryId };
            _mockCategoryRepo.Setup(repo => repo.GetByIdAsync(categoryId)).ReturnsAsync(category);

            // Act
            var result = await _controller.GetCategoryById(categoryId);

            // Assert
            var okResult = result.Result.Should().BeOfType<OkObjectResult>().Subject;
            var returnedCategory = okResult.Value.Should().BeAssignableTo<ExpenseCategory>().Subject;
            returnedCategory.Should().Be(category);
        }

        [Fact]
        public async Task GetCategoryById_ShouldReturnNotFound_WhenCategoryDoesNotExist()
        {
            // Arrange
            int categoryId = 1;
            _mockCategoryRepo.Setup(repo => repo.GetByIdAsync(categoryId)).ReturnsAsync((ExpenseCategory)null);

            // Act
            var result = await _controller.GetCategoryById(categoryId);

            // Assert
            result.Result.Should().BeOfType<NotFoundResult>();
        }

        [Fact]
        public async Task UpdateCategory_ShouldReturnNotFound_WhenCategoryDoesNotExist()
        {
            // Arrange
            var category = new ExpenseCategory { Id = 1 };
            _mockCategoryRepo.Setup(repo => repo.GetByIdAsync(category.Id)).ReturnsAsync((ExpenseCategory)null);

            // Act
            var result = await _controller.UpdateCategory(category);

            // Assert
            var notFoundResult = result.Should().BeOfType<NotFoundObjectResult>().Subject;
            notFoundResult.Value.Should().Be("Failed to find category with given id, cannot update");
        }

        [Fact]
        public async Task UpdateCategory_ShouldReturnOkResult_WhenCategoryIsUpdated()
        {
            // Arrange
            var category = new ExpenseCategory { Id = 1, Name = "Updated Name" };
            var existingCategory = new ExpenseCategory { Id = 1, Name = "Old Name" };

            _mockCategoryRepo.Setup(repo => repo.GetByIdAsync(category.Id)).ReturnsAsync(existingCategory);
            _mockCategoryRepo.Setup(repo => repo.Update(existingCategory, category));
            _mockCategoryRepo.Setup(repo => repo.Save()).Verifiable();

            // Act
            var result = await _controller.UpdateCategory(category);

            // Assert
            result.Should().BeOfType<OkResult>();
            _mockCategoryRepo.Verify(repo => repo.Update(existingCategory, category), Times.Once);
            _mockCategoryRepo.Verify(repo => repo.Save(), Times.Once);
        }
    }
}