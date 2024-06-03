using DataCore.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DataProvider.EntitiesTypeConfiguration
{
    public class ClientConfiguration : IEntityTypeConfiguration<Client>
    {
        public void Configure(EntityTypeBuilder<Client> entity)
        {
            //entity base
            entity.HasKey(x => x.Id).HasName("pk_clients");
            entity.HasIndex(x => x.Id);
            entity.ToTable("clients");

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

            entity.Property(e => e.ClientType).HasColumnName("client_type");
            entity.Property(e => e.ProjectId).HasColumnName("project_id");
            entity.Property(e => e.PersonId).HasColumnName("person_id");
            entity.Property(e => e.OrganizationId).HasColumnName("organization_id");

            entity.HasOne(d => d.Project).WithMany(p => p.Clients)
                .HasForeignKey(d => d.ProjectId)
                .HasConstraintName("fk_clients_project_id");

            entity.HasOne(e => e.Person).WithMany(p => p.ProjectClients)
                .HasForeignKey(d => d.PersonId)
                .HasConstraintName("fk_clients_person_id")
                .OnDelete(DeleteBehavior.Cascade);
            entity.HasOne(e => e.Organization).WithMany(p => p.ProjectClients)
                .HasForeignKey(d => d.OrganizationId)
                .HasConstraintName("fk_clients_organization_id")
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasIndex(x => new { x.ProjectId, x.PersonId, x.OrganizationId } ).IsUnique();

            //owner
            entity.Property(e => e.EntityOwnerId)
                .ValueGeneratedNever()
                .HasColumnName("entity_owner_id");
            entity.HasIndex(x => x.EntityOwnerId);
            entity.HasOne(x => x.EntityOwner).WithMany(x => x.Clients)
                .HasForeignKey(x => x.EntityOwnerId)
                .HasConstraintName("fk_clients_entity_owner_id")
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
