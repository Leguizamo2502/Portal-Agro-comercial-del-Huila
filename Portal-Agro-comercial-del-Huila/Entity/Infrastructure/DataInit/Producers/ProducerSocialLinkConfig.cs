using Entity.Domain.Models.Implements.Producers;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Entity.Infrastructure.DataInit.Producers
{
    public class ProducerSocialLinkConfig : IEntityTypeConfiguration<ProducerSocialLink>
    {
        public void Configure(EntityTypeBuilder<ProducerSocialLink> b)
        {
            b.ToTable("ProducerSocialLinks");
            b.HasKey(x => x.Id);

            b.Property(x => x.Url)
             .IsRequired()
             .HasMaxLength(512);

            b.HasOne(x => x.Producer)
             .WithMany(p => p.SocialLinks)
             .HasForeignKey(x => x.ProducerId)
             .OnDelete(DeleteBehavior.Cascade);

            // Evita duplicar la misma red para el mismo productor
            b.HasIndex(x => new { x.ProducerId, x.Network }).IsUnique();
        }
    }
}
