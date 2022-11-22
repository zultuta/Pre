using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Pre.UserProjectManager.Core.Entity;

namespace Pre.UserProjectManager.Infrastructure.Data.EntityConfig
{
    public class UserEntityConfig : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.HasKey(t => t.Id);
            builder.Property(t => t.Id).HasColumnName("id").ValueGeneratedOnAdd();

            builder.Property(t => t.FirstName).HasColumnName("first_name").IsRequired();
            builder.Property(t => t.LastName).HasColumnName("last_name").IsRequired();
            builder.Property(t => t.Email).HasColumnName("email");
            builder.Property(t => t.UserName).HasColumnName("user_name").IsRequired();
            builder.Property(t => t.Password).HasColumnName("password").IsRequired();
            builder.Property(t => t.PasswordSalt).HasColumnName("password_salt").IsRequired();
            builder.Property(t => t.LastLoginDate).HasColumnName("last_login_date");
            builder.Property(t => t.DateCreated).HasColumnName("date_created");
            builder.Property(t => t.TimeCreated).HasColumnName("time_created");
            builder.Property(t => t.LastUpdatedTime).HasColumnName("last_updated_time");

            builder.HasIndex(x => x.UserName)
               .HasDatabaseName("ix_user_user_name").IsUnique(false);

            builder.ToTable("user");
        }
    }

}
