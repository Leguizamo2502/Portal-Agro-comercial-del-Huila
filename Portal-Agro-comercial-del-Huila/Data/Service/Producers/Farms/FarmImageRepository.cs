using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Data.Interfaces.Implements.Producers;
using Data.Repository;
using Entity.Domain.Models.Implements.Producers;
using Entity.Infrastructure.Context;

namespace Data.Service.Producers.Farms
{
    public class FarmImageRepository : DataGeneric<FarmImage>, IFarmImageRepository
    {
        public FarmImageRepository(ApplicationDbContext context) : base(context)
        {
        }
    }
}
