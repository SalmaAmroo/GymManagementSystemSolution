using GymManagementBLL.Services.Interfaces;
using GymManagementBLL.ViewModels.AccountViewModels;
using GymManagementDAL.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace GymManagementPL.Controllers
{
    public class AccountController : Controller
    {
        private readonly IAccountService _accountService;
        private readonly SignInManager<ApplicationUser> _signInManager;

        public AccountController(IAccountService accountService, SignInManager<ApplicationUser> signInManager )
        {
            _accountService = accountService;
            _signInManager = signInManager;
        }
        public ActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Login(LoginViewModel model)
        {
            if (!ModelState.IsValid) return View(model);
            var User = _accountService.ValidateUser(model);
            if (User is null)
            {
                ModelState.AddModelError("InvalidLogin" , "Invalid Email Or Password");
                return View(model);
            }

            var Result =_signInManager.PasswordSignInAsync(User ,model.Password , model.RememberMe ,false ).Result;
            if (Result.IsNotAllowed)
                ModelState.AddModelError("NotAllowed" , "Your Account Is Not Allowed To Login");
            if(Result.IsLockedOut)
                ModelState.AddModelError("LockedOut" , "Your Account Is Locked Out");
            if (Result.Succeeded)
                return RedirectToAction("Index" , "Home");

            return View(model);
        }
        [HttpPost]
        public ActionResult Logout()
        {
            _signInManager.SignOutAsync().GetAwaiter().GetResult();
            return RedirectToAction(nameof(Login));
        }
       
        public ActionResult AccessDined()
        {
            return View();
        }
    }
}
