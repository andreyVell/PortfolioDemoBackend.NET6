using DataCore.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DataProvider.EntitiesTypeConfiguration
{
    public class ChatMessageViewedInfoConfiguration : IEntityTypeConfiguration<ChatMessageViewedInfo>
    {
        public void Configure(EntityTypeBuilder<ChatMessageViewedInfo> entity)
        {
            //entity base
            entity.HasKey(x => x.Id).HasName("pk_chat_message_viewed_infos");
            entity.HasIndex(x => x.Id);
            entity.ToTable("chat_message_viewed_infos");

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


            //navigation props
            entity.Property(e => e.MessageId).HasColumnName("message_id");
            entity.HasOne(e => e.Message).WithMany(p => p.ViewedInfos)
                .HasForeignKey(d => d.MessageId)
                .HasConstraintName("fk_chat_message_viewed_infos_message_id")
                .OnDelete(DeleteBehavior.Cascade);
            entity.HasIndex(x => x.MessageId);

            entity.Property(e => e.ViewedById).HasColumnName("viewed_by_id");
            entity.HasOne(e => e.ViewedBy).WithMany(p => p.ViewedMessages)
                .HasForeignKey(d => d.ViewedById)
                .HasConstraintName("fk_chat_message_viewed_infos_viewed_by_id")
                .OnDelete(DeleteBehavior.Cascade);
            entity.HasIndex(x => x.ViewedById);

            //owner
            entity.Property(e => e.EntityOwnerId)
                .ValueGeneratedNever()
                .HasColumnName("entity_owner_id");
            entity.HasIndex(x => x.EntityOwnerId);
            entity.HasOne(x => x.EntityOwner).WithMany(x => x.ChatMessageViewedInfos)
                .HasForeignKey(x => x.EntityOwnerId)
                .HasConstraintName("fk_projects_entity_owner_id")
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
