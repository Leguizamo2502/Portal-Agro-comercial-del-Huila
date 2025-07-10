
using Entity.Context;
using Entity.DTOs;
using Entity.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.SqlServer.Server;

namespace Data
{
    public class RolFormPermissionData
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<RolFormPermissionData> _logger;

        public RolFormPermissionData(ApplicationDbContext context, ILogger<RolFormPermissionData> logger)
        {
            _context = context;
            _logger = logger;
        }
        // ================================================
        // Métodos SQL
        // ================================================
        public async Task<IEnumerable<RolFormPermissionDto>> GetAllAsync()
        {
            try
            {
                //SqlServer
                //string query = @"SELECT 
                //                    RFP.Id AS Id, 
                //                    R.Id AS RolId, 
                //                    R.Name AS RolName, 
                //                    F.Id AS FormId, 
                //                    F.Name AS FormName, 
                //                    P.Id AS PermissionId, 
                //                    P.Name AS PermissionName
                //                FROM RolFormPermission AS RFP
                //                INNER JOIN Rol AS R ON RFP.RolId = R.Id
                //                INNER JOIN Form AS F ON RFP.FormId = F.Id
                //                INNER JOIN Permission AS P ON RFP.PermissionId = P.Id
                //                WHERE RFP.IsDeleted = 0; ";

                // postgresql
                //string query = @"SELECT 
                //                    rfp.id AS id, 
                //                    r.id AS rolid, 
                //                    r.name AS rolname, 
                //                    f.id AS formid, 
                //                    f.name AS formname, 
                //                    p.id AS permissionid, 
                //                    p.name AS permissionname
                //                FROM rolformpermission AS rfp
                //                INNER JOIN rol AS r ON rfp.rolid = r.id
                //                INNER JOIN form AS f ON rfp.formid = f.id
                //                INNER JOIN ""Permission"" AS p ON rfp.permissionid = p.id
                //                WHERE rfp.isdeleted = false;";

                //MySql
                string query = @"SELECT 
                                    RFP.Id AS Id, 
                                    R.Id AS RolId, 
                                    R.Name AS RolName, 
                                    F.Id AS FormId, 
                                    F.Name AS FormName, 
                                    P.Id AS PermissionId, 
                                    P.Name AS PermissionName
                                FROM RolFormPermission AS RFP
                                INNER JOIN Rol AS R ON RFP.RolId = R.Id
                                INNER JOIN Form AS F ON RFP.FormId = F.Id
                                INNER JOIN Permission AS P ON RFP.PermissionId = P.Id
                                WHERE RFP.IsDeleted = 0; ";


                return await _context.QueryAsync<RolFormPermissionDto>(query);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener el permiso de rol {RolFormPermissionId}");
                throw; // Relanza la excepcion  para q sea manejada por las capas superiores

            }
        }

        public async Task<RolFormPermission?> GetByIdAsync(int id)
        {
            try
            {
                //SqlServer
                //string query = @"SELECT 
                //                   RFP.Id AS Id, 
                //                    R.Id AS RolId, 
                //                    R.Name AS RolName, 
                //                    F.Id AS FormId, 
                //                    F.Name AS FormName, 
                //                    P.Id AS PermissionId, 
                //                    P.Name AS PermissionName
                //                FROM RolFormPermission AS RFP
                //                INNER JOIN Rol AS R ON RFP.RolId = R.Id
                //                INNER JOIN Form AS F ON RFP.FormId = F.Id
                //                INNER JOIN Permission AS P ON RFP.PermissionId = P.Id
                //                 WHERE RFP.Id = @Id AND RFP.IsDeleted = 0";

                // postgresql
                //string query = @"SELECT 
                //                    rfp.id AS id, 
                //                    r.id AS rolid, 
                //                    r.name AS rolname, 
                //                    f.id AS formid, 
                //                    f.name AS formname, 
                //                    p.id AS permissionid, 
                //                    p.name AS permissionname
                //                FROM rolformpermission AS rfp
                //                INNER JOIN rol AS r ON rfp.rolid = r.id
                //                INNER JOIN form AS f ON rfp.formid = f.id
                //                INNER JOIN ""Permission"" AS p ON rfp.permissionid = p.id
                //                WHERE rfp.id = @id AND rfp.isdeleted = false;";

                //MySql
                string query = @"SELECT 
                                   RFP.Id AS Id, 
                                    R.Id AS RolId, 
                                    R.Name AS RolName, 
                                    F.Id AS FormId, 
                                    F.Name AS FormName, 
                                    P.Id AS PermissionId, 
                                    P.Name AS PermissionName
                                FROM RolFormPermission AS RFP
                                INNER JOIN Rol AS R ON RFP.RolId = R.Id
                                INNER JOIN Form AS F ON RFP.FormId = F.Id
                                INNER JOIN Permission AS P ON RFP.PermissionId = P.Id
                                 WHERE RFP.Id = @Id AND RFP.IsDeleted = 0";


                return await _context.QueryFirstOrDefaultAsync<RolFormPermission>(query, new { Id = id });

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener el permiso de rol con ID {RolFormPermissionId}", id);
                throw; // Relanza la excepcion  para q sea manejada por las capas superiores

            }
        }

