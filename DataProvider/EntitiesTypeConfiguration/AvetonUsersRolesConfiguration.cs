using DataCore.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DataProvider.EntitiesTypeConfiguration
{
    public class AvetonUsersRolesConfiguration : IEntityTypeConfiguration<AvetonUsersRoles>
    {
        public void Configure(EntityTypeBuilder<AvetonUsersRoles> entity)
        {
            //entity base
            entity.HasKey(x => x.Id).HasName("pk_aveton_users_roles");
            entity.HasIndex(x => x.Id);
            entity.ToTable("aveton_users_roles");

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

            entity.Property(e => e.RoleId)
                .IsRequired()
                .HasColumnName("role_id");
            entity.Property(e => e.UserId)
                .IsRequired()
                .HasColumnName("user_id");



            entity.HasOne(pt => pt.User)
                .WithMany(t => t.NavigationUserRoles)
                .HasForeignKey(pt => pt.UserId)
                .OnDelete(DeleteBehavior.Cascade);
            entity.HasOne(pt => pt.Role)
                .WithMany(p => p.NavigationUserRoles)
                .HasForeignKey(pt => pt.RoleId)
                .OnDelete(DeleteBehavior.Cascade);


            entity.HasIndex(x => x.UserId);
            entity.HasIndex(x => x.RoleId);

            //owner
            entity.Property(e => e.EntityOwnerId)
                .ValueGeneratedNever()
                .HasColumnName("entity_owner_id");
            entity.HasIndex(x => x.EntityOwnerId);
            entity.HasOne(x => x.EntityOwner).WithMany(x => x.UsersRoles)
                .HasForeignKey(x => x.EntityOwnerId)
                .HasConstraintName("fk_aveton_users_roles_entity_owner_id")
                .OnDelete(DeleteBehavior.Cascade);

        }
    }
}
