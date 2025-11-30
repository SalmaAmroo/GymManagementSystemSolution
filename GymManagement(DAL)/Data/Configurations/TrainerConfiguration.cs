using GymManagementDAL.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GymManagement_DAL_.Data.Configurations
{
    internal class TrainerConfiguration : GymUserConfiguration<Trainer>, IEntityTypeConfiguration<Trainer>
    {
        public new void Configure(EntityTypeBuilder<Trainer> builder)
        {
            builder.Property(T=> T.CreatedAt)
                  .HasColumnName("HireDate")
                  .HasDefaultValueSql("GETDATE()");
            base.Configure(builder);
        }

    }
}
