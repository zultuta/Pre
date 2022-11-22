using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Pre.UserProjectManager.Core.Entity;

namespace Pre.UserProjectManager.Infrastructure.Data.EntityConfig
{
    public class ProductEntityConfig : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> builder)
        {
            builder.HasKey(t => t.Id);
            builder.Property(t => t.Id).HasColumnName("id").ValueGeneratedOnAdd();

            builder.Property(t => t.Name).HasColumnName("name").IsRequired();
            builder.Property(t => t.Weight).HasColumnName("weight");
            builder.Property(t => t.Unit).HasColumnName("unit");
            builder.Property(t => t.CarbonFootPrint).HasColumnName("carbon_footprint");
            builder.Property(t => t.CarbonFootPrintPerGram).HasColumnName("carbon_footprint_per_gram");
            builder.Property(t => t.TimeCreated).HasColumnName("time_created");
            builder.Property(t => t.DateCreated).HasColumnName("date_created");
            builder.Property(t => t.LastUpdatedTime).HasColumnName("last_updated_time");

            builder.HasOne(t => t.Project)
              .WithMany(t => t.Products)
              .HasForeignKey(t => t.ProjectId)
              .HasConstraintName("fk_product_project_id")
              .IsRequired(false)
              .OnDelete(DeleteBehavior.Cascade);


            builder.ToTable("product");
        }
    }
}
