using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace api.Data
{
    public class ApplicationDataContext : IdentityDbContext<AppUser>
    {
        public ApplicationDataContext (DbContextOptions<ApplicationDataContext> options): base(options)
        {

        }
         
         public DbSet<Expense> expenses {get;set;}
         public DbSet<Category> categories {get;set;}

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            List<IdentityRole> roles = new List<IdentityRole>
            {
                new IdentityRole
                {
                    Name= "Admin",
                    NormalizedName= "ADMIN"
                },
                new IdentityRole
                {
                    Name= "User",
                    NormalizedName= "USER"
                }
            };
            List<Category> categories= new List<Category>
            {
                new Category
                {
                    Id=1,
                    Title= "Groceries"
                },
                new Category
                {
                    Id=2,
                    Title="Electronics"
                },
                new Category
                {
                    Id=3,
                    Title="Utilities"
                },
                new Category
                {
                    Id=4,
                    Title="Clothing"
                },
                new Category
                {
                    Id=5,
                    Title="Health"
                },
                new Category
                {
                    Id=6,
                    Title="Leisure"
                },
                new Category
                {
                    Id=7,
                    Title="Others"
                },
            };
            builder.Entity<IdentityRole>().HasData(roles);
            builder.Entity<Category>().HasData(categories);
        }
        
    }
}