        public async Task<RolFormPermissionDto?> GetByIdDtoAsync(int id)
        {
            try
            {
                //SqlServer
                //string query = @"SELECT 
                //                   RFP.Id AS Id, 
                //                    R.Id AS RolId, 
                //                    R.Name AS RolName, 
                //                    F.Id AS FormId, 
                //                    F.Name AS FormName, 
                //                    P.Id AS PermissionId, 
                //                    P.Name AS PermissionName
                //                FROM RolFormPermission AS RFP
                //                INNER JOIN Rol AS R ON RFP.RolId = R.Id
                //                INNER JOIN Form AS F ON RFP.FormId = F.Id
                //                INNER JOIN Permission AS P ON RFP.PermissionId = P.Id
                //                 WHERE RFP.Id = @Id AND RFP.IsDeleted = 0";

                // postgresql
                //string query = @"SELECT 
                //                    rfp.id AS id, 
                //                    r.id AS rolid, 
                //                    r.name AS rolname, 
                //                    f.id AS formid, 
                //                    f.name AS formname, 
                //                    p.id AS permissionid, 
                //                    p.name AS permissionname
                //                FROM rolformpermission AS rfp
                //                INNER JOIN rol AS r ON rfp.rolid = r.id
                //                INNER JOIN form AS f ON rfp.formid = f.id
                //                INNER JOIN ""Permission"" AS p ON rfp.permissionid = p.id
                //                WHERE rfp.id = @id AND rfp.isdeleted = false;";

                //MySql
                string query = @"SELECT 
                                   RFP.Id AS Id, 
                                    R.Id AS RolId, 
                                    R.Name AS RolName, 
                                    F.Id AS FormId, 
                                    F.Name AS FormName, 
                                    P.Id AS PermissionId, 
                                    P.Name AS PermissionName
                                FROM RolFormPermission AS RFP
                                INNER JOIN Rol AS R ON RFP.RolId = R.Id
                                INNER JOIN Form AS F ON RFP.FormId = F.Id
                                INNER JOIN Permission AS P ON RFP.PermissionId = P.Id
                                 WHERE RFP.Id = @Id AND RFP.IsDeleted = 0";

                return await _context.QueryFirstOrDefaultAsync<RolFormPermissionDto>(query, new { Id = id });

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener el permiso de rol con ID {RolFormPermissionId}", id);
                throw; // Relanza la excepcion  para q sea manejada por las capas superiores

            }
        }


