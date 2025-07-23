using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Data.Interfaces.Implements.Producers;
using Data.Repository;
using Entity.Domain.Models.Implements.Producers;
using Entity.Infrastructure.Context;

namespace Data.Service.Producers
{
    public class ProducerRepository : DataGeneric<Producer>, IProducerRepository
    {
        public ProducerRepository(ApplicationDbContext context) : base(context)
        {
        }
    }
}
