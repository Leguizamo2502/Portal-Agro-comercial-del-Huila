using Entity.Domain.Models.Implements.Auth;
using Entity.Domain.Models.Implements.Producers;
using Entity.Domain.Models.Implements.Products;
using Entity.Domain.Models.Implements.Security;
using Entity.DTOs.Auth;
using Entity.DTOs.Producer.Categories;
using Entity.DTOs.Producer.Farm.Create;
using Entity.DTOs.Producer.Farm.Select;
using Entity.DTOs.Producer.Producer.Create;
using Entity.DTOs.Products;
using Entity.DTOs.Security.Create.Rols;
using Entity.DTOs.Security.Me;
using Entity.DTOs.Security.Selects.Rols;
using Entity.DTOs.Security.Selects.RolUserDto;
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

            // Map User → UserDto
            config.NewConfig<User, UserDto>()
                .Map(dest => dest.Person, src => src.Person)
                .Map(dest => dest.Roles, src => src.RolUsers.Select(r => r.Rol.Name).ToList());


            //FarmWith PRoducer a producer y famr
            config.NewConfig<ProducerWithFarmRegisterDto, Producer>();
            config.NewConfig<ProducerWithFarmRegisterDto, Farm>();
            config.NewConfig<ProducerWithFarmRegisterDto, FarmRegisterDto>();

            config.NewConfig<FarmRegisterDto, Farm>().Ignore(des => des.FarmImages);

            config.NewConfig<FarmImage, FarmImageDto>();
            config.NewConfig<Farm, FarmSelectDto>()
                .Map(dest => dest.CityName, src => src.City.Name)
                .Map(dest => dest.DepartmentName, src => src.City.Department.Name)
                .Map(dest => dest.ProducerName, src => src.Producer.User.Person.FirstName) // o ajusta según tu modelo
                .Map(dest => dest.Images, src => src.FarmImages.Adapt<List<FarmImageDto>>());


            //Products
            config.NewConfig<ProductCreateDto, Product>().Ignore(des => des.ProductImages);
            config.NewConfig<ProductImage, ProductImageDto>();
            config.NewConfig<Product, ProductSelectDto>()
                .Map(dest=>dest.PersonName,src => $"{src.Farm.Producer.User.Person.FirstName} {src.Farm.Producer.User.Person.LastName}")
                .Map(dest => dest.Images, src => src.ProductImages.Adapt<List<ProductImageDto>>());

            //Category
            // Updated mapping to handle potential null references
            config.NewConfig<Category, CategorySelectDto>()
                .Map(dest => dest.Id, src => src.Id) // si no se mapea automáticamente
                .Map(dest => dest.Name, src => src.Name)
                .Map(dest => dest.ParentId, src => src.ParentCategoryId)
                .Map(dest => dest.ParentName, src => src.ParentCategory != null ? src.ParentCategory.Name : null);

            config.NewConfig<CategoryRegisterDto, Category>();


            //Security
            config.NewConfig<Rol, RolSelectDto>();
            config.NewConfig<RolRegisterDto, Rol>();

            config.NewConfig<RolUser, RolUserSelectDto>()
                .Map(dest => dest.UserName, src => src.User.Person.FirstName)
                .Map(dest => dest.RolName, src => src.Rol.Name);








            return config;
        }
    }
}
