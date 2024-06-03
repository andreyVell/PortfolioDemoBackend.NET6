using DataCore.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DataProvider.EntitiesTypeConfiguration
{
    public class StageReportConfiguration : IEntityTypeConfiguration<StageReport>
    {
        public void Configure(EntityTypeBuilder<StageReport> entity)
        {
            //entity base
            entity.HasKey(x => x.Id).HasName("pk_stage_reports");
            entity.HasIndex(x => x.Id);
            entity.ToTable("stage_reports");

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
            entity.Property(e => e.ReportDate).HasColumnType(DbConfig.DbTimeStampType).HasColumnName("report_date");
            entity.Property(e => e.StageManagerId).HasColumnName("stage_manager_id");
            entity.Property(e => e.EmployeeId).HasColumnName("employee_id");
            entity.Property(e => e.Name).HasMaxLength(500).HasColumnName("name");
            entity.Property(e => e.Content).HasMaxLength(2000).HasColumnName("content");

            entity.HasOne(pt => pt.Employee).WithMany(t => t.StageReports).HasForeignKey(pt => pt.EmployeeId).OnDelete(DeleteBehavior.SetNull);
            entity.HasOne(pt => pt.StageManager).WithMany(t => t.StageReports).HasForeignKey(pt => pt.StageManagerId).OnDelete(DeleteBehavior.SetNull);
            entity.HasOne(pt => pt.ProjectStage).WithMany(t => t.StageReports).HasForeignKey(pt => pt.ProjectStageId).OnDelete(DeleteBehavior.Cascade);

            entity.HasIndex(x => x.EmployeeId);
            entity.HasIndex(x => x.StageManagerId);
            entity.HasIndex(x => x.ProjectStageId);

            //owner
            entity.Property(e => e.EntityOwnerId)
                .ValueGeneratedNever()
                .HasColumnName("entity_owner_id");
            entity.HasIndex(x => x.EntityOwnerId);
            entity.HasOne(x => x.EntityOwner).WithMany(x => x.StageReports)
                .HasForeignKey(x => x.EntityOwnerId)
                .HasConstraintName("fk_stage_reports_entity_owner_id")
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
