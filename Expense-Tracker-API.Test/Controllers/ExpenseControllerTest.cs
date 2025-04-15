using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using api.Controllers;
using api.Dtos;
using api.Dtos.ExpenseDtos;
using api.Helper;
using api.Interface;
using api.Models;
using api.Repositories;
using AutoMapper;
using FakeItEasy;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Xunit;

namespace Expense_Tracker_API.Test.Controllers
{
    public class ExpenseControllerTests
    {
        private readonly IExpenseRepository _fakeExpenseRepo;
        private readonly IMapper _fakeMapper;
        private readonly UserManager<AppUser> _fakeUserManager;
        private readonly ExpenseController _controller;

        public ExpenseControllerTests()
        {
            _fakeExpenseRepo = A.Fake<IExpenseRepository>();
            _fakeMapper = A.Fake<IMapper>();

            // Properly mock the UserManager with required IUserStore dependency
            _fakeUserManager = A.Fake<UserManager<AppUser>>(options =>
                options.WithArgumentsForConstructor(new object[]
                {
                    A.Fake<IUserStore<AppUser>>(), null, null, null, null, null, null, null, null
                }));

            _controller = new ExpenseController(_fakeExpenseRepo, _fakeMapper, _fakeUserManager);

            // Set up HttpContext with mock User identity
            var user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
            {
                new Claim(ClaimTypes.Name, "testUser"),
            }, "mock"));

            _controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext { User = user }
            };
        }

        [Fact]
        public async Task Create_ShouldReturnBadRequest_WhenModelStateIsInvalid()
        {
            // Arrange
            _controller.ModelState.AddModelError("Cost", "Cost is required");
            var expenseDto = new CreateExpenseDto();
            var categoryId = 1;

            // Act
            var result = await _controller.Create(expenseDto, categoryId);

            // Assert
            result.Should().BeOfType<BadRequestObjectResult>();
        }

        [Fact]
        public async Task Create_ShouldReturnOk_WhenExpenseIsCreated()
        {
            // Arrange
            var expenseDto = new CreateExpenseDto();
            var categoryId = 1;
            var createdExpense = new Expense();
            var mappedExpense = new GetExpenseDto();

            A.CallTo(() => _fakeExpenseRepo.CreateAsync(expenseDto, "testUser", categoryId)).Returns(createdExpense);
            A.CallTo(() => _fakeMapper.Map<GetExpenseDto>(createdExpense)).Returns(mappedExpense);

            // Act
            var result = await _controller.Create(expenseDto, categoryId);

            // Assert
            result.Should().BeOfType<OkObjectResult>();
                
        }

        [Fact]
        public async Task GetAll_ShouldReturnOk_WithListOfExpenses()
        {
            // Arrange
            var query = new QueryObject();
            var expenses = new List<Expense>();
            var mappedExpenses = new List<GetExpenseDto>();

            A.CallTo(() => _fakeExpenseRepo.GetAllAsync(query, "testUser")).Returns(expenses);
            A.CallTo(() => _fakeMapper.Map<List<GetExpenseDto>>(expenses)).Returns(mappedExpenses);

            // Act
            var result = await _controller.GetAll(query);

            // Assert
            result.Should().BeOfType<OkObjectResult>()
                .Which.Value.Should().BeEquivalentTo(mappedExpenses);
        }

        [Fact]
        public async Task GetById_ShouldReturnOk_WhenExpenseExists()
        {
            // Arrange
            var expenseId = 1;
            var expense = new Expense();
            var mappedExpense = new GetExpenseDto();

            A.CallTo(() => _fakeExpenseRepo.GetByIdAsync(expenseId, "testUser")).Returns(expense);
            A.CallTo(() => _fakeMapper.Map<GetExpenseDto>(expense)).Returns(mappedExpense);

            // Act
            var result = await _controller.GetById(expenseId);

            // Assert
            result.Should().BeOfType<OkObjectResult>();
                
        }
        [Fact]
        public async Task GetById_ShouldReturnNotFound_WhenExpenseDoesNotExist()
        {
            // Arrange
            var expenseId = 99;
            A.CallTo(() => _fakeExpenseRepo.GetByIdAsync(expenseId, A<string>._)).Returns((Expense)null);
            // Act
            var result = await _controller.GetById(expenseId);

            // Assert
            result.Should().BeOfType<NotFoundResult>();
        }





        [Fact]
        public async Task Update_ShouldReturnBadRequest_WhenModelStateIsInvalid()
        {
            // Arrange
            _controller.ModelState.AddModelError("Cost", "Cost is required");
            var expenseDto = new UpdateExpenseDto();
            var expenseId = 1;
            var categoryId = 1;

            // Act
            var result = await _controller.Update(expenseDto, expenseId, categoryId);

            // Assert
            result.Should().BeOfType<BadRequestResult>();
        }

        [Fact]
        public async Task Update_ShouldReturnOk_WhenExpenseIsUpdated()
        {
            // Arrange
            var expenseId = 1;
            var categoryId = 1;
            var expenseDto = new UpdateExpenseDto();
            var mappedExpense = new Expense();
            var updatedExpense = new Expense();
            var mappedUpdatedExpense = new GetExpenseDto();

            A.CallTo(() => _fakeMapper.Map<Expense>(expenseDto)).Returns(mappedExpense);
            A.CallTo(() => _fakeExpenseRepo.UpdateAsync(expenseId, mappedExpense, "testUser", categoryId)).Returns(updatedExpense);
            A.CallTo(() => _fakeMapper.Map<GetExpenseDto>(updatedExpense)).Returns(mappedUpdatedExpense);

            // Act
            var result = await _controller.Update(expenseDto, expenseId, categoryId);

            // Assert
            result.Should().BeOfType<OkObjectResult>();
               
        }

        

        [Fact]
        public async Task Delete_ShouldReturnNoContent_WhenExpenseIsDeleted()
        {
            // Arrange
            var expenseId = 1;
            var deletedExpense = new Expense();

            A.CallTo(() => _fakeExpenseRepo.DeleteAsync(expenseId)).Returns(deletedExpense);

            // Act
            var result = await _controller.Delete(expenseId);

            // Assert
            result.Should().BeOfType<NoContentResult>();
        }

        [Fact]
        public async Task Delete_ShouldReturnNotFound_WhenExpenseDoesNotExist()
        {
            // Arrange
            var expenseId = 1;

            A.CallTo(() => _fakeExpenseRepo.DeleteAsync(expenseId)).Returns(null as Expense);

            // Act
            var result = await _controller.Delete(expenseId);

            // Assert
            result.Should().BeOfType<NotFoundResult>();
        }
    }
}
