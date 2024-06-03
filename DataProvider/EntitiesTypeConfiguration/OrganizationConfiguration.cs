using DataCore.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DataProvider.EntitiesTypeConfiguration
{
    public class OrganizationConfiguration : IEntityTypeConfiguration<Organization>
    {
        public void Configure(EntityTypeBuilder<Organization> entity)
        {
            //entity base
            entity.HasKey(x => x.Id).HasName("pk_organizations");
            entity.HasIndex(x => x.Id);
            entity.ToTable("organizations");

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

            entity.Property(e => e.Name)
                .HasMaxLength(300)
                .HasColumnName("name");
            entity.Property(e => e.Inn)
                .HasMaxLength(20)
                .HasColumnName("inn");
            entity.Property(e => e.ContactEmail)
                .HasMaxLength(100)
                .HasColumnName("contact_email");
            entity.Property(e => e.ContactPhone)
                .HasMaxLength(100)
                .HasColumnName("contact_phone");

            
            entity.Property(e => e.Login)
                .HasMaxLength(50)
                .HasColumnName("login");
            entity.Property(e => e.Password)
                .HasMaxLength(50)
                .HasColumnName("password");

            //owner
            entity.Property(e => e.EntityOwnerId)
                .ValueGeneratedNever()
                .HasColumnName("entity_owner_id");
            entity.HasIndex(x => x.EntityOwnerId);
            entity.HasOne(x => x.EntityOwner).WithMany(x => x.Organizations)
                .HasForeignKey(x => x.EntityOwnerId)
                .HasConstraintName("fk_organizations_entity_owner_id")
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
