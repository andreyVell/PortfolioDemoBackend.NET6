using DataCore.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DataProvider.EntitiesTypeConfiguration
{
    public class ProjectStageConfiguration : IEntityTypeConfiguration<ProjectStage>
    {
        public void Configure(EntityTypeBuilder<ProjectStage> entity)
        {
            //entity base
            entity.HasKey(x => x.Id).HasName("pk_project_stages");
            entity.HasIndex(x => x.Id);
            entity.ToTable("project_stages");

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
            entity.Property(e => e.ProjectId).HasColumnName("project_id");
            entity.Property(e => e.ParentStageId).HasColumnName("parent_stage_id");
            entity.Property(e => e.IsCompleted)
                .HasDefaultValue(false)
                .HasColumnName("is_completed");
            entity.Property(e => e.OrderNumber)                
                .HasColumnName("order_number");

            //navigation props
            entity.HasOne(e => e.Project).WithMany(p => p.Stages)
                .HasForeignKey(d => d.ProjectId)
                .HasConstraintName("fk_project_stages_project_id")
                .OnDelete(DeleteBehavior.Cascade);

            //navigation props
            entity.HasOne(e => e.ParentStage).WithMany(p => p.ChildStages)
                .HasForeignKey(d => d.ParentStageId)
                .HasConstraintName("fk_project_stages_parent_stage_id")
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasMany(e => e.Contractors)
                .WithMany(e => e.ProjectStages)
                .UsingEntity<DivisionContractor>();

            entity.HasIndex(x => x.ProjectId);
            entity.HasIndex(x => x.ParentStageId);


            //owner
            entity.Property(e => e.EntityOwnerId)
                .ValueGeneratedNever()
                .HasColumnName("entity_owner_id");
            entity.HasIndex(x => x.EntityOwnerId);
            entity.HasOne(x => x.EntityOwner).WithMany(x => x.ProjectStages)
                .HasForeignKey(x => x.EntityOwnerId)
                .HasConstraintName("fk_project_stages_entity_owner_id")
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
