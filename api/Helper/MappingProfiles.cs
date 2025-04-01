using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Dtos.CategoryDtos;
using api.Dtos.ExpenseDtos;
using api.Models;
using AutoMapper;

namespace api.Helper
{
    public class MappingProfiles: Profile
    {
        public MappingProfiles()
        {
            CreateMap<Expense, CreateExpenseDto>().ReverseMap();
            CreateMap<Expense, UpdateExpenseDto>().ReverseMap();
            CreateMap<Expense, GetExpenseDto>().ForMember(dest=> dest.CategoryName , opt=> opt.MapFrom(opt=> opt.Category.Title));
            CreateMap<Category, GetCategoryDto>().ReverseMap();
        }
        
    }
}