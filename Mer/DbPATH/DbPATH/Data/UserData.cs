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
    public class UserData
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<UserData> _logger;

        public UserData(ApplicationDbContext context, ILogger<UserData> logger)
        {
            _context = context;
            _logger = logger;
        }

        //Metodo para traer todo SQL
        public async Task<IEnumerable<User>> GetAllAsync()
        {

            try
            {
                //SqlServer
                //string query = @"SELECT * FROM [User] WHERE IsDeleted = 0;";

                // PostgreSQL
                //string query = @"SELECT * FROM ""User"" WHERE IsDeleted = FALSE;";

                //MySql
                string query = @"SELECT * FROM User WHERE IsDeleted = 0;";


                return (IEnumerable<User>)await _context.QueryAsync<User>(query);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener Los useres {User}");
                throw; // Relanza la excepcion  para q sea manejada por las capas superiores
            }


        }


        //Metodo para traer por id SQL
        public async Task<User?> GetByIdAsync(int id)
        {
            try
            {
                //SqlServer
                //string query = @"SELECT * FROM [User] WHERE Id = @Id AND IsDeleted = 0;";

                // PostgreSQL
                //string query = @"SELECT * FROM ""User"" WHERE Id = @Id AND IsDeleted = FALSE;";

                //MySql
                string query = @"SELECT * FROM User WHERE Id = @Id AND IsDeleted = 0;";

                return await _context.QueryFirstOrDefaultAsync<User>(query, new { Id = id });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener el user con ID {UserId}", id);
                throw; // Relanza la excepcion  para q sea manejada por las capas superiores
            }
        }


        //Metodo para crear SQL
        public async Task<User> CreateAsync(User user)
        {
            try
            {
                //SqlServer
                //string query = @"
                //    INSERT INTO [User] (UserName, Active, Password,IsDeleted, PersonId) 
                //    OUTPUT INSERTED.Id 
                //    VALUES (@UserName, @Active, @Password, @IsDeleted,@PersonId);";

                // PostgreSQL
                //string query = @"
                //            INSERT INTO ""User"" (UserName, Active, Password, IsDeleted, PersonId) 
                //            VALUES (@UserName, @Active, @Password, @IsDeleted, @PersonId)
                //            RETURNING Id;";

                //MySql
                string query = @"
                        INSERT INTO User (UserName, Active, Password, IsDeleted, PersonId) 
                        VALUES (@UserName, @Active, @Password, @IsDeleted, @PersonId);
                        SELECT LAST_INSERT_ID();";


                user.Id = await _context.QueryFirstOrDefaultAsync<int>(query, new
                {
                    user.UserName,
                    Active = true,
                    Password = "0000s",
                    IsDeleted = false,
                    user.PersonId
                });

                return user;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al crear el user.");
                throw;
            }
        }

        //Metodo para actualizar SQL

        public async Task<bool> UpdateAsync(User user)
        {
            try
            {
                //SqlServer
                //string query = @"
                //    UPDATE [User] 
                //    SET UserName = @UserName, 
                //        Active = @Active, 
                //        PersonId = @PersonId
                //    WHERE Id = @Id";

                // PostgreSQL
                //string query = @"
                //        UPDATE ""User"" 
                //        SET UserName = @UserName, 
                //            Active = @Active, 
                //            PersonId = @PersonId
                //        WHERE Id = @Id;";

                //MySql
                string query = @"
                        UPDATE User 
                        SET UserName = @UserName, 
                            Active = @Active, 
                            PersonId = @PersonId
                        WHERE Id = @Id;";

                var parameters = new
                {
                    user.Id,
                    user.UserName,
                    user.Active,
                    user.PersonId
                };

                int rowsAffected = await _context.ExecuteAsync(query, parameters);
                return rowsAffected > 0;


            }
            catch (Exception ex)
            {
                _logger.LogError($"Error al actualizar el user: {ex.Message}");
                return false;
            }
        }


        //Metodo para borrar logico SQL
        public async Task<bool> DeleteLogicAsync(int id)
        {
            try
            {
                //SqlServer
                //string query = @"UPDATE [User]
                //               SET IsDeleted = 1
                //               WHERE Id=@Id";

                // PostgreSQL
                //string query = @"UPDATE ""User"" SET IsDeleted = TRUE WHERE Id = @Id;";

                //MySql
                string query = @"UPDATE User SET IsDeleted = 1 WHERE Id = @Id;";


                int rowsAffected = await _context.ExecuteAsync(query, new { id });
                return rowsAffected > 0;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error al eliminar logicamente user: {ex.Message}");
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
                //               DELETE FROM [User]
                //               WHERE Id = @Id";

                // PostgreSQL
                //string query = @"DELETE FROM ""User"" WHERE Id = @Id;";

                //MySql
                string query = @"DELETE FROM User WHERE Id = @Id;";

                int rowsAffected = await _context.ExecuteAsync(query, new { Id = id });

                return rowsAffected > 0;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error al eliminar user: {ex.Message}");
                return false;
            }
        }





        //Metodo para obtener Todo LinQ
        public async Task<IEnumerable<User>> GetAllLinQAsync()

        {
            try
            {
                return await _context.Set<User>()
                    .Where(u => !u.IsDeleted)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener los useres");
                throw;
            }

        }


        //Metodo para obtener por id LinQ

        public async Task<User?> GetByIdLinQAsync(int id)
        {
            try
            {
                return await _context.Set<User>().FindAsync(id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener la usera con ID {UserId}", id);
                throw; // Relanza la excepcion  para q sea manejada por las capas superiores
            }
        }

        //Metodo para crear LinQ
        public async Task<User> CreateLinQAsync(User user)
        {
            try
            {
                await _context.Set<User>().AddAsync(user);
                await _context.SaveChangesAsync();
                return user;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error al crear la usera: {ex.Message}");
                throw;
            }
        }

        //Metodo para actualizar LinQ
        public async Task<bool> UpdateLinQAsync(User user)
        {
            try
            {
                _context.Set<User>().Update(user);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error al actualizar la usera: {ex.Message}");
                throw;
            }
        }


        //Metodo para borrar logico LinQ
        public async Task<bool> DeleteLogicLinQAsync(int id)
        {
            try
            {
                var user = await GetByIdLinQAsync(id);
                if (user == null)
                {
                    return false;
                }

                // Marcar como eliminado lógicamente
                user.IsDeleted = true; // de
                await _context.SaveChangesAsync();

                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al eliminar lógicamente el user con ID {id}", id);
                throw;
            }
        }

        //Metodo para Borrar persistente LinQ
        public async Task<bool> DeletePersistenceLinQAsync(int id)
        {
            try
            {
                var user = await GetByIdLinQAsync(id);
                if (user == null)
                {
                    return false;
                }
                _context.Set<User>().Remove(user);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error al eliminar la usera: {ex.Message}");
                throw;
            }
        }



    }
}
