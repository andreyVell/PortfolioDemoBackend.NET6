using DataCore.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DataProvider.EntitiesTypeConfiguration
{
    public class ChatMessageAttachedFileConfiguration : IEntityTypeConfiguration<ChatMessageAttachedFile>
    {
        public void Configure(EntityTypeBuilder<ChatMessageAttachedFile> entity)
        {
            //entity base
            entity.HasKey(x => x.Id).HasName("pk_chat_message_attached_files");
            entity.HasIndex(x => x.Id);
            entity.ToTable("chat_message_attached_files");

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
            entity.Property(e => e.FilePath)
                .HasMaxLength(500)
                .HasColumnName("file_path");
            entity.Property(e => e.ImageMediumSizeFilePath)
                .HasMaxLength(500)
                .HasColumnName("image_medium_size_file_path");
            entity.Property(e => e.FileName)
                .HasMaxLength(250)
                .HasColumnName("file_name");

            //navigation props
            entity.Property(e => e.ChatId).HasColumnName("chat_id").IsRequired(false);
            entity.HasOne(e => e.Chat).WithMany(p => p.AttachedFiles)                
                .HasForeignKey(d => d.ChatId)
                .HasConstraintName("fk_chat_message_attached_files_chat_id")
                .OnDelete(DeleteBehavior.SetNull);
            entity.HasIndex(x => x.ChatId);

            entity.Property(e => e.MessageId).HasColumnName("message_id").IsRequired(false);
            entity.HasOne(e => e.Message).WithMany(p => p.AttachedFiles)
                .HasForeignKey(d => d.MessageId)
                .HasConstraintName("fk_chat_message_attached_files_message_id")
                .OnDelete(DeleteBehavior.SetNull);
            entity.HasIndex(x => x.MessageId);

            //owner
            entity.Property(e => e.EntityOwnerId)
                .ValueGeneratedNever()
                .HasColumnName("entity_owner_id");
            entity.HasIndex(x => x.EntityOwnerId);
            entity.HasOne(x => x.EntityOwner).WithMany(x => x.ChatMessageAttachedFiles)
                .HasForeignKey(x => x.EntityOwnerId)
                .HasConstraintName("fk_projects_entity_owner_id")
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
