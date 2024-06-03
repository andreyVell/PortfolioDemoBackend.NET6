using DataCore.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DataProvider.EntitiesTypeConfiguration
{
    public class AvetonUserConfiguration : IEntityTypeConfiguration<AvetonUser>
    {
        public void Configure(EntityTypeBuilder<AvetonUser> entity)
        {
            entity.HasKey(x => x.Id).HasName("pk_aveton_users");
            entity.HasAlternateKey(x => x.Login);
            entity.HasIndex(x => x.Id);
            entity.HasIndex(x => x.Login).IsUnique();
            entity.ToTable("aveton_users");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("id");
            entity.Property(e => e.CreatedOn)
                .HasColumnType(DbConfig.DbTimeStampType)
                .HasColumnName("created_on");            
            entity.Property(e => e.UpdatedOn)
                .HasColumnType(DbConfig.DbTimeStampType)
                .HasColumnName("updated_on");
            entity.Property(e => e.Login)                
                .HasMaxLength(50)
                .HasColumnName("login");
            entity.Property(e => e.PasswordHash)
                .HasMaxLength(100)
                .HasColumnName("password_hash");
            entity.Property(e => e.PasswordSalt)
                .HasMaxLength(100)
                .HasColumnName("password_salt");

            entity.HasMany(e => e.Roles)
                .WithMany(e => e.Users)
                .UsingEntity<AvetonUsersRoles>();

            //owner
            entity.Property(e => e.OwnerId)
                .ValueGeneratedNever()
                .HasColumnName("entity_owner_id");
            entity.HasIndex(x => x.OwnerId);
            entity.HasOne(x => x.Owner).WithMany(x => x.Users)
                .HasForeignKey(x => x.OwnerId)
                .HasConstraintName("fk_aveton_users_entity_owner_id")
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
