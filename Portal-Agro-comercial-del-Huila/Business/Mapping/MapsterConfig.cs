using Entity.Domain.Models.Implements.Auth;
using Entity.Domain.Models.Implements.Security;
using Entity.DTOs.Auth;
using Entity.DTOs.Security.Me;
using Mapster;
using System.Linq;

namespace Business.Mapping
{
    public static class MapsterConfig
    {
        public static TypeAdapterConfig Register()
        {
            var config = TypeAdapterConfig.GlobalSettings;

            // RegisterUserDto → User
            config.NewConfig<RegisterUserDto, User>()
                  .Ignore(dest => dest.Id);

            // RegisterUserDto → Person
            config.NewConfig<RegisterUserDto, Person>()
                  .Ignore(dest => dest.Id);

            // User → UserDto
            config.NewConfig<User, UserDto>()
                  .Map(dest => dest.Person, src => src.Person)
                  .Map(dest => dest.Roles, src => src.RolUsers.Select(r => r.Rol.Name).ToList());

            // Person → PersonDto
            config.NewConfig<Person, PersonDto>();

            // User → UserMeDto
            config.NewConfig<User, UserMeDto>()
                  .Map(dest => dest.Person, src => src.Person)
                  .Map(dest => dest.Roles, src => src.RolUsers.Adapt<List<RolUserMeDto>>())
                  .Map(dest => dest.Forms, src =>
                      src.RolUsers
                         .SelectMany(ru => ru.Rol.RolFormPermissions)
                         .Select(rfp => rfp.Form)
                         .DistinctBy(f => f.Id)
                         .Adapt<List<FormMeDto>>());

            // RolUser → RolUserMeDto
            config.NewConfig<RolUser, RolUserMeDto>()
                  .Map(dest => dest.RolId, src => src.Rol.Id)
                  .Map(dest => dest.RolName, src => src.Rol.Name)
                  .Map(dest => dest.Permissions, src => src.Rol.RolFormPermissions.Adapt<List<RolPermissionMeDto>>());

            // RolFormPermission → RolPermissionMeDto
            config.NewConfig<RolFormPermission, RolPermissionMeDto>()
                  .Map(dest => dest.PermissionId, src => src.Permission.Id)
                  .Map(dest => dest.PermissionName, src => src.Permission.Name)
                  .Map(dest => dest.Form, src => src.Form.Adapt<FormMeDto>());

            // Form → FormMeDto
            config.NewConfig<Form, FormMeDto>();

            // Module → ModuleMeDto
            config.NewConfig<Module, ModuleMeDto>();

            return config;
        }
    }
}
