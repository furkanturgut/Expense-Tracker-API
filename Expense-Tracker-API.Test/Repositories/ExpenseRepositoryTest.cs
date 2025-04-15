using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Data;
using api.Dtos.ExpenseDtos;
using api.Dtos;
using api.Helper;
using api.Models;
using api.Repositories;
using FakeItEasy;
using FluentAssertions;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace Expense_Tracker_API.Test.Repositories
{
    public class ExpenseRepositoryTest
    {
        private readonly ApplicationDataContext _contextMock;
        private readonly UserManager<AppUser> _userManagerMock;
        private readonly ExpenseRepository _sut;

        public ExpenseRepositoryTest()
        {
            // Setup in-memory database
            var options = new DbContextOptionsBuilder<ApplicationDataContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            _contextMock = new ApplicationDataContext(options);
            _userManagerMock = A.Fake<UserManager<AppUser>>(x =>
                x.WithArgumentsForConstructor(new object[]
                {
                    A.Fake<IUserStore<AppUser>>(), null, null, null, null, null, null, null, null
                }));

            _sut = new ExpenseRepository(_contextMock, _userManagerMock);
        }

        [Fact]
        public async Task CreateAsync_ShouldCreateAndReturnExpense()
        {
            // Arrange
            var userName = "testUser";
            var categoryId = 1;
            var user = new AppUser { Id = "user123", UserName = userName, FullName="fullname123" };
            var category = new Category { Id = categoryId, Title = "Food" };
            var expenseDto = new CreateExpenseDto
            {
                Description = "Lunch",
                Cost = 25.50m
            };

            A.CallTo(() => _userManagerMock.FindByNameAsync(userName)).Returns(Task.FromResult(user));
            await _contextMock.categories.AddAsync(category);
            await _contextMock.SaveChangesAsync();

            // Act
            var result = await _sut.CreateAsync(expenseDto, userName, categoryId);

            // Assert
            result.Should().NotBeNull();
            result.Description.Should().Be("Lunch");
            result.Cost.Should().Be(25.50m);
            result.Category.Should().Be(category);
            result.User.Should().Be(user);

            _contextMock.expenses.Should().ContainSingle();
        }

        [Fact]
        public async Task DeleteAsync_WithExistingId_ShouldDeleteAndReturnExpense()
        {
            // Arrange
            var expense = new Expense
            {
                Id = 1,
                Description = "Dinner",
                Cost = 35.20m
            };
            await _contextMock.expenses.AddAsync(expense);
            await _contextMock.SaveChangesAsync();

            // Act
            var result = await _sut.DeleteAsync(1);

            // Assert
            result.Should().NotBeNull();
            result.Id.Should().Be(1);
            _contextMock.expenses.Should().BeEmpty();
        }

        [Fact]
        public async Task DeleteAsync_WithNonExistingId_ShouldReturnNull()
        {
            // Act
            var result = await _sut.DeleteAsync(999);

            // Assert
            result.Should().BeNull();
        }

        [Fact]
        public async Task GetAllAsync_ForAdmin_ShouldReturnAllExpenses()
        {
            // Arrange
            var user = new AppUser { Id = "admin123", UserName = "adminUser" };

            var expenses = new List<Expense>
            {
                new Expense { Id = 1, UserId = "user1", CreatedDayTime = DateTime.Now.AddDays(-2) },
                new Expense { Id = 2, UserId = "user2", CreatedDayTime = DateTime.Now.AddDays(-10) }
            };

            await _contextMock.expenses.AddRangeAsync(expenses);
            await _contextMock.SaveChangesAsync();

            A.CallTo(() => _userManagerMock.FindByNameAsync("adminUser")).Returns(Task.FromResult(user));
            A.CallTo(() => _userManagerMock.GetRolesAsync(user)).Returns(Task.FromResult<IList<string>>(new List<string> { "Admin" }));

            var query = new QueryObject();

            // Act
            var result = await _sut.GetAllAsync(query, "adminUser");

            // Assert
            result.Should().HaveCount(2);
        }

        [Fact]
        public async Task GetAllAsync_ForUser_ShouldReturnOnlyUserExpenses()
        {
            // Arrange
            var userId = "user123";
            var user = new AppUser { Id = userId, UserName = "testUser" };

            var expenses = new List<Expense>
            {
                new Expense { Id = 1, UserId = userId, CreatedDayTime = DateTime.Now.AddDays(-2) },
                new Expense { Id = 2, UserId = "differentUser", CreatedDayTime = DateTime.Now.AddDays(-3) }
            };

            await _contextMock.expenses.AddRangeAsync(expenses);
            await _contextMock.SaveChangesAsync();

            A.CallTo(() => _userManagerMock.FindByNameAsync("testUser")).Returns(Task.FromResult(user));
            A.CallTo(() => _userManagerMock.GetRolesAsync(user)).Returns(Task.FromResult<IList<string>>(new List<string> { "User" }));

            var query = new QueryObject();

            // Act
            var result = await _sut.GetAllAsync(query, "testUser");

            // Assert
            result.Should().HaveCount(1);
            result.First().UserId.Should().Be(userId);
        }

        [Fact]
        public async Task GetAllAsync_WithPastWeekFilter_ShouldReturnOnlyRecentExpenses()
        {
            // Arrange
            var user = new AppUser { Id = "user123", UserName = "testUser" };

            var expenses = new List<Expense>
            {
                new Expense { Id = 1, UserId = "user123", CreatedDayTime = DateTime.Now.AddDays(-2) },
                new Expense { Id = 2, UserId = "user123", CreatedDayTime = DateTime.Now.AddDays(-10) }
            };

            await _contextMock.expenses.AddRangeAsync(expenses);
            await _contextMock.SaveChangesAsync();

            A.CallTo(() => _userManagerMock.FindByNameAsync("testUser")).Returns(Task.FromResult(user));
            A.CallTo(() => _userManagerMock.GetRolesAsync(user)).Returns(Task.FromResult<IList<string>>(new List<string> { "User" }));

            var query = new QueryObject { PastWeek = true };

            // Act
            var result = await _sut.GetAllAsync(query, "testUser");

            // Assert
            result.Should().HaveCount(1);
            result.First().Id.Should().Be(1);
        }

        [Fact]
        public async Task GetByIdAsync_WithExistingIdAndAuthorizedUser_ShouldReturnExpense()
        {
            // Arrange
            var userId = "user123";
            var user = new AppUser { Id = userId, UserName = "testUser" };
            var expense = new Expense
            {
                Id = 1,
                UserId = userId,
                Description = "Groceries",
                Cost = 45.75m,
                Category = new Category { Id = 1, Title = "Food" }
            };

            await _contextMock.expenses.AddAsync(expense);
            await _contextMock.SaveChangesAsync();

            A.CallTo(() => _userManagerMock.FindByNameAsync("testUser")).Returns(Task.FromResult(user));
            A.CallTo(() => _userManagerMock.GetRolesAsync(user)).Returns(Task.FromResult<IList<string>>(new List<string> { "User" }));

            // Act
            var result = await _sut.GetByIdAsync(1, "testUser");

            // Assert
            result.Should().NotBeNull();
            result.Id.Should().Be(1);
            result.UserId.Should().Be(userId);
        }

        [Fact]
        public async Task GetByIdAsync_WithNonExistingId_ShouldReturnNull()
        {
            // Arrange
            var user = new AppUser { Id = "user123", UserName = "testUser" };

            A.CallTo(() => _userManagerMock.FindByNameAsync("testUser")).Returns(Task.FromResult(user));
            A.CallTo(() => _userManagerMock.GetRolesAsync(user)).Returns(Task.FromResult<IList<string>>(new List<string> { "User" }));

            // Act
            var result = await _sut.GetByIdAsync(999, "testUser");

            // Assert
            result.Should().BeNull();
        }

        [Fact]
        public async Task GetByIdAsync_WithUnauthorizedUser_ShouldThrowException()
        {
            // Arrange
            var userId = "user123";
            var differentUserId = "user456";
            var user = new AppUser { Id = differentUserId, UserName = "testUser" };
            var expense = new Expense
            {
                Id = 1,
                UserId = userId
            };

            await _contextMock.expenses.AddAsync(expense);
            await _contextMock.SaveChangesAsync();

            A.CallTo(() => _userManagerMock.FindByNameAsync("testUser")).Returns(Task.FromResult(user));
            A.CallTo(() => _userManagerMock.GetRolesAsync(user)).Returns(Task.FromResult<IList<string>>(new List<string> { "User" }));

            // Act & Assert
            await _sut.Invoking(sut => sut.GetByIdAsync(1, "testUser"))
                .Should().ThrowAsync<UnauthorizedAccessException>()
                .WithMessage("You do not have permission to access this resource.");
        }

        [Fact]
        public async Task UpdateAsync_WithValidData_ShouldUpdateAndReturnExpense()
        {
            // Arrange
            var userName = "testUser";
            var categoryId = 2;

            var user = new AppUser { Id = "user123", UserName = userName , FullName="fullname123"};
            var category = new Category { Id = categoryId, Title = "Entertainment" };
            var expense = new Expense
            {
                Id = 1,
                Description = "Old description",
                Cost = 10.00m
            };

            var updatedExpense = new Expense
            {
                Description = "Updated description",
                Cost = 20.00m
            };

            await _contextMock.expenses.AddAsync(expense);
            await _contextMock.categories.AddAsync(category);
            await _contextMock.SaveChangesAsync();

            A.CallTo(() => _userManagerMock.FindByNameAsync(userName)).Returns(Task.FromResult(user));

            // Act
            var result = await _sut.UpdateAsync(1, updatedExpense, userName, categoryId);

            // Assert
            result.Should().NotBeNull();
            result.Description.Should().Be("Updated description");
            result.Cost.Should().Be(20.00m);
            result.Category.Should().Be(category);
            result.User.Should().Be(user);
        }

        [Fact]
        public async Task UpdateAsync_WithNonExistingExpense_ShouldReturnNull()
        {
            // Arrange
            var userName = "testUser";
            var user = new AppUser { Id = "user123", UserName = userName };
            var updatedExpense = new Expense
            {
                Description = "Updated description",
                Cost = 20.00m
            };

            A.CallTo(() => _userManagerMock.FindByNameAsync(userName)).Returns(Task.FromResult(user));

            // Act
            var result = await _sut.UpdateAsync(999, updatedExpense, userName, 1);

            // Assert
            result.Should().BeNull();
        }

        [Fact]
        public async Task UpdateAsync_WithNonExistingCategory_ShouldReturnNull()
        {
            // Arrange
            var userName = "testUser";
            var user = new AppUser { Id = "user123", UserName = userName };
            var expense = new Expense
            {
                Id = 1,
                Description = "Old description",
                Cost = 10.00m
            };

            var updatedExpense = new Expense
            {
                Description = "Updated description",
                Cost = 20.00m
            };

            await _contextMock.expenses.AddAsync(expense);
            await _contextMock.SaveChangesAsync();

            A.CallTo(() => _userManagerMock.FindByNameAsync(userName)).Returns(Task.FromResult(user));

            // Act
            var result = await _sut.UpdateAsync(1, updatedExpense, userName, 999);

            // Assert
            result.Should().BeNull();
        }
    }
}
