using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Dtos.ExpenseDtos;
using api.Helper;
using api.Models;

namespace api.Interface
{
    public interface IExpenseRepository
    {
        Task<Expense> CreateAsync(CreateExpenseDto expenseDto, string UserName, int CategoryId);
        Task<List<Expense>> GetAllAsync(QueryObject query, String userName);
        Task<Expense?> GetByIdAsync(int Id, string userName);
        Task<Expense?> UpdateAsync(int Id, Expense expense, string userName, int categoryId);
        Task<Expense?> DeleteAsync(int Id);
    }
}