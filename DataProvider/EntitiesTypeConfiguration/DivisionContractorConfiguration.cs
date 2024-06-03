using DataCore.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DataProvider.EntitiesTypeConfiguration
{
    public class DivisionContractorConfiguration : IEntityTypeConfiguration<DivisionContractor>
    {
        public void Configure(EntityTypeBuilder<DivisionContractor> entity)
        {
            //entity base
            entity.HasKey(x => x.Id).HasName("pk_division_contractors");
            entity.HasIndex(x => x.Id);
            entity.ToTable("division_contractors");

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

            entity.Property(e => e.DivisionId)
                .IsRequired()
                .HasColumnName("division_id");
            entity.Property(e => e.ProjectStageId)
                .IsRequired()
                .HasColumnName("project_stage_id");

            entity.HasOne(pt => pt.Division)
                .WithMany(t => t.NavigationDivisionContractors)
                .HasForeignKey(pt => pt.DivisionId)
                .OnDelete(DeleteBehavior.Cascade);
            entity.HasOne(pt => pt.ProjectStage)
                .WithMany(p => p.NavigationDivisionContractors)
                .HasForeignKey(pt => pt.ProjectStageId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasIndex(x => new { x.DivisionId, x.ProjectStageId } );

            //owner
            entity.Property(e => e.EntityOwnerId)
                .ValueGeneratedNever()
                .HasColumnName("entity_owner_id");
            entity.HasIndex(x => x.EntityOwnerId);
            entity.HasOne(x => x.EntityOwner).WithMany(x => x.DivisionContractors)
                .HasForeignKey(x => x.EntityOwnerId)
                .HasConstraintName("fk_division_contractors_entity_owner_id")
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
