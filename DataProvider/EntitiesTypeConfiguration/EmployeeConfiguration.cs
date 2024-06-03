using DataCore.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DataProvider.EntitiesTypeConfiguration
{
    public class EmployeeConfiguration : IEntityTypeConfiguration<Employee>
    {
        public void Configure(EntityTypeBuilder<Employee> entity)
        {
            //entity base
            entity.HasKey(x => x.Id).HasName("pk_employees");
            entity.HasIndex(x => x.Id);
            entity.ToTable("employees");

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
            entity.Property(e => e.Email)
                .HasMaxLength(250)
                .HasColumnName("email");
            entity.Property(e => e.Birthday)
                .HasColumnType(DbConfig.DbTimeStampType)
                .HasColumnName("birthday");
            entity.Property(e => e.FirstName)
                .HasMaxLength(250)
                .HasColumnName("first_name");
            entity.Property(e => e.LastName)
                .HasMaxLength(250)
                .HasColumnName("last_name");
            entity.Property(e => e.SecondName)
                .HasMaxLength(250)
                .HasColumnName("second_name");
            entity.Property(e => e.MobilePhoneNumber)
                .HasMaxLength(250)
                .HasColumnName("mobile_phone_number");
            entity.Property(e => e.PathToAvatar)
                .HasMaxLength(250)
                .HasColumnName("path_to_avatar");
            entity.Property(e => e.PathToSmallAvatar)
                .HasMaxLength(250)
                .HasColumnName("path_to_small_avatar");
            entity.Property(e => e.CredentialsId).HasColumnName("credentials_id");



            //navigation props
            entity.HasOne(e => e.Credentials).WithOne(p => p.Employee)
                .HasForeignKey<Employee>(d=>d.CredentialsId)
                .HasConstraintName("fk_employees_aveton_user_id")
                .OnDelete(DeleteBehavior.SetNull);

            //owner
            entity.Property(e => e.EntityOwnerId)
                .ValueGeneratedNever()
                .HasColumnName("entity_owner_id");
            entity.HasIndex(x => x.EntityOwnerId);
            entity.HasOne(x => x.EntityOwner).WithMany(x => x.Employees)
                .HasForeignKey(x => x.EntityOwnerId)
                .HasConstraintName("fk_employees_entity_owner_id")
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
