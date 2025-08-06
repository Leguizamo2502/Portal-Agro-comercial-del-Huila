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
using Utilities.Exceptions;

namespace Business.Services.Producers.Categories
{
    public class CategoryService : BusinessGeneric<CategoryRegisterDto, CategorySelectDto, Category>, ICategoryService
    {
        private readonly ICategoryRepository _categoryRepository;
        public CategoryService(IDataGeneric<Category> data, IMapper mapper, ICategoryRepository categoryRepository) : base(data, mapper)
        {
            _categoryRepository = categoryRepository;
        }

        public override async Task<IEnumerable<CategorySelectDto>> GetAllAsync()
        {
            try
            {
                var entities = await _categoryRepository.GetAllAsync();
                return _mapper.Map<IEnumerable<CategorySelectDto>>(entities);
            }
            catch (Exception ex)
            {
                throw new BusinessException("Error al obtener todos los registros.", ex);
            }
        }
    }
}
