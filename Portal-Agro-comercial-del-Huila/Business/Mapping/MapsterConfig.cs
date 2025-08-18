using Entity.Domain.Models.Implements.Auth;
using Entity.Domain.Models.Implements.Producers;
using Entity.Domain.Models.Implements.Products;
using Entity.Domain.Models.Implements.Security;
using Entity.DTOs.Auth;
using Entity.DTOs.Auth.User;
using Entity.DTOs.Producer.Categories;
using Entity.DTOs.Producer.Farm.Create;
using Entity.DTOs.Producer.Farm.Select;
using Entity.DTOs.Producer.Farm.Update;
using Entity.DTOs.Products.Create;
using Entity.DTOs.Products.Select;
using Entity.DTOs.Products.Update;
using Entity.DTOs.Security.Create.Rols;
using Entity.DTOs.Security.Selects.RolFormPermission;
using Entity.DTOs.Security.Selects.Rols;
using Entity.DTOs.Security.Selects.RolUser;
using Mapster;

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
            config.NewConfig<Person, PersonSelectDto>()
                .Map(desr=>desr.FullName,src=>$"{src.FirstName} {src.LastName}")
                .Map(dest => dest.Email, src => src.User.Email);

            // Map User → UserDto
            config.NewConfig<User, UserDto>()
                .Map(dest => dest.Person, src => src.Person)
                .Map(dest => dest.Roles, src => src.RolUsers.Select(r => r.Rol.Name).ToList());

            config.NewConfig<User, UserSelectDto>()
                //.Map(dest=>dest.active,src=>src.Active)
                .Map(dest => dest.FullName, src => $"{src.Person.FirstName} {src.Person.LastName}")
                .Map(dest => dest.PhoneNumber, src => src.Person.PhoneNumber)
                .Map(dest => dest.Address, src => src.Person.Address)
                .Map(dest => dest.Identification, src => src.Person.Identification)
                .Map(dest => dest.CityId, src => src.Person.CityId)
                .Map(dest => dest.CityName, src => src.Person.City.Name);



            //FarmWith PRoducer a producer y famr
            config.NewConfig<ProducerWithFarmRegisterDto, Producer>();
            config.NewConfig<ProducerWithFarmRegisterDto, Farm>().Ignore(des => des.FarmImages);
            config.NewConfig<ProducerWithFarmRegisterDto, FarmRegisterDto>();

            config.NewConfig<FarmRegisterDto, Farm>().Ignore(des => des.FarmImages);

            config.NewConfig<FarmImage, FarmImageSelectDto>()
                .MapWith(src => new FarmImageSelectDto(
                    src.Id,
                    src.FileName ?? string.Empty,
                      src.ImageUrl ?? string.Empty,
                      src.PublicId ?? string.Empty,
                      src.FarmId
                    ));

            config.NewConfig<Farm, FarmSelectDto>()
                .Map(dest => dest.CityName, src => src.City.Name)
                .Map(dest => dest.DepartmentName, src => src.City.Department.Name)
                .Map(dest => dest.ProducerName, src => $"{src.Producer.User.Person.FirstName} {src.Producer.User.Person.LastName}")
                .Map(dest => dest.Images, src => src.FarmImages ?? new List<FarmImage>());

            config.NewConfig<FarmUpdateDto,Farm>()
                 .Ignore(dest => dest.FarmImages)   // Se manejan aparte
                .Ignore(dest => dest.Active)   // No se actualiza desde DTO
                .IgnoreNullValues(true);



            //Products
            config.NewConfig<ProductCreateDto, Product>().Ignore(des => des.ProductImages);
            config.NewConfig<ProductImage, ProductImageSelectDto>()
                  .MapWith(src => new ProductImageSelectDto(
                      src.Id,
                      src.FileName ?? string.Empty,
                      src.ImageUrl ?? string.Empty,
                      src.PublicId ?? string.Empty,
                      src.ProductId
                  ));
            // DTO de actualización → Entidad (ignorar nulos y valores por defecto)
            config.NewConfig<ProductUpdateDto, Product>()
                .Ignore(dest => dest.ProductImages)   // Se manejan aparte
                .Ignore(dest => dest.Active)   // No se actualiza desde DTO
                .IgnoreNullValues(true);


            config.NewConfig<Product, ProductSelectDto>()
                  .Map(dest => dest.PersonName,
                       src => (src.Farm != null && src.Farm.Producer != null &&
                               src.Farm.Producer.User != null && src.Farm.Producer.User.Person != null)
                            ? (src.Farm.Producer.User.Person.FirstName + " " +
                               src.Farm.Producer.User.Person.LastName)
                            : string.Empty)
                  // Mapear la colección usando el mapeo ProductImage -> ProductImageSelectDto
                  .Map(dest => dest.Images, src => src.ProductImages ?? new List<ProductImage>());

            //Category
            // Updated mapping to handle potential null references
            config.NewConfig<Category, CategorySelectDto>()
                .Map(dest => dest.Id, src => src.Id) // si no se mapea automáticamente
                .Map(dest => dest.Name, src => src.Name)
                .Map(dest => dest.ParentCategoryId, src => src.ParentCategoryId)
                .Map(dest => dest.ParentName, src => src.ParentCategory != null ? src.ParentCategory.Name : null);

            config.NewConfig<CategoryRegisterDto, Category>();


            //Security
            config.NewConfig<Rol, RolSelectDto>();
            config.NewConfig<RolRegisterDto, Rol>();

            config.NewConfig<RolUser, RolUserSelectDto>()
                .Map(dest => dest.UserName, src => src.User.Person.FirstName)
                .Map(dest => dest.RolName, src => src.Rol.Name);

            config.NewConfig<RolFormPermission, RolFormPermissionSelectDto>()
                .Map(dest => dest.RolName, src => src.Rol.Name)
                .Map(dest => dest.FormName, src => src.Form.Name)
                .Map(dest => dest.PermissionName, src => src.Permission.Name);
                


            //LOcation
            //config.NewConfig<City, CitySelectDto>()
            //    .Map(dest => dest.DepartmentName, src => src.Department.Name);







            return config;
        }
    }
}
