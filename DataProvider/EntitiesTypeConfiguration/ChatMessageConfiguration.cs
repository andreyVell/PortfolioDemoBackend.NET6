using DataCore.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DataProvider.EntitiesTypeConfiguration
{
    public class ChatMessageConfiguration : IEntityTypeConfiguration<ChatMessage>
    {
        public void Configure(EntityTypeBuilder<ChatMessage> entity)
        {
            //entity base
            entity.HasKey(x => x.Id).HasName("pk_chat_messages");
            entity.HasIndex(x => x.Id);
            entity.ToTable("chat_messages");

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
            entity.Property(e => e.Text)
                .HasMaxLength(2000)
                .HasColumnName("text");
            entity.HasIndex(x => x.Text);

            entity.Property(e => e.IsSystem)
                .HasDefaultValue(false)
                .HasColumnName("is_system");

            //navigation props
            entity.Property(e => e.ChatId).HasColumnName("chat_id");
            entity.HasOne(e => e.Chat).WithMany(p => p.Messages)
                .HasForeignKey(d => d.ChatId)
                .HasConstraintName("fk_chat_messages_chat_id")
                .OnDelete(DeleteBehavior.Cascade);
            entity.HasIndex(x => x.ChatId);

            entity.Property(e => e.OwnerId).HasColumnName("owner_id");
            entity.HasOne(e => e.Owner).WithMany(p => p.OutcomingMessages)
                .HasForeignKey(d => d.OwnerId)
                .HasConstraintName("fk_chat_messages_owner_id")
                .OnDelete(DeleteBehavior.SetNull);
            entity.HasIndex(x => x.OwnerId);

            //owner
            entity.Property(e => e.EntityOwnerId)
                .ValueGeneratedNever()
                .HasColumnName("entity_owner_id");
            entity.HasIndex(x => x.EntityOwnerId);
            entity.HasOne(x => x.EntityOwner).WithMany(x => x.ChatMessages)
                .HasForeignKey(x => x.EntityOwnerId)
                .HasConstraintName("fk_projects_entity_owner_id")
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
