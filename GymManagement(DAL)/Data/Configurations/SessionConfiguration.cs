using GymManagementDAL.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GymManagement_DAL_.Data.Configurations
{
    internal class SessionConfiguration : IEntityTypeConfiguration<Session>
    {
        public void Configure(EntityTypeBuilder<Session> builder)
        {
            builder.ToTable(Tb =>
            {
                Tb.HasCheckConstraint("SessionCapacityCheck", "Capacity Between 1 and 25");
                Tb.HasCheckConstraint("SessionEndDateCheck", "EndDate > StartDate");
            });

            builder.HasOne(x=>x.SessionCategory)
                   .WithMany(C=>C.sessions)
                   .HasForeignKey(x=>x.CategoryId);
            builder.HasOne(x=>x.TrainerSession)
                   .WithMany(T=>T.TrainerSessions)
                   .HasForeignKey(x=>x.TrainerId);
        }
    }
}
