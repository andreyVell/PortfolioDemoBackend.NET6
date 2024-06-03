using DataCore.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DataProvider.EntitiesTypeConfiguration
{
    public class AvetonRoleAccessConfiguration : IEntityTypeConfiguration<AvetonRoleAccess>
    {
        public void Configure(EntityTypeBuilder<AvetonRoleAccess> entity)
        {
            //entity base
            entity.HasKey(x => x.Id).HasName("pk_aveton_role_accesses");
            entity.HasIndex(x => x.Id);
            entity.ToTable("aveton_role_accesses");

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


            entity.Property(e => e.EntityName)
                .IsRequired()
                .HasMaxLength(100)
                .HasColumnName("entity_name");
            entity.Property(e => e.EntityAction)
                .IsRequired()
                .HasColumnName("entity_action");
            entity.Property(e => e.IsAllowed)
                .IsRequired()
                .HasColumnName("is_allowed");
            entity.Property(e => e.RoleId)
                .IsRequired()
                .HasColumnName("role_id");


            entity.HasOne(d => d.Role).WithMany(p => p.Accesses)
                .HasForeignKey(d => d.RoleId)
                .HasConstraintName("fk_aveton_role_accesses_role_id")
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasIndex(x => x.RoleId);
            entity.HasIndex(x => x.EntityAction);
            entity.HasIndex(x => x.EntityName);
            entity.HasIndex(x => x.IsAllowed);

            //owner
            entity.Property(e => e.EntityOwnerId)
                .ValueGeneratedNever()
                .HasColumnName("entity_owner_id");
            entity.HasIndex(x => x.EntityOwnerId);
            entity.HasOne(x => x.EntityOwner).WithMany(x => x.Accesses)
                .HasForeignKey(x => x.EntityOwnerId)
                .HasConstraintName("fk_aveton_role_accesses_entity_owner_id")
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
