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
    public class RolUserData
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<RolUserData> _logger;

        public RolUserData(ApplicationDbContext context, ILogger<RolUserData> logger)
        {
            _context = context;
            _logger = logger;
        }

        // Listar todos los RolUser sql
        public async Task<IEnumerable<RolUserDto>> GetAllAsync()
        {
            try
            {
                //SqlServer
                //string query = @"
                //                SELECT 
                //                    RU.Id, 
                //                    RU.RolId, 
                //                    R.Name AS RolName, 
                //                    RU.UserId, 
                //                    U.UserName
                //                FROM RolUser RU
                //                INNER JOIN Rol R ON RU.RolId = R.Id
                //                INNER JOIN [User] U ON RU.UserId = U.Id
                //                WHERE RU.IsDeleted = 0;";

                // postgresql
                //string query = @"SELECT 
                //                ru.id, 
                //                ru.rolid, 
                //                r.name AS rolname, 
                //                ru.userid, 
                //                u.username
                //            FROM roluser ru
                //            INNER JOIN rol r ON ru.rolid = r.id
                //            INNER JOIN ""User"" u ON ru.userid = u.id
                //            WHERE ru.isdeleted = false;";

                // MySql
                string query = @"
                                SELECT 
                                    RU.Id, 
                                    RU.RolId, 
                                    R.Name AS RolName, 
                                    RU.UserId, 
                                    U.UserName
                                FROM RolUser RU
                                INNER JOIN Rol R ON RU.RolId = R.Id
                                INNER JOIN User U ON RU.UserId = U.Id
                                WHERE RU.IsDeleted = 0;";


                return await _context.QueryAsync<RolUserDto>(query);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener Los RolUser");
                throw; // Relanza la excepcion  para q sea manejada por las capas superiores
            }

        }

        // Obtener RolUser por Id
        public async Task<RolUser?> GetByIdAsync(int id)
        {
            try
            {
                //SqlServer
                //string query = @"
                //                SELECT 
                //                    RU.Id, 
                //                    RU.RolId, 
                //                    R.Name AS RolName, 
                //                    RU.UserId, 
                //                    U.UserName
                //                FROM RolUser RU
                //                INNER JOIN Rol R ON RU.RolId = R.Id
                //                INNER JOIN [User] U ON RU.UserId = U.Id
                //                WHERE RU.Id = @Id;";

                // postgresql
                //string query = @"SELECT 
                //                    ru.id, 
                //                    ru.rolid, 
                //                    r.name AS rolname, 
                //                    ru.userid, 
                //                    u.username
                //                FROM roluser ru
                //                INNER JOIN rol r ON ru.rolid = r.id
                //                INNER JOIN ""User"" u ON ru.userid = u.id
                //                WHERE ru.id = @id;";

                //MySql
                string query = @"
                                SELECT 
                                    RU.Id, 
                                    RU.RolId, 
                                    R.Name AS RolName, 
                                    RU.UserId, 
                                    U.UserName
                                FROM RolUser RU
                                INNER JOIN Rol R ON RU.RolId = R.Id
                                INNER JOIN User U ON RU.UserId = U.Id
                                WHERE RU.Id = @Id;";


                return await _context.QueryFirstOrDefaultAsync<RolUser>(query, new { Id = id });
                //return await _context.Set<RolUser>().FindAsync(id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener RolUser con el ID {RolUserId}", id);
                throw;
            }
        }

        public async Task<RolUserDto?> GetByIdDtoAsync(int id)
        {
            try
            {
                //string query = @"
                //                SELECT 
                //                    RU.Id, 
                //                    RU.RolId, 
                //                    R.Name AS RolName, 
                //                    RU.UserId, 
                //                    U.UserName
                //                FROM RolUser RU
                //                INNER JOIN Rol R ON RU.RolId = R.Id
                //                INNER JOIN [User] U ON RU.UserId = U.Id
                //                WHERE RU.Id = @Id;";

                // postgresql
                //string query = @"SELECT 
                //                    ru.id, 
                //                    ru.rolid, 
                //                    r.name AS rolname, 
                //                    ru.userid, 
                //                    u.username
                //                FROM roluser ru
                //                INNER JOIN rol r ON ru.rolid = r.id
                //                INNER JOIN ""User"" u ON ru.userid = u.id
                //                WHERE ru.id = @id;";

                //MySql
                string query = @"
                                SELECT 
                                    RU.Id, 
                                    RU.RolId, 
                                    R.Name AS RolName, 
                                    RU.UserId, 
                                    U.UserName
                                FROM RolUser RU
                                INNER JOIN Rol R ON RU.RolId = R.Id
                                INNER JOIN User U ON RU.UserId = U.Id
                                WHERE RU.Id = @Id;";


                return await _context.QueryFirstOrDefaultAsync<RolUserDto>(query, new { Id = id });
                //return await _context.Set<RolUser>().FindAsync(id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener RolUser con el ID {RolUserId}", id);
                throw;
            }
        }

        // Método para crear un RolUser con SQL
        public async Task<RolUser> CreateAsync(RolUser rolUser)
        {
            try
            {
                //SqlServer
                //string query = @"INSERT INTO RolUser" +
                //                " (RolId, UserId, IsDeleted)" +
                //                " OUTPUT INSERTED.Id" +
                //                " VALUES" +
                //                " (@RolId, @UserId, @IsDeleted);";

                // postgresql
                //string query = @"INSERT INTO roluser 
                //            (rolid, userid, isdeleted)
                //            VALUES 
                //            (@rolid, @userid, @IsDeleted)
                //            RETURNING id;";

                //MySql
                string query = @"INSERT INTO RolUser
                                (RolId, UserId, IsDeleted)
                                VALUES
                                (@RolId, @UserId, @IsDeleted);
                                SELECT LAST_INSERT_ID();";


                var Parameters = new
                {
                    rolUser.RolId,
                    rolUser.UserId,
                    IsDeleted = false,
                };

                rolUser.Id = await _context.ExecuteScalarAsync<int>(query, Parameters);
                return rolUser;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"no se pudo agregar persona {rolUser}");
                throw;
            }
        }

        // Método para actualizar RolUser SQL
        public async Task<bool> UpdateAsync(RolUser rolUser)
        {
            if (rolUser == null)
            {
                throw new ArgumentNullException(nameof(RolUser), "El formModule no puede ser nulo.");
            }

            try
            {
                //SqlServer
                //string query = @"UPDATE RolUser
                //                 SET
                //                 RolId = @RolUser,
                //                 UserId = @UserId
                //                 WHERE Id = @Id;";

                // postgresql
                //string query = @"UPDATE roluser
                //                    SET 
                //                    rolid = @rolid,
                //                    userid = @userid
                //                    WHERE id = @Id;";

                //MySql
                string query = @"UPDATE RolUser
                                 SET
                                 RolId = @RolUser,
                                 UserId = @UserId
                                 WHERE Id = @Id;";

                var Parameters = new
                {
                    rolUser.Id,
                    rolUser.RolId,
                     rolUser.UserId,

                };
                int rowsAffected = await _context.ExecuteAsync(query, Parameters);
                return rowsAffected > 0;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"No se pudo actualizar {rolUser}");
                throw;
            }
        }


        // Método para Eliminar un RolUser Logico SQL
        public async Task<bool> DeleteLogicAsync(int id)
        {
            try
            {
                //SqlServer
                //string query = @"UPDATE RolUser
                //                 SET
                //                 IsDeleted = 1
                //                 WHERE Id = @Id";

                // postgresql
                //string query = @"UPDATE roluser
                //                SET isdeleted = true
                //                WHERE id = @Id;";

                //MySql
                string query = @"UPDATE RolUser
                                 SET
                                 IsDeleted = 1
                                 WHERE Id = @Id";

                int rowsAffected = await _context.ExecuteAsync(query, new { Id = id });
                return rowsAffected > 0;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al borra logicamente con ID {RolUserId}");
                throw;
            }
        }

        // Método para eleminar un RolUser con persistencia SQL
        public async Task<bool> DeletePersistenceAsync(int id)
        {
            try
            {
                //SqlServer
                //string query = @"DELETE FROM RolUser
                //                 WHERE Id = @Id";

                // postgresql
                //string query = @"DELETE FROM roluser
                //                    WHERE id = @id;";

                //MySql
                string query = @"DELETE FROM RolUser
                                 WHERE Id = @Id";

                int rowsAffected = await _context.ExecuteAsync(query, new { Id = id });
                return rowsAffected > 0; // almenos una fila
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al eliminar permanentemente RolUser con ID {RolUserId}", id);
                throw;
            }
        }





        
        

        // Método para obtener todo de RolUser LINQ
        public async Task<IEnumerable<RolUser>> GetAllLinQAsync()
        {
            try
            {

                return await _context.Set<RolUser>()
                    .Include(ru => ru.Rol)
                    .Include(ru => ru.User)
                    .Where(ru => !ru.IsDeleted)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error al traer el RolUser ");
                throw;
            }
        }

        // Obtiene un RolUser específico por su Id LINQ.
        public async Task<RolUser?> GetByIdLinQAsync(int id)
        {
            try
            {
                return await _context.Set<RolUser>()
                    .Include(ru => ru.Rol)
                    .Include(ru => ru.User)
                    .FirstOrDefaultAsync(ru => ru.Id == id);

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener usuario con el ID {UserId}", id);
                throw;
            }
        }

        // Crear RolUser LINQ
        public async Task<RolUser> CreateLinQAsync(RolUser rolUser)
        {
            try
            {
                rolUser.IsDeleted = false;
                await _context.Set<RolUser>().AddAsync(rolUser);
                await _context.SaveChangesAsync();
                return rolUser;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al crear RolUser");
                throw;
            }
        }
        // Actualizar RolUser
        public async Task<bool> UpdateLinQAsync(RolUser rolUser)
        {
            try
            {
                _context.Set<RolUser>().Update(rolUser);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al actualizar RolUser");
                return false;
            }
        }

        // Eliminar RolUser Logico LINQ
        public async Task<bool> DeleteLogicLinQAsinc(int id)
        {
            try
            {
                var user = await GetByIdLinQAsync(id);
                if (user == null)
                    return false;

                user.IsDeleted = true;
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al eliminar lógicamente el user con ID {id}", id);
                throw;
            }
        }

        // Eliminar RolUser persistente LINQ
        public async Task<bool> DeletePersistenceLinQAsync(int id)
        {
            try
            {
                var rolUser = await _context.Set<RolUser>().FindAsync(id);

                if (rolUser == null)
                    return false;

                _context.Set<RolUser>().Remove(rolUser);
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