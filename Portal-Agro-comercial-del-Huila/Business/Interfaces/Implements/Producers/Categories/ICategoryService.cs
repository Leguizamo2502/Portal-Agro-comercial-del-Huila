using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Business.Interfaces.IBusiness;
using Entity.DTOs.Producer.Categories;

namespace Business.Interfaces.Implements.Producers.Categories
{
    public interface ICategoryService : IBusiness<CategoryRegisterDto, CategorySelectDto>
    {
    }
}
