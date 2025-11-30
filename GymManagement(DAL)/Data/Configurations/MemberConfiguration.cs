using GymManagementDAL.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;


namespace GymManagement_DAL_.Data.Configurations
{
    internal class MemberConfiguration : GymUserConfiguration<Member>, IEntityTypeConfiguration<Member>
    {
        public new void Configure(EntityTypeBuilder<Member> builder)
        {
            builder.Property(M=>M.CreatedAt)
                   .HasColumnName("JoinDate")
                   .HasDefaultValueSql("GETDATE()");
            base.Configure(builder);
        }

    }
}
