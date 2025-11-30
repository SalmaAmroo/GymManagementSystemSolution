using GymManagementDAL.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;


namespace GymManagement_DAL_.Data.Configurations
{
    internal class PlanConfiguration : IEntityTypeConfiguration<Plan>
    {
        public void Configure(EntityTypeBuilder<Plan> builder)
        {
            builder.Property(P => P.Name)
                   .HasColumnType("varchar")
                   .HasMaxLength(50);
            builder.Property(P => P.Description)
                     .HasColumnType("varchar")
                     .HasMaxLength(100);
            builder.Property(P => P.Price)
                   .HasPrecision(10, 2);
            builder.ToTable(Tb => 
            {
                Tb.HasCheckConstraint("PlanDurationCheck", "DurationDate Between 1 and 365");
              
            });
        }
    }
}
