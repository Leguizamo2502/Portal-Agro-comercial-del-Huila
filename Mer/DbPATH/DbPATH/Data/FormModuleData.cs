using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entity.Context;
using Entity.DTOs;
using Entity.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace Data
{
    public class FormModuleData
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<FormModuleData> _logger;

        public FormModuleData(ApplicationDbContext context, ILogger<FormModuleData> logger)
        {
            _context = context;
            _logger = logger;
        }



        //Metodo para traer todo SQL
        public async Task<IEnumerable<FormModuleDto>> GetAllAsync()
        {
            //SqlServer
            //string query = @"SELECT fm.Id,
            //               f.Id AS FormId,
            //               f.Name AS FormName,
            //               m.Id AS ModuleId,
            //               m.Name AS ModuleName
            //        FROM FormModule fm
            //        INNER JOIN Form f ON fm.FormId = f.Id
            //        INNER JOIN Module m ON fm.ModuleId = m.Id
            //        WHERE fm.IsDeleted = 0;";

            //PostgreSQL
            //string query = @"
            //    SELECT fm.id,
            //           f.id AS formid,
            //           f.name AS formname,
            //           m.id AS moduleid,
            //           m.name AS modulename
            //    FROM formmodule fm
            //    INNER JOIN form f ON fm.formid = f.id
            //    INNER JOIN ""Module"" m ON fm.moduleid = m.id
            //    WHERE fm.isdeleted = false;";

            //MySql
            string query = @"
                    SELECT fm.Id,
                           f.Id AS FormId,
                           f.Name AS FormName,
                           m.Id AS ModuleId,
                           m.Name AS ModuleName
                    FROM FormModule fm
                    INNER JOIN Form f ON fm.FormId = f.Id
                    INNER JOIN Module m ON fm.ModuleId = m.Id
                    WHERE fm.IsDeleted = 0;";


            return await _context.QueryAsync<FormModuleDto>(query);
            //return await _context.Set<FormModule>().ToListAsync();
        }
        //Metodo para traer por id SQL
        public async Task<FormModule?> GetByIdAsync(int id)
        {
            try
            {

                //SqlServer
                //string query = @"
                //             SELECT fm.Id,
                //           f.Id AS FormId,
                //           f.Name AS FormName,
                //           m.Id AS ModuleId,
                //           m.Name AS ModuleName
                //            FROM FormModule fm
                //            INNER JOIN Form f ON fm.FormId = f.Id
                //            INNER JOIN Module m ON fm.ModuleId = m.Id
                //            WHERE fm.Id = @Id;";

                //Postgrestsql
                //string query = @"
                //        SELECT fm.id,
                //               f.id AS formid,
                //               f.name AS formname,
                //               m.id AS moduleid,
                //               m.name AS modulename
                //        FROM formmodule fm
                //        INNER JOIN form f ON fm.formid = f.id
                //        INNER JOIN ""Module"" m ON fm.moduleid = m.id
                //        WHERE fm.Id = @Id AND fm.isdeleted = false;
                //         ";

                //MySql
                string query = @"
                            SELECT fm.Id,
                                   f.Id AS FormId,
                                   f.Name AS FormName,
                                   m.Id AS ModuleId,
                                   m.Name AS ModuleName
                            FROM FormModule fm
                            INNER JOIN Form f ON fm.FormId = f.Id
                            INNER JOIN Module m ON fm.ModuleId = m.Id
                            WHERE fm.Id = @Id AND fm.IsDeleted = 0;";

                var parameters = new { Id = id };
                return await _context.QueryFirstOrDefaultAsync<FormModule>(query, parameters);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error al traer el FormModule por id {id}");
                throw;
            }

        }

        public async Task<FormModuleDto?> GetByIdDtoAsync(int id)
        {
            try
            {
                //SqlServer
                //string query = @"
                //             SELECT fm.Id,
                //           f.Id AS FormId,
                //           f.Name AS FormName,
                //           m.Id AS ModuleId,
                //           m.Name AS ModuleName
                //            FROM FormModule fm
                //            INNER JOIN Form f ON fm.FormId = f.Id
                //            INNER JOIN Module m ON fm.ModuleId = m.Id
                //            WHERE fm.Id = @Id;";

                //Postgrestsql
                //string query = @"
                //        SELECT fm.id,
                //               f.id AS formid,
                //               f.name AS formname,
                //               m.id AS moduleid,
                //               m.name AS modulename
                //        FROM formmodule fm
                //        INNER JOIN form f ON fm.formid = f.id
                //        INNER JOIN ""Module"" m ON fm.moduleid = m.id
                //        WHERE fm.Id = @Id AND fm.isdeleted = false;
                //         ";

                //MySql
                string query = @"
                            SELECT fm.Id,
                                   f.Id AS FormId,
                                   f.Name AS FormName,
                                   m.Id AS ModuleId,
                                   m.Name AS ModuleName
                            FROM FormModule fm
                            INNER JOIN Form f ON fm.FormId = f.Id
                            INNER JOIN Module m ON fm.ModuleId = m.Id
                            WHERE fm.Id = @Id AND fm.IsDeleted = 0;";

                var parameters = new { Id = id };
                return await _context.QueryFirstOrDefaultAsync<FormModuleDto>(query, parameters);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error al traer el FormModule por id {id}");
                throw;
            }

        }
        //Metodo para crear SQL
        public async Task<FormModule> CreateAsync(FormModule formModule)
        {
            try
            {
                //SqlServer
                //const string query = @"INSERT INTO FormModule (FormId, IsDeleted, ModuleId)  OUTPUT INSERTED.Id 
                //                     values(@FormId, @IsDeleted, @ModuleId);
                //                      ";

                //Postgrest
                //const string query = @"
                //                    INSERT INTO formmodule (formid, isdeleted, moduleid) 
                //                    VALUES (@FormId, @IsDeleted, @ModuleId)
                //                    RETURNING id;";

                //MySql
                const string query = @"
                                    INSERT INTO FormModule (FormId, IsDeleted, ModuleId)
                                    VALUES (@FormId, @IsDeleted, @ModuleId);
                                    SELECT LAST_INSERT_ID();";



                var parameters = new { 
                    FormId = formModule.FormId, 
                    ModuleId = formModule.ModuleId,
                    IsDeleted = false
                };
                formModule.Id = await _context.ExecuteScalarAsync<int>(query, parameters);
                return formModule;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"no se pudo agregar persona {formModule}");
                throw;
            }
        }

        //Metodo para actualizar SQL
        public async Task<bool> UpdateAsync(FormModule formModule)
        {
            if (formModule is null)
            {
                throw new ArgumentNullException(nameof(FormModule), "El formModule no puede ser nulo.");
            }
            try
            {
                //SqlServer
                //const string query = @"UPDATE FormModule " +
                //                      " SET FormId = @FormId, ModuleId = @ModuleId " +
                //                      "WHERE Id = @Id";

                //Postgres
                //const string query = @"
                //        UPDATE formmodule 
                //        SET formid = @FormId, moduleid = @ModuleId 
                //        WHERE id = @Id;";

                //MySql
                const string query = @"
                        UPDATE FormModule 
                        SET FormId = @FormId, ModuleId = @ModuleId 
                        WHERE Id = @Id;";


                var parameters = new { 
                    formModule.Id,
                    formModule.FormId, 
                    formModule.ModuleId 
                };
                int rowsAffected = await _context.ExecuteAsync(query, parameters);

                return rowsAffected > 0;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"No se pudo actualizar {formModule}");
                throw;

            }

        }
        //Metodo para Borrar Logico SQL
        public async Task<bool> DeleteLogicAsync(int id)
        {
            try
            {
                //SqlServer
                //string query = @"
                //    UPDATE FormModule 
                //    SET IsDeleted = 1
                //    WHERE Id = @Id";

                //Postgres
                //string query = @"
                //        UPDATE formmodule 
                //        SET isdeleted = true 
                //        WHERE id = @Id;";

                //MySql
                string query = @"
                        UPDATE FormModule 
                        SET IsDeleted = 1 
                        WHERE Id = @Id;";


                int rowsAffected = await _context.ExecuteAsync(query,new { Id = id });

                return rowsAffected > 0;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al borra logicamente con ID {FormModuleId}");
                throw;
            }
        }
        //Metodo para borrar persistente SQL
        public async Task<bool> DeletePersistenceAsync(int id)
        {
            try
            {
                //SqlServer
                //string query = @"
                //    DELETE FROM FormModule 
                //    WHERE Id = @Id";

                //Postgres
                //string query = @"
                //        DELETE FROM formmodule 
                //        WHERE id = @Id;";

                //MySql
                string query = @"
                        DELETE FROM FormModule 
                        WHERE Id = @Id;";


                int rowsAffected = await _context.ExecuteAsync(query, new { Id = id });

                return rowsAffected > 0; // Devuelve true si se eliminó al menos una fila
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al eliminar permanentemente FormModule con ID {FormModuleId}", id);
                throw;
            }
        }



        //Metodo para traer todo LinQ
        public async Task<IEnumerable<FormModule>> GetAllLinQAsync()
        {
            /* return await _context.Set<FormModule>().ToListAsync();*/

            /*return await _context.Set<FormModule>()

                .Include(form => form.Form)
                .Include(form => form.Module)
                .ToListAsync();*/


            /*  SELECT t1.columna1, t1.columna2, t2.columna3
              FROM tabla1 t1
              INNER JOIN tabla2 t2 ON t1.columna_comun = t2.columna_comun;*/

            //return (IEnumerable<Rol>)await _context.QueryAsync<IEnumerable<Rol>>(query);/
            try
            {
                return await _context.Set<FormModule>()
                        .Include(fm => fm.Form)   // Carga la relación con Form
                        .Include(fm => fm.Module) // Carga la relación con Module
                        .Where(fm => !fm.IsDeleted)
                        .ToListAsync(); // Devuelve todos los registros
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error al traer el FormModule ");
                throw;
            }


        }

        //Metodo para traer por id LinQ
        public async Task<FormModule?> GetByIdLinQAsync(int id)
        {
            try
            {
                return await _context.Set<FormModule>()
                    .Include(fm => fm.Form)
                    .Include(fm => fm.Module)
                    .FirstOrDefaultAsync(fm => fm.Id == id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error al traer el FormModule por id {id}");
                throw;
            }

        }

        //Metodo para crear LinQ
        public async Task<FormModule> CreateLinQAsync(FormModule formModule)
        {
            try
            {
                formModule.IsDeleted =false;
                await _context.Set<FormModule>().AddAsync(formModule);
                await _context.SaveChangesAsync();
                return formModule;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"no se pudo agregar persona {formModule}");
                throw;
            }

        }

        //Metodo para actualizar LinQ
        public async Task<bool> UpdateLinQAsync(FormModule formModule)
        {
            if (formModule is null)
            {
                throw new ArgumentNullException(nameof(FormModule), "El formModule no puede ser nulo.");
            }
            try
            {
                _context.Set<FormModule>().Update(formModule);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"No se pudo actualizar {formModule}");
                throw;

            }

        }


        //Metodo para boraar logico LinQ
        public async Task<bool> DeleteLinQAsync(int id)
        {
            try
            {
                var entity = await _context.Set<FormModule>().FindAsync(id);
                if (entity == null) return false;

                // Marcar como eliminado
                entity.IsDeleted = true;
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogInformation($"Error al realizar delete lógico con LINQ: {ex.Message}");
                return false;
            }

        }

        //Metodo para borrar persistente LinQ

        public async Task<bool> DeletePersistenceLinQAsync(int id)
        {
            try
            {
                var formModule = await _context.Set<FormModule>().FindAsync(id);

                if (formModule == null)
                    return false;

                _context.Set<FormModule>().Remove(formModule);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al eliminar usuario con el ID {UserId}", id);
                return false;
            }
        }


    }
}
