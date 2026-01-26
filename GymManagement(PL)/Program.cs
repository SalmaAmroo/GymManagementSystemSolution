using GymManagement_DAL_.Data.DbContexts;
using GymManagementBLL;
using GymManagementBLL.Services.AttachmentService;
using GymManagementBLL.Services.Interfaces;
using GymManagementBLL.Services.ServicesImplementation;
using GymManagementDAL.Data.Dataseed;
using GymManagementDAL.Entities;
using GymManagementDAL.Repositories.Classes;
using GymManagementDAL.Repositories.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
namespace GymManagementPL
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllersWithViews();
            builder.Services.AddDbContext<GymDbContext>(Options =>
            {
                Options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
            });
            builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
            builder.Services.AddScoped<ISessionRepository, SessionRepository>();
            builder.Services.AddAutoMapper(X=>X.AddProfile(new MappingProfiles())); 
            builder.Services.AddScoped<IAnalyticService, AnalyticService>();
            builder.Services.AddScoped<IMemberService,MemberService>();
            builder.Services.AddScoped<ITrainerService, TrainerService>();
            builder.Services.AddScoped<IPlanService, PlanService>();
            builder.Services.AddScoped<ISessionService, SessionService>();
            builder.Services.AddScoped<IAttachmentService, AttachmentService>();
            builder.Services.AddScoped<IAccountService, AccountService>();
            builder.Services.AddScoped<IMemberShipRepository, MemberShipRepository>();
            builder.Services.AddScoped<IMemberShipService, MemberShipService>();
            builder.Services.AddScoped<IBookingRepository, BookingRepository>();
            builder.Services.AddScoped<IBookingSsrvice, BookingService>();
            builder.Services.AddIdentity<ApplicationUser, IdentityRole>(config =>
            {
                config.User.RequireUniqueEmail = true;
            }).AddEntityFrameworkStores<GymDbContext>();
            builder.Services.ConfigureApplicationCookie(options =>
            {
                options.LogoutPath = "/Account/Login";
                options.AccessDeniedPath = "/Account/AccessDenied";
            });
   
            var app = builder.Build();
            #region Data Seeding
            using var scope = app.Services.CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<GymDbContext>();
            var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
            
            var PendingMigrations = dbContext.Database.GetPendingMigrations();
            if (PendingMigrations?.Any()??false)
            {
                dbContext.Database.Migrate();
            }
            GymDbContextSeeding.SeedData(dbContext);
            IdentityDbContextSeeding.SeedData(roleManager, userManager);
            #endregion
            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.MapStaticAssets();
            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Account}/{action=Login}/{id?}")
                .WithStaticAssets();

            app.Run();
        }
    }
}
