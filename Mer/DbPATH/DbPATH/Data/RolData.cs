using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entity.Context;
using Entity.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;


namespace Data
{
    public class RolData
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<RolData> _logger;

        public RolData(ApplicationDbContext context, ILogger<RolData> logger)
        {
            _context = context;
            _logger = logger;
        }

        //Metodo para traer todo SQL
        public async Task<IEnumerable<Rol>> GetAllAsync()
        {

            try
            {
                //SqlServer
                //string query = @"SELECT * FROM Rol WHERE IsDeleted = 0;";

                // PostgreSQL
                //string query = @"SELECT * FROM rol WHERE isdeleted = FALSE;";

                //MySql
                string query = @"SELECT * FROM Rol WHERE IsDeleted = 0;";





                return await _context.QueryAsync<Rol>(query);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener Los roles {Rol}");
                throw; // Relanza la excepcion  para q sea manejada por las capas superiores
            }
            

        }


        //Metodo para traer por id SQL
        public async Task<Rol?> GetByIdAsync(int id)
        {
            try
            {
                //SqlServer
                //string query = @"SELECT * FROM Rol WHERE Id = @Id AND IsDeleted = 0;";

                // PostgreSQL
                //string query = @"SELECT * FROM rol WHERE id = @Id AND isdeleted = FALSE;";

                //MySql
                string query = @"SELECT * FROM Rol WHERE Id = @Id AND IsDeleted = 0;";

                return await _context.QueryFirstOrDefaultAsync<Rol>(query, new { Id = id });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener el rol con ID {RolId}", id);
                throw; // Relanza la excepcion  para q sea manejada por las capas superiores
            }
        }


        //Metodo para crear SQL
        public async Task<Rol> CreateAsync(Rol rol)
        {
            try
            {
                //SqlServer
                //string query = @"
                //    INSERT INTO Rol (Name, Code, IsDeleted, CreateAt) 
                //    OUTPUT INSERTED.Id 
                //    VALUES (@Name, @Code, @IsDeleted, @CreateAt);";

                // PostgreSQL
                //string query = @"
                //        INSERT INTO rol (name, code, isdeleted, createat)
                //        VALUES (@Name, @Code, FALSE, @CreateAt)
                //        RETURNING id;";

                //MySql
                string query = @"
                        INSERT INTO Rol (Name, Code, IsDeleted, CreateAt) 
                        VALUES (@Name, @Code, @IsDeleted, @CreateAt);
                        SELECT LAST_INSERT_ID();";

                rol.Id = await _context.QueryFirstOrDefaultAsync<int>(query, new
                {
                    rol.Name,
                    rol.Code,
                    IsDeleted =  false,
                    CreateAt = DateTime.UtcNow,// Asigna la fecha actual
                    //DeleteAt = DateTime.UtcNow
                });

                return rol;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al crear el rol.");
                throw;
            }
        }

        //Metodo para actualizar SQL

        public async Task<bool> UpdateAsync(Rol rol)
        {
            try
            {
                //SqlServer
                //string query = @"UPDATE Rol
                //               SET Name = @Name, Code=@Code
                //                WHERE Id = @Id AND IsDeleted = 0;
                //                ";

                // PostgreSQL
                //string query = @"
                //        UPDATE rol
                //        SET name = @Name, code = @Code
                //        WHERE id = @Id AND isdeleted = FALSE;";

                //MySql
                string query = @"
                        UPDATE Rol
                        SET Name = @Name, Code=@Code
                        WHERE Id = @Id AND IsDeleted = 0;";

                // var parameters = new { FormId = formModule.FormId, ModuleId = formModule.ModuleId };
                var parameters = new { 
                    Name = rol.Name, 
                    Code=rol.Code, 
                    Id = rol.Id 
                };

                int rowsAffected = await _context.ExecuteAsync(query, parameters);
                return rowsAffected > 0;


            }
            catch(Exception ex)
            {
                _logger.LogError($"Error al actualizar el rol: {ex.Message}");
                return false;
            }
        }


        //Metodo para borrar logico SQL Data
        public async Task<bool> DeleteLogicAsync(int id)
        {
            try
            {
                //SqlServer
                //string query = @"UPDATE Rol
                //               SET IsDeleted = 1
                //               WHERE Id=@Id";

                // PostgreSQL
                //string query = @"UPDATE rol SET isdeleted = TRUE WHERE id = @Id;";

                //MySql
                string query = @"UPDATE Rol SET IsDeleted = 1 WHERE Id=@Id;";

                int rowsAffected = await _context.ExecuteAsync(query, new { id });
                return rowsAffected > 0;
            }catch(Exception ex)
            {
                _logger.LogError($"Error al eliminar logicamente rol: {ex.Message}");
                return false;
            }
        }

        //Metodo para borrar persistente SQL
        public async Task<bool> DeletePersistenceAsync(int id)
        {
            try
            {
                //SqlServer
                //string query = @"
                //               DELETE FROM Rol
                //               WHERE Id = @Id";

                // PostgreSQL
                //string query = @"DELETE FROM rol WHERE id = @Id;";

                //MySql
                string query = @"DELETE FROM Rol WHERE Id = @Id;";

                int rowsAffected = await _context.ExecuteAsync(query, new { Id = id });

                return rowsAffected > 0;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error al eliminar rol: {ex.Message}");
                return false;
            }
        }





        //Metodo para obtener Todo LinQ
        public async Task<IEnumerable<Rol>> GetAllLinQAsync()

        {

            return await _context.Set<Rol>()
                .Where(r=>!r.IsDeleted)
                .ToListAsync();


        }


        //Metodo para obtener por id LinQ

        public async Task<Rol?> GetByIdLinQAsync(int id)
        {
            try
            {
                return await _context.Set<Rol>().FindAsync(id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener la rola con ID {RolId}", id);
                throw; // Relanza la excepcion  para q sea manejada por las capas superiores
            }
        }

        //Metodo para crear LinQ
        public async Task<Rol> CreateLinQAsync(Rol rol)
        {
            try
            {
                await _context.Set<Rol>().AddAsync(rol);
                await _context.SaveChangesAsync();
                return rol;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error al crear la rola: {ex.Message}");
                throw;
            }
        }

        //Metodo para actualizar LinQ
        public async Task<bool> UpdateLinQAsync(Rol rol)
        {
            try
            {
                _context.Set<Rol>().Update(rol);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error al actualizar la rola: {ex.Message}");
                throw;
            }
        }


        //Metodo para borrar logico LinQ
        public async Task<bool> DeleteLogicLinQAsync(int id)
        {
            try
            {
                var rol = await GetByIdLinQAsync(id);
                if (rol == null)
                {
                    return false;
                }

                // Marcar como eliminado lógicamente
                rol.IsDeleted = true; // O rol.Active = 0; si es un campo numérico
                await _context.SaveChangesAsync();

                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al eliminar lógicamente el rol con ID {id}", id);
                throw;
            }
        }

        //Metodo para Borrar persistente LinQ
        public async Task<bool> DeletePersistenceLinQAsync(int id)
        {
            try
            {
                var rol = await GetByIdLinQAsync(id);
                if (rol == null)
                {
                    return false;
                }
                _context.Set<Rol>().Remove(rol);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error al eliminar la rola: {ex.Message}");
                throw;
            }
        }




    }
}
