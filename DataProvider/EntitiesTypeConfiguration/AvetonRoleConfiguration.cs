using DataCore.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DataProvider.EntitiesTypeConfiguration
{
    public class AvetonRoleConfiguration : IEntityTypeConfiguration<AvetonRole>
    {
        public void Configure(EntityTypeBuilder<AvetonRole> entity)
        {
            //entity base
            entity.HasKey(x => x.Id).HasName("pk_aveton_roles");
            entity.HasIndex(x => x.Id);
            entity.ToTable("aveton_roles");

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


            entity.Property(e => e.Name)
                .IsRequired()
                .HasMaxLength(200)
                .HasColumnName("name");
                
            entity.Property(e => e.IsDefault)
                .HasColumnName("is_default");
            entity.Property(e => e.IsSystemAdministrator)
                .HasDefaultValue(false)
                .HasColumnName("is_system_administrator");

            //owner
            entity.Property(e => e.EntityOwnerId)
                .ValueGeneratedNever()
                .HasColumnName("entity_owner_id");
            entity.HasIndex(x => x.EntityOwnerId);
            entity.HasOne(x => x.EntityOwner).WithMany(x => x.Roles)
                .HasForeignKey(x => x.EntityOwnerId)
                .HasConstraintName("fk_aveton_roles_entity_owner_id")
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
