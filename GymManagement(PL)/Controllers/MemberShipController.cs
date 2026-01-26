using GymManagementBLL.Services.Interfaces;
using GymManagementBLL.Services.ServicesImplementation;
using GymManagementBLL.ViewModels.MemberShipViewModels;
using GymManagementDAL.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace GymManagementPL.Controllers
{
    public class MemberShipController : Controller
    {
        private readonly IMemberShipService _memberShipService;

        public MemberShipController(IMemberShipService memberShipService)
        {
            _memberShipService = memberShipService;
        }
        public ActionResult Index()
        {
            var memberShips = _memberShipService.GetAllMemberShips();
            return View(memberShips);
        }
        public ActionResult Create()
        {
            LoadDropDownLists();

            return View();
        }
        [HttpPost]
        public ActionResult Create(CreateMemberShipViewModel model)
        {
            if (!ModelState.IsValid)
            {
                LoadDropDownLists();
                return View(model);
            }
            bool Result = _memberShipService.CreateMemberShip(model);
            if (Result)
            {
                TempData["SuccessMessage"] = "Member Created Successfully";
            }
            else
            {
                TempData["ErrorMessage"] = "Failed To Create MemberShip";
            }

            return RedirectToAction(nameof(Index));

        }
        public ActionResult Cancle(int Id)
        {
            var Result = _memberShipService.DeleteMemberShip(Id);
            if (Result)
            {
                TempData["SuccessMessage"] = "Membership Cancled Successfully";
                return RedirectToAction(nameof(Index));
            }
            else
            {
                TempData["ErrorMessage"] = "Failed To Cancle Membership ";
                return RedirectToAction(nameof(Index));

            }

        }
        #region HelperMethods
        public void LoadDropDownLists()
        {
            var plans = _memberShipService.GetPlanForSelectListViewModels();
            ViewBag.Plans = new SelectList(plans, "Id", "Name");
            var members = _memberShipService.GetMemberForSelectListViewModels();
            ViewBag.Members = new SelectList(members, "Id", "Name");
        }
        #endregion

    }
}
