using DataCore.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DataProvider.EntitiesTypeConfiguration
{
    public class ChatMemberConfiguration : IEntityTypeConfiguration<ChatMember>
    {
        public void Configure(EntityTypeBuilder<ChatMember> entity)
        {
            //entity base
            entity.HasKey(x => x.Id).HasName("pk_chat_members");
            entity.HasIndex(x => x.Id);
            entity.ToTable("chat_members");

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
            entity.Property(e => e.Type)
                .HasColumnName("type"); 

            //navigation props
            entity.Property(e => e.ChatId).HasColumnName("chat_id");
            entity.HasOne(e => e.Chat).WithMany(p => p.ChatMembers)
                .HasForeignKey(d => d.ChatId)
                .HasConstraintName("fk_chat_members_chat_id")
                .OnDelete(DeleteBehavior.Cascade);
            entity.HasIndex(x => x.ChatId);

            entity.Property(e => e.EmployeeId).HasColumnName("employee_id");
            entity.HasOne(e => e.Employee).WithMany(p => p.ChatMembers)
                .HasForeignKey(d => d.EmployeeId)
                .HasConstraintName("fk_chat_members_employee_id")
                .OnDelete(DeleteBehavior.SetNull);
            entity.HasIndex(x => x.EmployeeId);

            entity.Property(e => e.OrganizationClientId).HasColumnName("organization_id");
            entity.HasOne(e => e.OrganizationClient).WithMany(p => p.ChatMembers)
                .HasForeignKey(d => d.OrganizationClientId)
                .HasConstraintName("fk_chat_members_organization_id")
                .OnDelete(DeleteBehavior.SetNull);
            entity.HasIndex(x => x.OrganizationClientId);

            entity.Property(e => e.PersonClientId).HasColumnName("person_id");
            entity.HasOne(e => e.PersonClient).WithMany(p => p.ChatMembers)
                .HasForeignKey(d => d.PersonClientId)
                .HasConstraintName("fk_chat_members_person_id")
                .OnDelete(DeleteBehavior.SetNull);
            entity.HasIndex(x => x.PersonClientId);

            //owner
            entity.Property(e => e.EntityOwnerId)
                .ValueGeneratedNever()
                .HasColumnName("entity_owner_id");
            entity.HasIndex(x => x.EntityOwnerId);
            entity.HasOne(x => x.EntityOwner).WithMany(x => x.ChatMembers)
                .HasForeignKey(x => x.EntityOwnerId)
                .HasConstraintName("fk_projects_entity_owner_id")
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
