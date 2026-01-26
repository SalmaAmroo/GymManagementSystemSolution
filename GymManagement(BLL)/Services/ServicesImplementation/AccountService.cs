using GymManagementBLL.Services.Interfaces;
using GymManagementBLL.ViewModels.AccountViewModels;
using GymManagementDAL.Entities;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagementBLL.Services.ServicesImplementation
{
    public class AccountService : IAccountService
    {
        public AccountService(UserManager<ApplicationUser> userManager) 
        {
            _userManager = userManager;
        }

        public UserManager<ApplicationUser> _userManager { get; }

        public ApplicationUser? ValidateUser(LoginViewModel loginViewModel)
        {
            var User = _userManager.FindByEmailAsync(loginViewModel.Email).Result;
            if (User is null) return null;
            var isPasswordValid = _userManager.CheckPasswordAsync(User, loginViewModel.Password).Result;
            return isPasswordValid ? User : null;

        }
    }
}
