using Data.Interfaces.Implements.Security;
using Data.Repository;
using Entity.Domain.Models.Implements.Security;
using Entity.Infrastructure.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Service.Security
{
    public class FormRepository : DataGeneric<Form>, IFormRepository
    {
        public FormRepository(ApplicationDbContext context) : base(context)
        {
        }
    }
}
