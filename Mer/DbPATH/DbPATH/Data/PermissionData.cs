using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.Text;
using System.Threading.Tasks;
using Entity.Context;
using Entity.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Data
{
    public class PermissionData
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<PermissionData> _logger;

        public PermissionData(ApplicationDbContext context, ILogger<PermissionData> logger)
        {
            _context = context;
            _logger = logger;
        }

        //Metodo para traer todo SQL
        public async Task<IEnumerable<Permission>> GetAllAsync()
        {

            try
            {
                //SqlServer
                //string query = @"SELECT * FROM Permission WHERE IsDeleted = 0;";

                //Postgres
                //string query = @"SELECT * FROM ""Permission"" WHERE isdeleted = FALSE;";

                //MySql
                string query = @"SELECT * FROM Permission WHERE IsDeleted = 0;";


                return (IEnumerable<Permission>)await _context.QueryAsync<Permission>(query);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener Los permissiones {Permission}");
                throw; // Relanza la excepcion  para q sea manejada por las capas superiores
            }


        }


        //Metodo para traer por id SQL
        public async Task<Permission?> GetByIdAsync(int id)
        {
            try
            {
                //SqlServer
                //string query = @"SELECT * FROM Permission WHERE Id = @Id AND IsDeleted = 0;";

                //Postgres
                //string query = @"SELECT * FROM ""Permission"" WHERE id = @Id AND isdeleted = FALSE;";

                //MySql
                string query = @"SELECT * FROM Permission WHERE Id = @Id AND IsDeleted = 0;";


                return await _context.QueryFirstOrDefaultAsync<Permission>(query, new { Id = id });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener el permission con ID {PermissionId}", id);
                throw; // Relanza la excepcion  para q sea manejada por las capas superiores
            }
        }


        //Metodo para crear SQL
        public async Task<Permission> CreateAsync(Permission permission)
        {
            try
            {
                //SqlServer
                //string query = @"
                //    INSERT INTO Permission (Name, Description, IsDeleted) 
                //    OUTPUT INSERTED.Id 
                //    VALUES (@Name, @Description, @IsDeleted);"
                //;

                //Postgres
                //string query = @"
                //        INSERT INTO ""Permission"" (name, description, isdeleted)
                //        VALUES (@Name, @Description, @IsDeleted)
                //        RETURNING id;";

                //MySql
                string query = @"
                        INSERT INTO Permission (Name, Description, IsDeleted)
                        VALUES (@Name, @Description, @IsDeleted);
                        SELECT LAST_INSERT_ID();";


                permission.Id = await _context.QueryFirstOrDefaultAsync<int>(query, new
                {
                    permission.Name,
                    permission.Description,
                    IsDeleted = false // Se asegura que siempre sea true
                });

                return permission;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al crear el permiso.");
                throw;
            }
        }

        //Metodo para actualizar SQL

        public async Task<bool> UpdateAsync(Permission permission)
        {
            try
            {
                //SqlServer
                //string query = @"
                //    UPDATE Permission 
                //    SET Name = @Name, 
                //        Description = @Description
                //    WHERE Id = @Id AND IsDeleted = 0;";

                //Postgres
                //string query = @"
                //    UPDATE ""Permission""
                //    SET name = @Name,
                //        description = @Description
                //    WHERE id = @Id AND isdeleted = FALSE;";

                //MySql
                string query = @"
                    UPDATE Permission
                    SET Name = @Name,
                        Description = @Description
                    WHERE Id = @Id AND IsDeleted = 0;";


                int rowsAffected = await _context.ExecuteAsync(query, new
                {
                    permission.Name,
                    permission.Description,
                    permission.Id
                });

                return rowsAffected > 0;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al actualizar el permiso con ID {PermissionId}", permission.Id);
                throw;
            }
        }


        //Metodo para borrar logico SQL
        public async Task<bool> DeleteLogicAsync(int id)
        {
            try
            {
                //SqlServer
                //string query = @"UPDATE Permission
                //               SET IsDeleted = 1
                //               WHERE Id=@Id";

                //Postgres
                //string query = @"UPDATE ""Permission"" SET isdeleted = TRUE WHERE id = @Id;";

                //MySql
                string query = @"UPDATE Permission SET IsDeleted = 1 WHERE Id = @Id;";


                int rowsAffected = await _context.ExecuteAsync(query, new { id });
                return rowsAffected > 0;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error al eliminar logicamente permission: {ex.Message}");
                return false;
            }
        }

        //Metodo para borrar persistente SQL
        public async Task<bool> DeletePersistenceAsync(int id)
        {
            try
            {
                //SQlServer
                //string query = @"
                //               DELETE FROM Permission
                //               WHERE Id = @Id";

                //Postgres
                //string query = @"DELETE FROM ""Permission"" WHERE id = @Id;";

                //MySql
                string query = @"DELETE FROM Permission WHERE Id = @Id;";

                int rowsAffected = await _context.ExecuteAsync(query, new { Id = id });

                return rowsAffected > 0;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error al eliminar permission: {ex.Message}");
                return false;
            }
        }





        //Metodo para obtener Todo LinQ
        public async Task<IEnumerable<Permission>> GetAllLinQAsync()

        {

            return await _context.Set<Permission>()
                .Where(p=>!p.IsDeleted)
                .ToListAsync();


        }


        //Metodo para obtener por id LinQ

        public async Task<Permission?> GetByIdLinQAsync(int id)
        {
            try
            {
                return await _context.Set<Permission>().FindAsync(id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener la permissiona con ID {PermissionId}", id);
                throw; // Relanza la excepcion  para q sea manejada por las capas superiores
            }
        }

        //Metodo para crear LinQ
        public async Task<Permission> CreateLinQAsync(Permission permission)
        {
            try
            {
                await _context.Set<Permission>().AddAsync(permission);
                await _context.SaveChangesAsync();
                return permission;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error al crear la permissiona: {ex.Message}");
                throw;
            }
        }

        //Metodo para actualizar LinQ
        public async Task<bool> UpdateLinQAsync(Permission permission)
        {
            try
            {
                _context.Set<Permission>().Update(permission);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error al actualizar la permissiona: {ex.Message}");
                throw;
            }
        }


        //Metodo para borrar logico LinQ
        public async Task<bool> DeleteLogicLinQAsync(int id)
        {
            try
            {
                var permission = await GetByIdLinQAsync(id);
                if (permission == null)
                {
                    return false;
                }

                // Marcar como eliminado lógicamente
                permission.IsDeleted = true; // O permission.Active = 0; si es un campo numérico
                await _context.SaveChangesAsync();

                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al eliminar lógicamente el permission con ID {id}", id);
                throw;
            }
        }

        //Metodo para Borrar persistente LinQ
        public async Task<bool> DeletePersistenceLinQAsync(int id)
        {
            try
            {
                var permission = await GetByIdLinQAsync(id);
                if (permission == null)
                {
                    return false;
                }
                _context.Set<Permission>().Remove(permission);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error al eliminar la permissiona: {ex.Message}");
                throw;
            }
        }
    }
}
