using DataCore.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DataProvider.EntitiesTypeConfiguration
{
    public class PositionConfiguration : IEntityTypeConfiguration<Position>
    {
        public void Configure(EntityTypeBuilder<Position> entity)
        {
            //entity base
            entity.HasKey(x => x.Id).HasName("pk_positions");
            entity.HasIndex(x => x.Id);
            entity.ToTable("positions");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("id");
            entity.Property(e => e.CreatedOn)
                .HasColumnType(DbConfig.DbTimeStampType)
                .HasColumnName("created_on");
            entity.Property(e => e.UpdatedOn)
                .HasColumnType(DbConfig.DbTimeStampType)
                .HasColumnName("updated_on");
            entity.Property(e => e.CreatedByUser).HasMaxLength(100).HasColumnName("created_by_user_login");
            entity.Property(e => e.UpdatedByUser).HasMaxLength(100).HasColumnName("updated_by_user_login");

            //entity props
            entity.Property(e => e.Name)
                .HasMaxLength(500)
                .HasColumnName("name");

            //owner
            entity.Property(e => e.EntityOwnerId)
                .ValueGeneratedNever()
                .HasColumnName("entity_owner_id");
            entity.HasIndex(x => x.EntityOwnerId);
            entity.HasOne(x => x.EntityOwner).WithMany(x => x.Positions)
                .HasForeignKey(x => x.EntityOwnerId)
                .HasConstraintName("fk_positions_entity_owner_id")
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
