using DataCore.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DataProvider.EntitiesTypeConfiguration
{
    public class JobConfiguration : IEntityTypeConfiguration<Job>
    {
        public void Configure(EntityTypeBuilder<Job> entity)
        {
            //entity base
            entity.HasKey(x => x.Id).HasName("pk_jobs");
            entity.HasIndex(x => x.Id);
            entity.ToTable("jobs");

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

            entity.Property(e => e.StartDate)
                .HasColumnType(DbConfig.DbTimeStampType)
                .HasColumnName("start_date");
            entity.Property(e => e.PositionId).HasColumnName("position_id");
            entity.Property(e => e.EmployeeId).HasColumnName("employee_id");
            entity.Property(e => e.DivisionId).HasColumnName("division_id");



            //navigation props
            entity.HasOne(e => e.Position).WithMany(p => p.Jobs)
                .HasForeignKey(d => d.PositionId)
                .HasConstraintName("fk_jobs_position_id")
                .OnDelete(DeleteBehavior.SetNull);
            entity.HasOne(e => e.Division).WithMany(p => p.Jobs)
                .HasForeignKey(d => d.DivisionId)
                .HasConstraintName("fk_jobs_division_id")
                .OnDelete(DeleteBehavior.SetNull);
            entity.HasOne(e => e.Employee).WithMany(p => p.Jobs)
                .HasForeignKey(d => d.EmployeeId)
                .HasConstraintName("fk_jobs_employee_id")
                .OnDelete(DeleteBehavior.Cascade);

            //owner
            entity.Property(e => e.EntityOwnerId)
                .ValueGeneratedNever()
                .HasColumnName("entity_owner_id");
            entity.HasIndex(x => x.EntityOwnerId);
            entity.HasOne(x => x.EntityOwner).WithMany(x => x.Jobs)
                .HasForeignKey(x => x.EntityOwnerId)
                .HasConstraintName("fk_jobs_entity_owner_id")
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
