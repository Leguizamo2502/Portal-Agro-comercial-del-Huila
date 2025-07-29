using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Data.Interfaces.Implements.Producers.Categories;
using Data.Repository;
using Entity.Domain.Models.Implements.Products;
using Entity.Infrastructure.Context;

namespace Data.Service.Producers.Categories
{
    public class CategoryRepository : DataGeneric<Category>, ICategoryRepository
    {
        public CategoryRepository(ApplicationDbContext context) : base(context)
        {
        }
    }
}
