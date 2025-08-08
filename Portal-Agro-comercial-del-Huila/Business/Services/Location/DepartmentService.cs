using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Business.Interfaces.Implements.Location;
using Business.Repository;
using Data.Interfaces.IRepository;
using Entity.Domain.Models.Implements.Location;
using Entity.DTOs.Location.Create;
using Entity.DTOs.Location.Select;
using MapsterMapper;

namespace Business.Services.Location
{
    public class DepartmentService : BusinessGeneric<DepartmentRegisterDto, DepartmentSelectDto, Department>, IDepartmentService
    {
        public DepartmentService(IDataGeneric<Department> data, IMapper mapper) : base(data, mapper)
        {
        }
    }
}