        // Método para crear un RolFormPermission con SQL
        public async Task<RolFormPermission> CreateAsync(RolFormPermission rolFormPermission)
        {
            try
            {
                //SqlServer
                //string query = @"INSERT INTO RolFormPermission" +
                //                " (RolId, FormId, PermissionId, IsDeleted)" +
                //                " OUTPUT INSERTED.Id" +
                //                " VALUES" +
                //                " (@RolId, @FormId, @PermissionId, @IsDeleted)";

                // postgresql
                //string query = @"INSERT INTO rolformpermission 
                //                    (rolid, formid, permissionid, isdeleted)
                //                    VALUES 
                //                    (@rolid, @formid, @permissionid, @IsDeleted)
                //                    RETURNING id;";

                //MySql
                //MySql
                string query = @"INSERT INTO RolFormPermission 
                                (RolId, FormId, PermissionId, IsDeleted)
                             VALUES 
                                (@RolId, @FormId, @PermissionId, @IsDeleted); 
                             SELECT LAST_INSERT_ID();";



                var Parameters = new
                {
                    rolFormPermission.RolId,
                    rolFormPermission.FormId,
                    rolFormPermission.PermissionId,
                    IsDeleted = false,

                };

                rolFormPermission.Id = await _context.ExecuteScalarAsync<int>(query, Parameters);
                return rolFormPermission;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"no se pudo agregar rolFormPermission {rolFormPermission}");
                throw;
            }
        }

        // Método para Actualizar un RolFormPermission con SQL
        public async Task<bool> UpdateAsync(RolFormPermission rolFormPermission)
        {
            try
            {
                //SqlServer
                //string query = @"UPDATE RolFormPermission" +
                //                " SET" +
                //                " RolId = @RolId," +
                //                " FormId = @FormId," +
                //                " PermissionId = @PermissionId" +
                //                " WHERE Id = @Id AND IsDeleted = 0;";

                // postgresql
                //string query = @"UPDATE rolformpermission
                //            SET 
                //            rolid = @rolid,
                //            formid = @formid,
                //            permissionid = @permissionid
                //            WHERE id = @Id AND isdeleted = false;";

                //MySql
                string query = @"UPDATE RolFormPermission
                                 SET
                                 RolId = @RolId,
                                 FormId = @FormId,
                                 PermissionId = @PermissionId
                                 WHERE Id = @Id AND IsDeleted = 0;";

                var Parameters = new
                {
                    rolFormPermission.Id,
                    rolFormPermission.RolId,
                    rolFormPermission.FormId,
                    rolFormPermission.PermissionId,
                };

                
                int rowsAffected = await _context.ExecuteAsync(query, Parameters);
                return rowsAffected>0; 
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"no se pudo agregar rolFormPermission {rolFormPermission}");
                throw;
            }
        }

        // Método para Eliminar un rolFormPermission Logico SQL
        public async Task<bool> DeleteLogicAsync(int id)
        {
            try
            {
                //SqlServer
                //string query = @"UPDATE RolFormPermission" +
                //                " SET" +
                //                " IsDeleted = 1" +
                //                " WHERE Id = @Id";

                // postgresql
                //string query = @"UPDATE rolformpermission
                //                SET isdeleted = true
                //                WHERE id = @Id;";

                //MySql
                string query = @"UPDATE RolFormPermission
                                 SET IsDeleted = 1
                                 WHERE Id = @Id;";

                int rowsAffected = await _context.ExecuteAsync(query, new { Id = id });
                return rowsAffected > 0;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al borra logicamente con ID {FormModuleId}");
                throw;
            }
        }

        // Método para eleminar un rolFormPermission con persistencia SQL
        public async Task<bool> DeletePersistenceAsync(int id)
        {
            try
            {
                //SqlServer
                //string query = @"DELETE FROM RolFormPermission
                //                 WHERE Id = @Id";

                // postgresql
                //string query = @"DELETE FROM rolformpermission
                //                    WHERE id = @Id;";

                //MySql
                string query = @"DELETE FROM RolFormPermission
                                 WHERE Id = @Id;";

                int rowsAffected = await _context.ExecuteAsync(query, new { Id = id });
                return rowsAffected > 0; // almenos una fila
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al eliminar permanentemente rolFormPermission con ID {Id}", id);
                throw;
            }
        }

        // ================================================
        // Métodos LINQ
        // ================================================

        // Obtner todos los RolFormPermision
        public async Task<IEnumerable<RolFormPermission>> GetAllLinQAsync()
        {
            return await _context.Set<RolFormPermission>()
                .Include(rfp => rfp.Rol)
                .Include(rfp => rfp.Form)
                .Include(rfp => rfp.Permission)
                .Where(rfp => !rfp.IsDeleted)
                .ToListAsync();
        }

        public async Task<RolFormPermission?> GetByIdLinQAsync(int id)
        {
            try
            {
                return await _context.Set<RolFormPermission>()
                                   .Include(rfp => rfp.Rol)
                                    .Include(rfp => rfp.Form)
                                    .Include(rfp => rfp.Permission)
                                    .FirstOrDefaultAsync(rfp => rfp.Id == id);

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener el permiso de rol con ID {RolFormPermissionId}", id);
                throw; // Relanza la excepcion  para q sea manejada por las capas superiores

            }
        }


        // Crear RolFormPermission con LINQ
        public async Task<RolFormPermission> CreateLinQAsync(RolFormPermission rolFormPermission)
        {
            try
            {
                await _context.Set<RolFormPermission>().AddAsync(rolFormPermission);
                rolFormPermission.IsDeleted = false;
                await _context.SaveChangesAsync();
                return rolFormPermission;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al crear RolFormPermission");
                throw;
            }
        }

        // Actualizar RolFormPermission
        public async Task<bool> UpdateLinQAsync(RolFormPermission rolFormPermission)
        {

            if (rolFormPermission is null)
            {
                throw new ArgumentNullException(nameof(RolFormPermission), "El formModule no puede ser nulo.");
            }

            try
            {
                _context.Set<RolFormPermission>().Update(rolFormPermission);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al actualizar RolFormPermission");
                return false;
            }
        }

        // Eliminar RolUser Logico LINQ
        public async Task<bool> DeleteLoqicLinQAsinc(int id)
        {
            try
            {
                var rolFormPermission = await GetByIdLinQAsync(id);
                if (rolFormPermission == null)
                    return false;

                rolFormPermission.IsDeleted = true;
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al eliminar lógicamente el user con ID {id}", id);
                throw;
            }
        }

        // Eliminar RolFormPermission con LINQ Persistente
        public async Task<bool> DeleteLinQAsync(int id)
        {
            try
            {
                var rolFormPermission = await GetByIdAsync(id);
                if (rolFormPermission == null)
                {
                    return false;
                }
                _context.Set<RolFormPermission>().Remove(rolFormPermission);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al eliminar RolFormPermission con el ID {RolFormPermissionId}", id);
                return false;
            }
        }
    }
}