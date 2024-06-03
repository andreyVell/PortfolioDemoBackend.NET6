using DataCore.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DataProvider.EntitiesTypeConfiguration
{
    public class SystemOwnerConfiguration : IEntityTypeConfiguration<SystemOwner>
    {
        public void Configure(EntityTypeBuilder<SystemOwner> entity)
        {
            entity.HasKey(x => x.Id).HasName("pk_system_owners");
            entity.HasIndex(x => x.Id);
            entity.ToTable("system_owners");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("id");            
            entity.Property(e => e.Name).HasMaxLength(100).HasColumnName("name");
        }
    }
}
