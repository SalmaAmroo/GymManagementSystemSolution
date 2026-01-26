using GymManagementDAL.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Reflection;


namespace GymManagement_DAL_.Data.DbContexts
{
    public class GymDbContext : IdentityDbContext<ApplicationUser>
    {
        public GymDbContext( DbContextOptions<GymDbContext> options) : base(options)
        {
            
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
            modelBuilder.Entity<ApplicationUser>(Au =>
            {
                Au.Property(x => x.FirstName)
                  .HasColumnType("varchar")
                  .HasMaxLength(50);

                Au.Property(x => x.LastName)
                  .HasColumnType("varchar")
                  .HasMaxLength(50);

            });
        }

        #region BbSets
        public DbSet<Member> Members { get; set; }
        public DbSet<HealthRecord> HealthRecord { get; set; }
        public DbSet<Trainer> Trainers { get; set; }
        public DbSet<Plan> Plans { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Session> Sessions { get; set; }
        public DbSet<MemberShip> MemberShips { get; set; }
        public DbSet<MemberSession> MemberSessions { get; set; }
     
        #endregion
    }
}
