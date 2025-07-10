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
    public class ModuleData
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<ModuleData> _logger;
        public ModuleData(ApplicationDbContext context, ILogger<ModuleData> logger)
        {
            _context = context;
            _logger = logger;
        }

        //Metodo para traer todo SQL
        public async Task<IEnumerable<Module>> GetAllAsync()
        {

            try
            {
                //SqlServer
                //string query = @"SELECT * FROM Module WHERE IsDeleted = 0;";

                //Postgres
                //string query = @"SELECT * FROM ""Module"" WHERE IsDeleted = FALSE;";

                //MySql
                string query = @"SELECT * FROM Module WHERE IsDeleted = 0;";

                return (IEnumerable<Module>)await _context.QueryAsync<Module>(query);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener Los modulees {Module}");
                throw; // Relanza la excepcion  para q sea manejada por las capas superiores
            }


        }


        //Metodo para traer por id SQL
        public async Task<Module?> GetByIdAsync(int id)
        {
            try
            {
                //SqlServer
                //string query = @"SELECT * FROM Module WHERE Id = @Id AND IsDeleted=0;";

                //Postgres
                //string query = @"SELECT * FROM ""Module"" WHERE id = @Id AND isdeleted = FALSE;";

                //MySql
                string query = @"SELECT * FROM Module WHERE Id = @Id AND IsDeleted = 0;";


                return await _context.QueryFirstOrDefaultAsync<Module>(query, new { Id = id });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener el module con ID {ModuleId}", id);
                throw; // Relanza la excepcion  para q sea manejada por las capas superiores
            }
        }


        //Metodo para crear SQL
        public async Task<Module> CreateAsync(Module module)
        {
            try
            {
                //SqlServer
                //string query = @"
                //    INSERT INTO Module (Name, Description, IsDeleted) 
                //    OUTPUT INSERTED.Id 
                //    VALUES (@Name, @Description, @IsDeleted);";

                //Postgres
                //string query = @"
                //        INSERT INTO ""Module"" (name, description, isdeleted) 
                //        VALUES (@Name, @Description, @IsDeleted) 
                //        RETURNING id;";

                //MySql
                string query = @"
                        INSERT INTO Module (Name, Description, IsDeleted) 
                        VALUES (@Name, @Description, @IsDeleted); 
                        SELECT LAST_INSERT_ID();";

                var parameters = new
                {
                    module.Name,
                    module.Description,
                    IsDeleted = false // Se asigna siempre true
                };

                module.Id = await _context.QueryFirstOrDefaultAsync<int>(query, parameters);
                return module;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al crear el módulo.");
                throw;
            }
        }

        //Metodo para actualizar SQL

        public async Task<bool> UpdateAsync(Module module)
        {
            try
            {
                //SqlServer
                //string query = @"
                //    UPDATE Module 
                //    SET Name = @Name, 
                //        Description = @Description
                //    WHERE Id = @Id AND IsDeleted=0;";

                //Postgres
                //string query = @"
                //        UPDATE ""Module"" 
                //        SET name = @Name, 
                //            description = @Description
                //        WHERE id = @Id AND isdeleted = FALSE;";

                //MySql
                string query = @"
                            UPDATE Module 
                            SET Name = @Name, 
                                Description = @Description
                            WHERE Id = @Id AND IsDeleted = 0;";


                var parameters = new
                {
                    module.Name,
                    module.Description,
                    module.Id
                };

                int rowsAffected = await _context.ExecuteAsync(query, parameters);
                return rowsAffected > 0;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al actualizar el módulo con ID {ModuleId}", module.Id);
                throw;
            }
        }


        //Metodo para borrar logico SQL
        public async Task<bool> DeleteLogicAsync(int id)
        {
            try
            {
                //SqlServer
                //string query = @"UPDATE Module
                //               SET IsDeleted = 1
                //               WHERE Id=@Id";

                //Postgres
                //string query = @"UPDATE ""Module"" SET isdeleted = TRUE WHERE id = @Id;";

                //MySql
                string query = @"UPDATE Module SET IsDeleted = 1 WHERE Id = @Id;";


                int rowsAffected = await _context.ExecuteAsync(query, new { id });
                return rowsAffected > 0;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error al eliminar logicamente module: {ex.Message}");
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
                //               DELETE FROM Module
                //               WHERE Id = @Id";

                //Postgres
                //string query = @"DELETE FROM ""Module"" WHERE id = @Id;";

                //MySql
                string query = @"DELETE FROM Module WHERE Id = @Id;";


                int rowsAffected = await _context.ExecuteAsync(query, new { Id = id });

                return rowsAffected > 0;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error al eliminar module: {ex.Message}");
                return false;
            }
        }





        //Metodo para obtener Todo LinQ
        public async Task<IEnumerable<Module>> GetAllLinQAsync()

        {

            return await _context.Set<Module>()
                .Where(m=>!m.IsDeleted)
                .ToListAsync();


        }


        //Metodo para obtener por id LinQ

        public async Task<Module?> GetByIdLinQAsync(int id)
        {
            try
            {
                return await _context.Set<Module>().FindAsync(id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener la modulea con ID {ModuleId}", id);
                throw; // Relanza la excepcion  para q sea manejada por las capas superiores
            }
        }

        //Metodo para crear LinQ
        public async Task<Module> CreateLinQAsync(Module module)
        {
            try
            {
                await _context.Set<Module>().AddAsync(module);
                await _context.SaveChangesAsync();
                return module;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error al crear la modulea: {ex.Message}");
                throw;
            }
        }

        //Metodo para actualizar LinQ
        public async Task<bool> UpdateLinQAsync(Module module)
        {
            try
            {
                _context.Set<Module>().Update(module);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error al actualizar la modulea: {ex.Message}");
                throw;
            }
        }


        //Metodo para borrar logico LinQ
        public async Task<bool> DeleteLogicLinQAsync(int id)
        {
            try
            {
                var module = await GetByIdLinQAsync(id);
                if (module == null)
                {
                    return false;
                }

                // Marcar como eliminado lógicamente
                module.IsDeleted = true; // O module.Active = 0; si es un campo numérico
                await _context.SaveChangesAsync();

                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al eliminar lógicamente el module con ID {id}", id);
                throw;
            }
        }

        //Metodo para Borrar persistente LinQ
        public async Task<bool> DeletePersistenceLinQAsync(int id)
        {
            try
            {
                var module = await GetByIdLinQAsync(id);
                if (module == null)
                {
                    return false;
                }
                _context.Set<Module>().Remove(module);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error al eliminar la modulea: {ex.Message}");
                throw;
            }
        }
    }
}
