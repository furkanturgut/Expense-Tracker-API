using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using api.Data;
using api.Dtos.ExpenseDtos;
using api.Helper;
using api.Interface;
using api.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace api.Repositories
{
    public class ExpenseRepository : IExpenseRepository
    {
        private readonly ApplicationDataContext _context;
        private readonly UserManager<AppUser> _userManager;

        public ExpenseRepository(ApplicationDataContext context, UserManager<AppUser> userManager)
        {
            this._context = context;
            this._userManager = userManager;
        }
        public async Task<Expense> CreateAsync(CreateExpenseDto expenseDto, string UserName, int CategoryId)
        {
            var user =  await  _userManager.FindByNameAsync(UserName);
            var category= await _context.categories.FirstOrDefaultAsync(c=> c.Id==CategoryId);
            var expense= new Expense{
                Category=category,
                User=user,
                Description=expenseDto.Description,
                Cost=expenseDto.Cost
            };
            await _context.AddAsync(expense);
            await _context.SaveChangesAsync();
            return expense;
        }

        public async Task<Expense?> DeleteAsync(int Id)
        {
            var expense = await _context.expenses.FirstOrDefaultAsync(e=> e.Id== Id);
            if (expense== null)
            {
                return null;
            }
            _context.Remove(expense);
            await _context.SaveChangesAsync();
            return expense;
        }

        public async Task<List<Expense>> GetAllAsync(QueryObject query, String userName)
        {
            var user = await  _userManager.FindByNameAsync(userName);
            var userRole = await _userManager.GetRolesAsync(user);
            var expenses=  _context.expenses.Include(e=> e.Category).AsQueryable();
            if (query.PastWeek)
            {
                expenses=expenses.Where(e=> e.CreatedDayTime >= DateTime.Now.AddDays(-7));
            }
            if (query.LastMonth)
            {
                    expenses = expenses.Where(e => e.CreatedDayTime >= DateTime.Now.AddMonths(-1));
            }
            if (query.Last3Months)
            {
                expenses = expenses.Where(e => e.CreatedDayTime >= DateTime.Now.AddMonths(-3));
            }
            if(query.StartDate.HasValue)
            {
                var startDate=query.StartDate.Value; 
                expenses =  expenses.Where(e=> e.CreatedDayTime >= startDate);
            }
            if (userRole[0] == "User") {expenses = expenses.Where(u => u.UserId== user.Id.ToString());}
           
            return await expenses.ToListAsync();           
        }

        public async Task<Expense?> GetByIdAsync(int Id, string userName)
        {
            var user = await  _userManager.FindByNameAsync(userName);
            var userRole = await _userManager.GetRolesAsync(user);
            var expense = await _context.expenses.Include(e=>e.Category).FirstOrDefaultAsync(e=> e.Id== Id);
            if (expense==null)
            {
                return null;
            }
            if (userRole[0]== "Admin"|| expense.UserId== user.Id)
            {
                return expense;
            }
            else
            {
                throw new UnauthorizedAccessException("You do not have permission to access this resource.");
            }
            
        }

        public async Task<Expense?> UpdateAsync(int Id, Expense expense, string userName, int categoryId)
        {
            var expenseModel = await  _context.expenses.FirstOrDefaultAsync(e=> e.Id==Id);
           if (expense==null)
           {
                return null;
           }
           var user = await _userManager.FindByNameAsync(userName);
           var category = await _context.categories.FindAsync(categoryId);
           if (category==null)
           {
            return null;
           }
           expenseModel.Category=category;
           expenseModel.Cost=expense.Cost;
           expenseModel.Description=expense.Description;
           expenseModel.User= user;
           await _context.SaveChangesAsync();
           return expenseModel;
        }
    }
}