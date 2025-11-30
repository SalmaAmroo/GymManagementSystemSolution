using GymManagementDAL.Entities;
using Microsoft.EntityFrameworkCore;
using System.Reflection;


namespace GymManagement_DAL_.Data.DbContexts
{
    public class GymDbContext : DbContext
    {
        public GymDbContext( DbContextOptions<GymDbContext> options) : base(options)
        {
            
        }
        //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        //{
        //    optionsBuilder.UseSqlServer("Server=.;Database=GymManagementDB;Trusted_Connection=True;TrustServerCertificate=True;");

        //}
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
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
