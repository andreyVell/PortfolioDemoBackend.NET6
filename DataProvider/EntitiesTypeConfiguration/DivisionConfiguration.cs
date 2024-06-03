using DataCore.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DataProvider.EntitiesTypeConfiguration
{
    public class DivisionConfiguration : IEntityTypeConfiguration<Division>
    {
        public void Configure(EntityTypeBuilder<Division> entity)
        {
            //entity base
            entity.HasKey(x => x.Id).HasName("pk_divisions");
            entity.HasIndex(x => x.Id);
            entity.ToTable("divisions");

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
            entity.Property(e => e.ParentDivisionId).HasColumnName("parent_division_id");

            //entity props
            entity.Property(e => e.Name)
                .HasMaxLength(500)
                .HasColumnName("name");


            //navigation props
            entity.HasOne(e => e.ParentDivision).WithMany(p => p.ChildDivisions)
                .HasForeignKey(d => d.ParentDivisionId)
                .HasConstraintName("fk_divisions_parent_division_id")
                .OnDelete(DeleteBehavior.SetNull);

            //owner
            entity.Property(e => e.EntityOwnerId)
                .ValueGeneratedNever()
                .HasColumnName("entity_owner_id");
            entity.HasIndex(x => x.EntityOwnerId);
            entity.HasOne(x => x.EntityOwner).WithMany(x => x.Divisions)
                .HasForeignKey(x => x.EntityOwnerId)
                .HasConstraintName("fk_divisions_entity_owner_id")
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
