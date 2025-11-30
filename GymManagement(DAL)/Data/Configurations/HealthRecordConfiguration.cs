using GymManagementDAL.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GymManagement_DAL_.Data.Configurations
{
    internal class HealthRecordConfiguration : IEntityTypeConfiguration<HealthRecord>
    {
        public void Configure(EntityTypeBuilder<HealthRecord> builder)
        {
            builder.ToTable("Members")
                    .HasKey(HR => HR.Id); 

            builder.HasOne<Member>()
                   .WithOne(M => M.HealthRecord)
                   .HasForeignKey<HealthRecord>(HR => HR.Id);
            builder.Ignore(HR => HR.CreatedAt);
            builder.Ignore(HR => HR.UpdatedAt);

        }
    }
}
