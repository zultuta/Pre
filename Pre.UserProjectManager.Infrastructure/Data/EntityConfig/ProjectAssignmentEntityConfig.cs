using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Pre.UserProjectManager.Core.Entity;

namespace Pre.UserProjectManager.Infrastructure.Data.EntityConfig
{
    public class ProjectAssignmentEntityConfig : IEntityTypeConfiguration<ProjectAssignment>
    {
        public void Configure(EntityTypeBuilder<ProjectAssignment> builder)
        {
            builder.HasKey(t => t.Id);
            builder.Property(t => t.Id).HasColumnName("id").ValueGeneratedOnAdd();

            builder.Property(t => t.AssigneeUserId).HasColumnName("assignee_user_id");
            builder.Property(t => t.ProjectId).HasColumnName("project_id");
            builder.Property(t => t.AssignedBy).HasColumnName("assigned_by");
            builder.Property(t => t.DateCreated).HasColumnName("date_created");
            builder.Property(t => t.TimeCreated).HasColumnName("time_created");
            builder.Property(t => t.LastUpdatedTime).HasColumnName("last_updated_time");

            builder.HasIndex(x => new { x.AssigneeUserId, x.ProjectId })
              .HasDatabaseName("ix_assignee_user_project_id").IsUnique(false);

            builder.ToTable("project_assignment");
        }
    }
}
