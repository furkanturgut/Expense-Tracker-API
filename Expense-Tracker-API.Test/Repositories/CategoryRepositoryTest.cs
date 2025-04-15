using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Data;
using api.Models;
using api.Repositories;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace Expense_Tracker_API.Test.Repositories
{
    public class CategoryRepositoryTest
    {
        private ApplicationDataContext CreateInMemoryDbContext()
        {
            var options = new DbContextOptionsBuilder<ApplicationDataContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            var context = new ApplicationDataContext(options);
            context.Database.EnsureCreated();
            return context;
        }

        private void ClearCategories(ApplicationDataContext context)
        {
            context.categories.RemoveRange(context.categories);
            context.SaveChanges();
        }

        [Fact]
        public async Task GetAll_ShouldReturnAllCategories()
        {
            // Arrange
            var dbContext = CreateInMemoryDbContext();
            ClearCategories(dbContext);
            var testCategories = new List<Category>
            {
                new Category { Id = 97, Title = "Food" },
                new Category { Id = 98, Title = "Transport" },
                new Category { Id = 99, Title = "Entertainment" }
            };

            await dbContext.categories.AddRangeAsync(testCategories);
            await dbContext.SaveChangesAsync();

            var repository = new CategoryRepository(dbContext);

            // Act
            var result = await repository.GetAll();

            // Assert
            result.Should().NotBeNull();
            result.Should().HaveCount(3);
            result.Should().BeEquivalentTo(testCategories, options =>
                options.Excluding(c => c.Expenses)); // Exclude Expenses collection from comparison

            // Verify specific items
            result.Should().Contain(c => c.Title == "Food");
            result.Should().Contain(c => c.Title == "Transport");
            result.Should().Contain(c => c.Title == "Entertainment");
        }

        [Fact]
        public async Task GetAll_ShouldReturnEmptyList_WhenNoCategories()
        {
            // Arrange
            var dbContext = CreateInMemoryDbContext();
            ClearCategories(dbContext);
            var repository = new CategoryRepository(dbContext);

            // Act
            var result = await repository.GetAll();

            // Assert
            result.Should().NotBeNull();
            result.Should().BeEmpty();
        }
    }
}
