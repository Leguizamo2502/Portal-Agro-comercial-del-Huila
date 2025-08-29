using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Business.Interfaces.Implements.Producers.Categories;
using Business.Repository;
using Data.Interfaces.Implements.Producers.Categories;
using Data.Interfaces.IRepository;
using Entity.Domain.Models.Implements.Producers.Products;
using Entity.DTOs.Producer.Categories;
using MapsterMapper;
using Microsoft.Extensions.Logging;
using Utilities.Exceptions;

namespace Business.Services.Producers.Categories
{
    public class CategoryService : BusinessGeneric<CategoryRegisterDto, CategorySelectDto, Category>, ICategoryService
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly ILogger<CategoryService> _logger;
        public CategoryService(IDataGeneric<Category> data, IMapper mapper, ICategoryRepository categoryRepository, ILogger<CategoryService> logger) : base(data, mapper)
        {
            _categoryRepository = categoryRepository;
            _logger = logger;
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

        public async Task<IEnumerable<CategoryNodeDto>> GetNodesAsync(int? parentId)
        {
            try
            {
                var nodes = (await _categoryRepository.GetNodesAsync(parentId)).ToList();
                if (nodes.Count == 0) return Enumerable.Empty<CategoryNodeDto>();

                var parentIds = nodes.Select(n => n.Id).ToList();
                var childrenCount = await _categoryRepository.GetChildrenCountByParentsAsync(parentIds);

                return nodes
                    .Select(n => new CategoryNodeDto
                    {
                        Id = n.Id,
                        Name = n.Name,
                        HasChildren = childrenCount.ContainsKey(n.Id) && childrenCount[n.Id] > 0
                    })
                    .OrderBy(x => x.Name)
                    .ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener nodos de categorías (parentId={ParentId})", parentId);
                throw new BusinessException("Error al obtener categorías.", ex);
            }
        }
    }
}
