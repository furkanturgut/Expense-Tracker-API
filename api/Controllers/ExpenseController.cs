using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using api.Data;
using api.Dtos.ExpenseDtos;
using api.Extensions;
using api.Helper;
using api.Interface;
using api.Models;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace api.Controllers
{   [ApiController]
    [Route("api/[controller]")]
    public class ExpenseController : Controller
    {
        private readonly IExpenseRepository _expenseRepo;
        private readonly IMapper _mapper;

        public ExpenseController(IExpenseRepository expenseRepo, IMapper mapper)
        {
            this._expenseRepo = expenseRepo;
            this._mapper = mapper;
        }

        [HttpPost]
        [Authorize]

        public async Task<IActionResult> Create([FromBody] CreateExpenseDto expenseDto, [FromQuery]int categoryId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var userName = User.GetUserName();
        
            var createdExpense = await _expenseRepo.CreateAsync(expenseDto, userName, categoryId);
            return Ok(_mapper.Map<GetExpenseDto>(createdExpense));
        }
        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetAll([FromQuery]QueryObject query)
        {
            var expenses = _mapper.Map<List<GetExpenseDto>>(await _expenseRepo.GetAllAsync(query));
            return Ok(expenses);
        }
        [HttpGet]
        [Route("{expenseId:int}")]
         [Authorize]
        public async Task<IActionResult> GetById([FromRoute]int expenseId)
        {
            var expense = _mapper.Map<GetExpenseDto>(await _expenseRepo.GetByIdAsync(expenseId));
            if (expense == null)
            {
                return NotFound();
            }
            return Ok(expense);
        }

        [HttpPut]
        [Route("{expenseId:int}")]
         [Authorize]
        public async Task<IActionResult> Update([FromBody]UpdateExpenseDto expenseDto,[FromRoute] int expenseId,[FromQuery] int categoryId)
        {
            var userName = User.GetUserName();
            if(!ModelState.IsValid)
            {
               return BadRequest(); 
            }
            var mappedExpense = _mapper.Map<Expense>(expenseDto);
            var expense= await _expenseRepo.UpdateAsync( expenseId,mappedExpense, userName,categoryId);
            if (expense== null)
            {
                return NotFound();
            }
            return Ok(_mapper.Map<GetExpenseDto>(expense));

        }
        [HttpDelete]
        [Route("{expenseId:int}")]
         [Authorize]
        public async Task<IActionResult> Delete ([FromRoute]int expenseId)
        {
            var expense = await _expenseRepo.DeleteAsync(expenseId);
            if (expense == null )
            {
                return NotFound();
            }
            return NoContent();
        }

    }
}