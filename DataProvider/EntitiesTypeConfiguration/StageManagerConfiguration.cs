using DataCore.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DataProvider.EntitiesTypeConfiguration
{
    public class StageManagerConfiguration : IEntityTypeConfiguration<StageManager>
    {
        public void Configure(EntityTypeBuilder<StageManager> entity)
        {
            //entity base
            entity.HasKey(x => x.Id).HasName("pk_stage_managers");
            entity.HasIndex(x => x.Id);
            entity.ToTable("stage_managers");

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

            entity.Property(e => e.ProjectStageId).HasColumnName("project_stage_id");
            entity.Property(e => e.EmployeeId).HasColumnName("employee_id");

            entity.HasOne(pt => pt.ProjectStage).WithMany(t => t.StageManagers).HasForeignKey(pt => pt.ProjectStageId).OnDelete(DeleteBehavior.Cascade);
            entity.HasOne(pt => pt.Employee).WithMany(p => p.StageManagers).HasForeignKey(pt => pt.EmployeeId).OnDelete(DeleteBehavior.SetNull);

            entity.HasIndex(x => new { x.ProjectStageId, x.EmployeeId });

            //owner
            entity.Property(e => e.EntityOwnerId)
                .ValueGeneratedNever()
                .HasColumnName("entity_owner_id");
            entity.HasIndex(x => x.EntityOwnerId);
            entity.HasOne(x => x.EntityOwner).WithMany(x => x.StageManagers)
                .HasForeignKey(x => x.EntityOwnerId)
                .HasConstraintName("fk_stage_managers_entity_owner_id")
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
