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
    public class PersonData
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<PersonData> _logger;

        public PersonData(ApplicationDbContext context, ILogger<PersonData> logger)
        {
            _context = context;
            _logger = logger;
        }

        //Metodo para traer todo SQL
        public async Task<IEnumerable<Person>> GetAllAsync()
        {

            try
            {
                //SQlServer
                //string query = @"SELECT * FROM Person WHERE IsDeleted = 0;";

                // PostgreSQL
                //string query = @"SELECT * FROM person WHERE isdeleted = FALSE;";

                //MySql
                string query = @"SELECT * FROM Person WHERE IsDeleted = 0;";

                return await _context.QueryAsync<Person>(query);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener Los persones {Person}");
                throw; // Relanza la excepcion  para q sea manejada por las capas superiores
            }


        }


        //Metodo para traer por id SQL
        public async Task<Person?> GetByIdAsync(int id)
        {
            try
            {
                //SqlServer
                //string query = @"SELECT * FROM Person WHERE Id = @Id AND IsDeleted = 0;";

                // PostgreSQL
                //string query = @"SELECT * FROM person WHERE id = @Id AND isdeleted = FALSE;";

                //MySql
                string query = @"SELECT * FROM Person WHERE id = @Id AND IsDeleted = 0;";

                return await _context.QueryFirstOrDefaultAsync<Person>(query, new { Id = id });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener el person con ID {PersonId}", id);
                throw; // Relanza la excepcion  para q sea manejada por las capas superiores
            }
        }


        //Metodo para crear SQL
        public async Task<Person> CreateAsync(Person person)
        {
            try
            {
                //SqlServer
                //string query = @"
                //    INSERT INTO Person (FirstName, LastName, Email, PhoneNumber, Address, IsDeleted) 
                //    OUTPUT INSERTED.Id 
                //    VALUES (@FirstName, @LastName, @Email, @PhoneNumber, @Address, @IsDeleted);";

                // PostgreSQL
                //string query = @"
                //        INSERT INTO person (firstname, lastname, email, phonenumber, address, isdeleted)
                //        VALUES (@FirstName, @LastName, @Email, @PhoneNumber, @Address, @IsDeleted)
                //        RETURNING id;";

                //MySql
                string query = @"
                    INSERT INTO Person (FirstName, LastName, Email, PhoneNumber, Address, IsDeleted) 
                    VALUES (@FirstName, @LastName, @Email, @PhoneNumber, @Address, @IsDeleted);
                    SELECT LAST_INSERT_ID();
                    ";

                person.Id = await _context.QueryFirstOrDefaultAsync<int>(query, new
                {
                    person.FirstName,
                    person.LastName,
                    person.Email,
                    person.PhoneNumber,
                    person.Address,
                    IsDeleted = false, // Se establece siempre en false al crear

                });

                return person;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al crear la persona.");
                throw;
            }
        }

        //Metodo para actualizar SQL

        public async Task<bool> UpdateAsync(Person person)
        {
            try
            {
                //SqlServer
                //string query = @"
                //    UPDATE Person 
                //    SET FirstName = @FirstName, 
                //        LastName = @LastName, 
                //        Email = @Email, 
                //        PhoneNumber = @PhoneNumber, 
                //        Address = @Address 
                //    WHERE Id = @Id;";

                // PostgreSQL
                //string query = @"
                //        UPDATE person
                //        SET firstname = @FirstName,
                //            lastname = @LastName,
                //            email = @Email,
                //            phonenumber = @PhoneNumber,
                //            address = @Address
                //        WHERE id = @Id;";

                //MySql
                string query = @"
                    UPDATE Person 
                    SET FirstName = @FirstName, 
                        LastName = @LastName, 
                        Email = @Email, 
                        PhoneNumber = @PhoneNumber, 
                        Address = @Address 
                    WHERE Id = @Id;";

                int rowsAffected = await _context.ExecuteAsync(query, new
                {
                    person.Id,
                    person.FirstName,
                    person.LastName,
                    person.Email,
                    person.PhoneNumber,
                    person.Address,
                });

                return rowsAffected > 0;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al actualizar la persona con ID {PersonId}", person.Id);
                throw;
            }
        }


        //Metodo para borrar logico SQL
        public async Task<bool> DeleteLogicAsync(int id)
        {
            try
            {
                //SqlServer
                //string query = @"UPDATE Person
                //               SET IsDeleted = 1
                //               WHERE Id=@Id";

                // PostgreSQL
                //string query = @"UPDATE person SET isdeleted = TRUE WHERE id = @Id;";

                //MySql
                string query = @"UPDATE Person SET IsDeleted = 1 WHERE Id = @Id;";

                int rowsAffected = await _context.ExecuteAsync(query, new { Id=id });
                return rowsAffected > 0;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error al eliminar logicamente person: {ex.Message}");
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
                //               DELETE FROM Person
                //               WHERE Id = @Id";

                // PostgreSQL
                //string query = @"DELETE FROM person WHERE id = @Id;";

                //MySql
                string query = @"DELETE FROM Person WHERE Id = @Id;";

                int rowsAffected = await _context.ExecuteAsync(query, new { Id = id });

                return rowsAffected > 0;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error al eliminar person: {ex.Message}");
                return false;
            }
        }





        //Metodo para obtener Todo LinQ
        public async Task<IEnumerable<Person>> GetAllLinQAsync()

        {

            return await _context.Set<Person>()
                .Where(p=>!p.IsDeleted)
                .ToListAsync();


        }


        //Metodo para obtener por id LinQ

        public async Task<Person?> GetByIdLinQAsync(int id)
        {
            try
            {
                return await _context.Set<Person>().FindAsync(id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener la persona con ID {PersonId}", id);
                throw; // Relanza la excepcion  para q sea manejada por las capas superiores
            }
        }

        //Metodo para crear LinQ
        public async Task<Person> CreateLinQAsync(Person person)
        {
            try
            {
                await _context.Set<Person>().AddAsync(person);
                await _context.SaveChangesAsync();
                return person;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error al crear la persona: {ex.Message}");
                throw;
            }
        }

        //Metodo para actualizar LinQ
        public async Task<bool> UpdateLinQAsync(Person person)
        {
            try
            {
                _context.Set<Person>().Update(person);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error al actualizar la persona: {ex.Message}");
                throw;
            }
        }


        //Metodo para borrar logico LinQ
        public async Task<bool> DeleteLogicLinQAsync(int id)
        {
            try
            {
                var person = await GetByIdLinQAsync(id);
                if (person == null)
                {
                    return false;
                }

                // Marcar como eliminado lógicamente
                person.IsDeleted = true; 
                await _context.SaveChangesAsync();

                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al eliminar lógicamente el person con ID {id}", id);
                throw;
            }
        }

        //Metodo para Borrar persistente LinQ
        public async Task<bool> DeletePersistenceLinQAsync(int id)
        {
            try
            {
                var person = await GetByIdLinQAsync(id);
                if (person == null)
                {
                    return false;
                }
                _context.Set<Person>().Remove(person);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error al eliminar la persona: {ex.Message}");
                throw;
            }
        }
    }
}
