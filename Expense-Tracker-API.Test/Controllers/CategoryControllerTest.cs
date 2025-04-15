using api.Controllers;
using api.Dtos.CategoryDtos;
using api.Interface;
using api.Models;
using AutoMapper;
using FakeItEasy;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace Expense_Tracker_API.Test.Controllers
{
    public class CategoryControllerTest
    {
        private readonly IMapper _mapper;
        private readonly ICategoryRepository _categoryRepository;
        private readonly CategoryController _controller;

        public CategoryControllerTest()
        {
            // Set up fakes
            _mapper = A.Fake<IMapper>();
            _categoryRepository = A.Fake<ICategoryRepository>();

            // Create the controller with fakes
            _controller = new CategoryController(_mapper, _categoryRepository);
        }

        [Fact]
        public async Task GetAll_ReturnsOkResult_WithListOfCategories()
        {
            // Arrange
            var categoryEntities = new List<Category>(); // Use the actual entity type
            var categoryDtos = new List<GetCategoryDto>();

            A.CallTo(() => _categoryRepository.GetAll()).Returns(categoryEntities);
            A.CallTo(() => _mapper.Map<List<GetCategoryDto>>(A<List<Category>>.Ignored))
                .Returns(categoryDtos);

            // Act
            var result = await _controller.GetAll();

            // Assert
            result.Should().BeOfType<OkObjectResult>();
            var okResult = result as OkObjectResult;
            okResult?.Value.Should().BeEquivalentTo(categoryDtos);
            A.CallTo(() => _categoryRepository.GetAll()).MustHaveHappenedOnceExactly();
            A.CallTo(() => _mapper.Map<List<GetCategoryDto>>(A<List<Category>>.Ignored)).MustHaveHappenedOnceExactly();
        }
    }
}
