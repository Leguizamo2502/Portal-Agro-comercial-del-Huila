using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Business.Interfaces.Implements.Location;
using Business.Repository;
using Data.Interfaces.Implements.Location;
using Data.Interfaces.IRepository;
using Entity.Domain.Models.Implements.Location;
using Entity.DTOs.Location.Create;
using Entity.DTOs.Location.Select;
using MapsterMapper;
using Utilities.Exceptions;

namespace Business.Services.Location
{
    public class CityService : BusinessGeneric<CityRegisterDto, CitySelectDto, City>, ICityService
    {
        private readonly ICityRepository _cityRepository;
        public CityService(IDataGeneric<City> data, IMapper mapper, ICityRepository cityRepository) : base(data, mapper)
        {
            _cityRepository = cityRepository;
        }

        public override async Task<IEnumerable<CitySelectDto>> GetAllAsync()
        {
            try
            {
                var entities = await _cityRepository.GetAllAsync();
                return _mapper.Map<IEnumerable<CitySelectDto>>(entities);
            }
            catch (Exception ex)
            {
                throw new BusinessException("Error al obtener todos los registros.", ex);
            }
        }



        public async Task<IEnumerable<CitySelectDto>> GetCityByDepartment(int idDepartment)
        {
            try
            {
                var entities = await _cityRepository.GetCityByDepartment(idDepartment);
                return _mapper.Map<IEnumerable<CitySelectDto>>(entities);
            }
            catch (Exception ex)
            {
                throw new Exception("Error al obtener las ciudades por departamento", ex);
            }

        }
    }
}