using Entity.Domain.Models.Implements.Auth;
using Entity.Domain.Models.Implements.Orders;
using Entity.Domain.Models.Implements.Producers;
using Entity.Domain.Models.Implements.Producers.Farms;
using Entity.Domain.Models.Implements.Producers.Products;
using Entity.Domain.Models.Implements.Security;
using Entity.DTOs.Auth;
using Entity.DTOs.Auth.User;
using Entity.DTOs.Order.Create;
using Entity.DTOs.Order.Reviews;
using Entity.DTOs.Order.Select;
using Entity.DTOs.Producer.Categories;
using Entity.DTOs.Producer.Farm.Create;
using Entity.DTOs.Producer.Farm.Select;
using Entity.DTOs.Producer.Farm.Update;
using Entity.DTOs.Producer.Producer.Select;
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
                .Map(dest => dest.FirstName, src =>src.Person.FirstName)
                .Map(dest=> dest.LastName, src => src.Person.LastName)
                .Map(dest => dest.PhoneNumber, src => src.Person.PhoneNumber)
                .Map(dest => dest.Address, src => src.Person.Address)
                .Map(dest => dest.Identification, src => src.Person.Identification)
                .Map(dest => dest.CityId, src => src.Person.CityId)
                .Map(dest => dest.DepartmentId, src => src.Person.City.DepartmentId)

                .Map(dest => dest.CityName, src => src.Person.City.Name);



            //Reviews
            config.NewConfig<Review, ReviewSelectDto>()
                .Map(dest => dest.UserName, src => $"{src.User.Person.FirstName} {src.User.Person.LastName}");

            



            //FarmWith PRoducer a producer y famr
            config.NewConfig<ProducerWithFarmRegisterDto, Producer>()
                .Ignore(dest => dest.SocialLinks);
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


            //Producer
            config.NewConfig<Producer, ProducerSelectDto>()
                .Map(dest => dest.FullName, src => $"{src.User.Person.FirstName} {src.User.Person.LastName}")
                .Map(dest => dest.Email, src => src.User.Email)
                .Map(dest => dest.PhoneNumber, src => src.User.Person.PhoneNumber)
                .Map(dest => dest.Networks, src => src.SocialLinks);
                


            //Products
            // Config global de Mapster
            config.NewConfig<ProductCreateDto, Product>()
                .Ignore(dest => dest.ProductImages)
                .Ignore(dest => dest.ProductFarms)
                .Ignore(dest => dest.Producer)      // nav
                .Ignore(dest => dest.Category)      // nav
                .Ignore(dest => dest.ProducerId);   // lo asignas tú (pid)

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
                .Ignore(d => d.ProductImages)
                .Ignore(d => d.ProductFarms)
                .Ignore(d => d.Category)   // nav
                .Ignore(d => d.Producer)// nav  // No se actualiza desde DTO
                .IgnoreNullValues(true);


            // Product -> ProductSelectDto (toma la primera finca asociada como legacy)
            config.NewConfig<Product, ProductSelectDto>()
                .Map(dest => dest.CategoryName, src => src.Category.Name)
                .Map(dest => dest.Images, src => src.ProductImages.Where(pi => !pi.IsDeleted))
                .Map(dest => dest.ProducerCode, src => src.Producer.Code)

                // NUEVO: lista completa de fincas activas
                .Map(dest => dest.FarmIds,
                     src => src.ProductFarms
                               .Where(pf => !pf.IsDeleted)
                               .Select(pf => pf.FarmId)
                               .ToList())

                // Legacy: una sola finca (primera por Id para estabilidad)
                .Map(dest => dest.FarmId,
                     src => src.ProductFarms
                               .Where(pf => !pf.IsDeleted)
                               .OrderBy(pf => pf.FarmId)
                               .Select(pf => (int?)pf.FarmId)
                               .FirstOrDefault() ?? 0)

                .Map(dest => dest.FarmName,
                     src => src.ProductFarms
                               .Where(pf => !pf.IsDeleted)
                               .OrderBy(pf => pf.FarmId)
                               .Select(pf => pf.Farm.Name)
                               .FirstOrDefault() ?? string.Empty)

                .Map(dest => dest.CityName,
                     src => src.ProductFarms
                               .Where(pf => !pf.IsDeleted)
                               .OrderBy(pf => pf.FarmId)
                               .Select(pf => pf.Farm.City.Name)
                               .FirstOrDefault() ?? string.Empty)

                .Map(dest => dest.DepartmentName,
                     src => src.ProductFarms
                               .Where(pf => !pf.IsDeleted)
                               .OrderBy(pf => pf.FarmId)
                               .Select(pf => pf.Farm.City.Department.Name)
                               .FirstOrDefault() ?? string.Empty)

                .Map(dest => dest.PersonName,
                     src => src.ProductFarms
                               .Where(pf => !pf.IsDeleted)
                               .OrderBy(pf => pf.FarmId)
                               .Select(pf => pf.Farm.Producer.User.Person != null
                                     ? (pf.Farm.Producer.User.Person.FirstName + " " + pf.Farm.Producer.User.Person.LastName)
                                     : null)
                               .FirstOrDefault() ?? string.Empty);


            //Orders

            // Listado
            config.NewConfig<Order, OrderListItemDto>()
                  .Map(d => d.ProductName, s => s.ProductNameSnapshot)
                  .Map(d => d.QuantityRequested, s => s.QuantityRequested)
                  .Map(d => d.Subtotal, s => s.Subtotal)
                  .Map(d => d.Total, s => s.Total)
                  .Map(d => d.Status, s => s.Status.ToString())
                  .Map(d => d.PaymentImageUrl, s => s.PaymentImageUrl)  // quedará null al crear (esperable)
                  .Map(d => d.CreateAt, s => s.CreateAt);

            // Detalle
            config.NewConfig<Order, OrderDetailDto>()
                  .Map(d => d.ProductId, s => s.ProductId)
                  .Map(d => d.ProductName, s => s.ProductNameSnapshot)
                  .Map(d => d.UnitPrice, s => s.UnitPriceSnapshot)
                  .Map(d => d.QuantityRequested, s => s.QuantityRequested)
                  .Map(d => d.Subtotal, s => s.Subtotal)
                  .Map(d => d.Total, s => s.Total)
                  .Map(d => d.Status, s => s.Status.ToString())
                  .Map(d => d.UserReceivedAnswer, s => s.UserReceivedAnswer.ToString())
                  .Map(d => d.PaymentImageUrl, s => s.PaymentImageUrl)      // null en creación
                  .Map(d => d.PaymentUploadedAt, s => s.PaymentUploadedAt)  // null en creación
                  .Map(d => d.ProducerDecisionAt, s => s.ProducerDecisionAt)
                  .Map(d => d.ProducerDecisionReason, s => s.ProducerDecisionReason)
                  .Map(d => d.ProducerNotes, s => s.ProducerNotes)
                  .Map(d => d.RecipientName, s => s.RecipientName)
                  .Map(d => d.ContactPhone, s => s.ContactPhone)
                  .Map(d => d.AddressLine1, s => s.AddressLine1)
                  .Map(d => d.AddressLine2, s => s.AddressLine2)
                  .Map(d => d.CityId, s => s.CityId)
                  .Map(d=> d.CityName, s => s.City.Name)
                  .Map(d => d.DepartmentName, s => s.City.Department.Name)
                  .Map(d => d.AdditionalNotes, s => s.AdditionalNotes)
                  .Map(d => d.UserReceivedAt, s => s.UserReceivedAt)
                  .Map(d => d.CreateAt, s => s.CreateAt)
                  .Map(d => d.RowVersion, s => Convert.ToBase64String(s.RowVersion ?? Array.Empty<byte>()));


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
