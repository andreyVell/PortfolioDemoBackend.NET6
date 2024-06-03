using DataCore.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DataProvider.EntitiesTypeConfiguration
{
    public class StageReportAttachedFileConfiguration : IEntityTypeConfiguration<StageReportAttachedFile>
    {
        public void Configure(EntityTypeBuilder<StageReportAttachedFile> entity)
        {
            //entity base
            entity.HasKey(x => x.Id).HasName("pk_stage_report_attached_files");
            entity.HasIndex(x => x.Id);
            entity.ToTable("stage_report_attached_files");

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

            entity.Property(e => e.FileName)
                .HasMaxLength(250)
                .HasColumnName("file_name");
            entity.Property(e => e.FilePath)
                .HasMaxLength(500)
                .HasColumnName("file_path");
            entity.Property(e => e.ImageMediumSizeFilePath)
                .HasMaxLength(500)
                .HasColumnName("image_medium_size_file_path");
            entity.Property(e => e.StageReportId).HasColumnName("stage_report_id");

            entity.HasOne(pt => pt.StageReport).WithMany(p => p.AttachedFiles).HasForeignKey(pt => pt.StageReportId).OnDelete(DeleteBehavior.SetNull);

            entity.HasIndex(x => x.StageReportId);

            //owner
            entity.Property(e => e.EntityOwnerId)
                .ValueGeneratedNever()
                .HasColumnName("entity_owner_id");
            entity.HasIndex(x => x.EntityOwnerId);
            entity.HasOne(x => x.EntityOwner).WithMany(x => x.StageReportAttachedFiles)
                .HasForeignKey(x => x.EntityOwnerId)
                .HasConstraintName("fk_stage_report_attached_files_entity_owner_id")
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
