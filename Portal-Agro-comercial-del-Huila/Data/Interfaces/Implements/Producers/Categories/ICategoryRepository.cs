using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Data.Interfaces.IRepository;
using Entity.Domain.Models.Implements.Producers.Products;

namespace Data.Interfaces.Implements.Producers.Categories
{
    public interface ICategoryRepository : IDataGeneric<Category>
    {
        /// <summary>
        /// Si parentId == null => categorías raíz; si tiene valor => hijos de esa categoría.
        /// Solo activas y no eliminadas.
        /// </summary>
        Task<IEnumerable<Category>> GetNodesAsync(int? parentId);

        /// <summary>
        /// Devuelve un diccionario ParentId -> Count de hijos activos/no eliminados
        /// para el conjunto de parentIds indicado (consulta en batch, sin N+1).
        /// </summary>
        Task<Dictionary<int, int>> GetChildrenCountByParentsAsync(IEnumerable<int> parentIds);
    }
}
