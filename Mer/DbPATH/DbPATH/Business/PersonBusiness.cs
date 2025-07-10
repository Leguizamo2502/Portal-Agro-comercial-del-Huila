using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Data;
using Entity.DTOs;
using Entity.Model;
using Microsoft.Extensions.Logging;
using Utilities.Exceptions;

namespace Business
{
    public  class PersonBusiness
    {
        private readonly PersonData _personData;
        private readonly ILogger<PersonBusiness> _logger;

        public PersonBusiness(PersonData personData, ILogger<PersonBusiness> logger)
        {
            _personData = personData;
            _logger = logger;
        }

        // Método para obtener todas las personas y Dtos
        public async Task<IEnumerable<PersonDto>> GetAllPersonsAsync()
        {
            try
            {
                var persons = await _personData.GetAllAsync();
                var personsDTO = MapToDTOList(persons);
                return personsDTO;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener todas las personas");
                throw new ExternalServiceException("Base de datos", "Error al recuperar la lista de personas", ex);
            }
        }

        // Método para obtener una persona por ID como DTO
        public async Task<PersonDto> GetPersonByIdAsync(int id)
        {
            if (id <= 0)
            {
                _logger.LogWarning("Se intento obtener una persona con un ID inválido: {PersonId}", id);
                throw new Utilities.Exceptions.ValidationException("ID de persona deb ser mayor a 0");
            }
            try
            {
                var person = await _personData.GetByIdAsync(id);
                if (person == null)
                {
                    _logger.LogWarning("No se encontró la persona con ID {PersonId}", id);
                    throw new EntityNotFoundException("Persona",id);
                }
                return MapToDTO(person);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener la persona con ID {PersonId}", id);
                throw new ExternalServiceException("Base de datos", "Error al recuperar la persona", ex);
            }
        }


        // Método para crear una persona desde DTO
        public async Task<PersonDto> CreatePersonAsync(PersonDto personDto)
        {
           
            try
            {
                ValidatePerson(personDto);
                var person = MapToEntity(personDto);
                var createdPerson = await _personData.CreateAsync(person);
                return MapToDTO(person);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al crear la persona {personaName}",personDto?.FirstName ?? "null");
                throw new ExternalServiceException("Base de datos", "Error al crear la persona", ex);
            }
        }
        // Metodo para actualizar
        public async Task<PersonDto> UpdatePersonAsync(PersonDto personDto)
        {
            try
            {
                if (personDto == null || personDto.Id <= 0)
                {
                    throw new ValidationException("id", "El ID del person debe ser mayor que cero y no nulo");
                }

                var existingPerson = await _personData.GetByIdAsync(personDto.Id);
                if (existingPerson == null)
                {
                    throw new EntityNotFoundException("Person", personDto.Id);
                }

                var updatedPerson = MapToEntity(personDto);
                bool success = await _personData.UpdateAsync(updatedPerson);

                if (!success)
                {
                    throw new Exception("No se pudo actualizar el person.");
                }

                return MapToDTO(updatedPerson);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error al actualizar el person con ID {personDto?.Id}");
                throw new ExternalServiceException("Base de datos", $"Error al actualizar el person con ID {personDto?.Id}", ex);
            }
        }

        //Metodo para borrar logicamente
        public async Task<bool> DeletePersonLogicalAsync(int id)
        {
            try
            {
                if (id <= 0)
                {
                    throw new ValidationException("id", "El ID del person debe ser mayor que cero");
                }

                var existingPerson = await _personData.GetByIdAsync(id);
                if (existingPerson == null)
                {
                    throw new EntityNotFoundException("Person", id);
                }

                return await _personData.DeleteLogicAsync(id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error al realizar la eliminación lógica del person con ID {id}");
                throw new ExternalServiceException("Base de datos", $"Error al realizar la eliminación lógica del person con ID {id}", ex);
            }
        }

        //Metodo para borrar persistente
        public async Task<bool> DeletePersonPersistenceAsync(int id)
        {
            try
            {
                if (id <= 0)
                {
                    throw new ValidationException("id", "El ID del person debe ser mayor que cero");
                }

                var existingPerson = await _personData.GetByIdAsync(id);
                if (existingPerson == null)
                {
                    throw new EntityNotFoundException("Person", id);
                }

                return await _personData.DeletePersistenceAsync(id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error al eliminar permanentemente el person con ID {id}");
                throw new ExternalServiceException("Base de datos", $"Error al eliminar el person con ID {id}", ex);
            }
        }

        //Metodo para validar el DTO de persona
        public void ValidatePerson(PersonDto PersonDto)
        {
            if(PersonDto == null)
            {
                throw new Utilities.Exceptions.ValidationException("Persona", "El objeto persona no puede ser nulo");
            }
            if(string.IsNullOrWhiteSpace(PersonDto.FirstName))
            {
                _logger.LogWarning("Se intento crear una persona con un nombre nulo o vacío");
                throw new Utilities.Exceptions.ValidationException("Persona", "El nombre de la persona no puede ser nulo o vacío");
            }
        }

        // Método para mapear de Person a PersonDTO
        private PersonDto MapToDTO(Person person)
        {
            return new PersonDto
            {
                Id = person.Id,
                FirstName = person.FirstName,
                LastName = person.LastName,
                Email = person.Email,
                PhoneNumber = person.PhoneNumber,
                Address = person.Address,
                IsDeleted = person.IsDeleted,
               
                

                
            };
        }


        // Método para mapear de PersonDTO a Person
        private Person MapToEntity(PersonDto personDto)
        {
            return new Person
            {
                Id = personDto.Id,
                FirstName = personDto.FirstName,
                LastName = personDto.LastName, // Si existe en la entidad
                Email = personDto.Email,
                PhoneNumber = personDto.PhoneNumber,
                Address = personDto.Address,
                IsDeleted = personDto.IsDeleted,
                

            };
        }

        // Método para mapear una lista de Person a una lista de PersonDTO
        private IEnumerable<PersonDto> MapToDTOList(IEnumerable<Person> persones)
        {
            var personesDTO = new List<PersonDto>();
            foreach (var person in persones)
            {
                personesDTO.Add(MapToDTO(person));
            }
            return personesDTO;
        }


    }
}
