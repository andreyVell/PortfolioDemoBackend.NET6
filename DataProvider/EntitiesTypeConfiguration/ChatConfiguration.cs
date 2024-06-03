using DataCore.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DataProvider.EntitiesTypeConfiguration
{
    public class ChatConfiguration : IEntityTypeConfiguration<Chat>
    {
        public void Configure(EntityTypeBuilder<Chat> entity)
        {
            //entity base
            entity.HasKey(x => x.Id).HasName("pk_chats");
            entity.HasIndex(x => x.Id);
            entity.ToTable("chats");

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
                .HasMaxLength(300)
                .HasColumnName("name");
            entity.Property(e => e.PathToAvatarSmallImage)
                .HasMaxLength(500)
                .HasColumnName("path_to_avatar_small_image");
            entity.Property(e => e.PathToAvatarBigImage)
                .HasMaxLength(500)
                .HasColumnName("path_to_avatar_big_image");
            entity.Property(e => e.IsGroupChat)
                .HasDefaultValue(false)
                .HasColumnName("is_group_chat");

            //owner
            entity.Property(e => e.EntityOwnerId)
                .ValueGeneratedNever()
                .HasColumnName("entity_owner_id");
            entity.HasIndex(x => x.EntityOwnerId);
            entity.HasOne(x => x.EntityOwner).WithMany(x => x.Chats)
                .HasForeignKey(x => x.EntityOwnerId)
                .HasConstraintName("fk_chats_entity_owner_id")
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
