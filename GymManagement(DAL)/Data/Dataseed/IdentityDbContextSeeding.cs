using GymManagementDAL.Entities;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagementDAL.Data.Dataseed
{
    public static class IdentityDbContextSeeding
    {
        public static bool SeedData(RoleManager<IdentityRole> roleManager, UserManager<ApplicationUser> userManager)
        {
            try
            {
                var HasUsers = userManager.Users.Any();
                var HasRoles = roleManager.Roles.Any();
                if (HasRoles && HasUsers) return false;
                if (!HasRoles)
                {
                    var Roles = new List<IdentityRole>
                    {
                        new (){ Name = "SuperAdmin"},
                        new (){ Name = "Admin"},
                    };
                    foreach (var role in Roles)
                    {
                        if (!roleManager.RoleExistsAsync(role.Name!).Result)
                        {
                            roleManager.CreateAsync(role).Wait();

                        }

                    }

                }
                if (!HasUsers)
                {
                    var MainAdmin = new ApplicationUser()
                    {
                        FirstName = "Salma",
                        LastName = "Amro",
                        UserName = "SalmaAmro",
                        Email = "SalmaAmro@gmail.com",
                        PhoneNumber = "01032117876"
                    };
                    userManager.CreateAsync(MainAdmin, "P@ss0rd").Wait();
                    userManager.AddToRoleAsync(MainAdmin, "SuperAdmin").Wait();
                    var Admin = new ApplicationUser()
                    {
                        FirstName = "Karim",
                        LastName = "Mohamed",
                        UserName = "KarimMohamed",
                        Email = "KarimMohamed@gmail.com",
                        PhoneNumber = "01023062004"
                    };
                    userManager.CreateAsync(Admin, "P@ss0rd").Wait();
                    userManager.AddToRoleAsync(Admin, "Admin").Wait();
                }

                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Seeding Failed : {ex}");
                return false;
            }
        }
    }
}
