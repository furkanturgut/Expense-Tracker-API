using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Data;
using api.Interface;
using api.Models;
using Microsoft.EntityFrameworkCore;

namespace api.Repositories
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly ApplicationDataContext _dataContext;

        public CategoryRepository(ApplicationDataContext dataContext)
        {
            this._dataContext = dataContext;
        }
        public async Task<List<Category>> GetAll()
        {
            return await _dataContext.categories.ToListAsync();
        }
    }
}