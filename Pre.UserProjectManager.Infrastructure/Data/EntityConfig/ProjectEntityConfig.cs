using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Pre.UserProjectManager.Core.Entity;

namespace Pre.UserProjectManager.Infrastructure.Data.EntityConfig
{
    public class ProjectEntityConfig : IEntityTypeConfiguration<Project>
    {
        public void Configure(EntityTypeBuilder<Project> builder)
        {
            builder.HasKey(t => t.Id);
            builder.Property(t => t.Id).HasColumnName("id").ValueGeneratedOnAdd();

            builder.Property(t => t.Name).HasColumnName("name").IsRequired();
            builder.Property(t => t.TimeCreated).HasColumnName("time_created");
            builder.Property(t => t.DateCreated).HasColumnName("date_created");
            builder.Property(t => t.LastUpdatedTime).HasColumnName("last_updated_time");

            builder.HasOne(t => t.User)
              .WithMany(t => t.Projects)
              .HasForeignKey(t => t.UserId)
              .HasConstraintName("fk_project_user_id")
              .IsRequired(false)
              .OnDelete(DeleteBehavior.NoAction);


            builder.HasIndex(x => new { x.UserId, x.Name })
              .HasDatabaseName("ix_prject_name_id").IsUnique(false);

            builder.HasIndex(x => new { x.Name })
              .HasDatabaseName("ix_prject_name_unique_id").IsUnique(false);


            builder.ToTable("project");
        }
    }
}
