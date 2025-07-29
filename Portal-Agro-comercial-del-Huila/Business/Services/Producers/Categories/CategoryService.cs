using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Business.Interfaces.Implements.Producers.Categories;
using Business.Repository;
using Data.Interfaces.Implements.Producers.Categories;
using Data.Interfaces.IRepository;
using Entity.Domain.Models.Implements.Products;
using Entity.DTOs.Producer.Categories;
using MapsterMapper;

namespace Business.Services.Producers.Categories
{
    public class CategoryService : BusinessGeneric<CategoryRegisterDto, CategorySelectDto, Category>, ICategoryService
    {
        private readonly ICategoryRepository _categoryRepository;
        public CategoryService(IDataGeneric<Category> data, IMapper mapper, ICategoryRepository categoryRepository) : base(data, mapper)
        {
            _categoryRepository = categoryRepository;
        }
    }
}
