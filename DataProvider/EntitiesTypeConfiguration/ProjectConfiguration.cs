using DataCore.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DataProvider.EntitiesTypeConfiguration
{
    public class ProjectConfiguration : IEntityTypeConfiguration<Project>
    {
        public void Configure(EntityTypeBuilder<Project> entity)
        {
            //entity base
            entity.HasKey(x => x.Id).HasName("pk_projects");
            entity.HasIndex(x => x.Id);
            entity.ToTable("projects");

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
                .HasMaxLength(1000)
                .HasColumnName("name");
            entity.Property(e => e.Description)
                .HasMaxLength(5000)
                .HasColumnName("description");
            entity.Property(e => e.ManagerId).HasColumnName("manager_id");

            //navigation props
            entity.HasOne(e => e.Manager).WithMany(p => p.ProjectsWhereThisEmployeeIsManager)
                .HasForeignKey(d => d.ManagerId)
                .HasConstraintName("fk_projects_manager_id")
                .OnDelete(DeleteBehavior.SetNull);

            entity.HasIndex(x => x.ManagerId);

            //owner
            entity.Property(e => e.EntityOwnerId)
                .ValueGeneratedNever()
                .HasColumnName("entity_owner_id");
            entity.HasIndex(x => x.EntityOwnerId);
            entity.HasOne(x => x.EntityOwner).WithMany(x => x.Projects)
                .HasForeignKey(x => x.EntityOwnerId)
                .HasConstraintName("fk_projects_entity_owner_id")
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
