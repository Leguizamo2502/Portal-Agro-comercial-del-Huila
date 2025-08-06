using Business.Interfaces.Implements.Security.Mes;
using Data.Interfaces.Implements.Security.Mes;
using Entity.DTOs.Auth;
using Entity.DTOs.Security.Me;
using Mapster;
using MapsterMapper;
using Microsoft.SqlServer.Server;
using Utilities.Exceptions;

public class MeService : IMeService
{
    private readonly IMeRepository _meRepository;
    private readonly IMapper _mapper;

    public MeService(IMeRepository meRepository, IMapper mapper)
    {
        _meRepository = meRepository;
        _mapper = mapper;
    }

    /// <summary>
    /// Construye el contexto de usuario con información detallada, roles, permisos y estructura de menú.
    /// </summary>
    /// <param name="userId">Identificador único del usuario.</param>
    /// <returns>
    /// Objeto <see cref="UserMeDto"/> con toda la información contextual del usuario.
    /// </returns>
    /// <exception cref="BusinessException">
    /// Se lanza cuando no se encuentra el usuario especificado.
    /// </exception>
    /// <remarks>
    /// Este método:
    /// 1. Obtiene datos básicos del usuario
    /// 2. Recupera y filtra roles activos
    /// 3. Construye la estructura de permisos
    /// 4. Organiza los módulos y formularios para el menú
    /// </remarks>
    public async Task<UserMeDto> GetAllDataMeAsync(int userId)
    {
        // 1. Obtener usuario principal (con patrón null-coalescing throw)
        var user = await _meRepository.GetUserWithPersonAsync(userId)
                    ?? throw new BusinessException("Usuario no encontrado");

        // 2. Obtener roles con sus permisos
        var userRoles = await _meRepository.GetUserRolesWithPermissionsAsync(userId);

        // 3. Filtrar roles activos no eliminados (con DistinctBy para evitar duplicados)
        var roles = userRoles
            .Where(ur => ur.Active && !ur.IsDeleted)
            .Select(ur => ur.Rol)
            .Where(r => r.Active && !r.IsDeleted)
            .DistinctBy(r => r.Id)
            .ToList();

        var roleNames = roles.Select(r => r.Name).ToList();

        // 4. Construir diccionario de permisos:
        //    - Key: ID de formulario
        //    - Value: Conjunto de permisos para ese formulario
        var formPermissions = new Dictionary<int, HashSet<string>>();
        var permissions = new HashSet<string>();

        foreach (var role in roles)
        {
            foreach (var rfp in role.RolFormPermissions)
            {
                if (string.IsNullOrWhiteSpace(rfp.Permission?.Name)) continue;

                permissions.Add(rfp.Permission.Name);

                if (!formPermissions.ContainsKey(rfp.FormId))
                    formPermissions[rfp.FormId] = new();

                formPermissions[rfp.FormId].Add(rfp.Permission.Name);
            }
        }

        // 5. Obtener formularios con sus módulos
        var formIds = formPermissions.Keys.ToList();
        var formsWithModules = await _meRepository.GetFormsWithModulesByIdsAsync(formIds);

        // 6. Organizar módulos con sus formularios y permisos
        var modules = formsWithModules
            .Where(f => f.FormModules.Any())
            .GroupBy(f => f.FormModules.First().Module)
            .Select(g =>
            {
                var module = g.Key.Adapt<MenuModuleDto>();
                module.Forms = g.Select(form =>
                {
                    var dto = form.Adapt<FormMeDto>();
                    dto.Permissions = formPermissions[form.Id];
                    return dto;
                }).ToList();

                return module;
            }).ToList();

        // 7. Construir DTO final
        return new UserMeDto
        {
            Id = user.Id,
            FullName = $"{user.Person.FirstName} {user.Person.LastName}",
            Email = user.Email,
            Roles = roleNames,
            Permissions = permissions.ToList(),
            Menu = modules
        };
    }
}

