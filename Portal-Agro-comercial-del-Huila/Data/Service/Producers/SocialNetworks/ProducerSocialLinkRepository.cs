using Data.Interfaces.Implements.Producers.SocialNetworks;
using Data.Repository;
using Entity.Domain.Models.Implements.Producers;
using Entity.Infrastructure.Context;

namespace Data.Service.Producers.SocialNetworks
{
    public class ProducerSocialLinkRepository : DataGeneric<ProducerSocialLink>, IProducerSocialLinkRepository
    {
        public ProducerSocialLinkRepository(ApplicationDbContext context) : base(context)
        {
        }
    }
}
