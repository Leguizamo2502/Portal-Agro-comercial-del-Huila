using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entity.Domain.Models.Implements.Auth;
using Entity.DTOs.Auth;
using Mapster;

namespace Business.Mapping
{
    public static class MapsterConfig
    {
        public static TypeAdapterConfig Register()
        {
            var config = TypeAdapterConfig.GlobalSettings;

            // Map RegisterUserDto → User
            config.NewConfig<RegisterUserDto, User>()
                .Ignore(dest => dest.Id);

            // Map RegisterUserDto → Person
            config.NewConfig<RegisterUserDto, Person>()
                .Ignore(dest => dest.Id);

            // Map User → UserDto
            config.NewConfig<User, UserDto>()
                .Map(dest => dest.Person, src => src.Person)
                .Map(dest => dest.Roles, src => src.RolUsers.Select(r => r.Rol.Name).ToList());

            // Map Person → PersonDto
            config.NewConfig<Person, PersonDto>();

            return config;
        }
    }
}
