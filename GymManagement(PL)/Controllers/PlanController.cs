using Microsoft.AspNetCore.Mvc;
using GymManagementBLL.Services.Interfaces;
using GymManagementBLL.ViewModels.PlanViewModels;
using Microsoft.AspNetCore.Authorization;
namespace GymManagementPL.Controllers
{
    [Authorize]
    public class PlanController : Controller
    {
        private readonly IPlanService _planService;

        // GetAllPlans
        public PlanController(IPlanService planService)
        {
            _planService = planService;
        }
        public ActionResult Index()
        {
            var Plans = _planService.GetAllPlans();
            return View(Plans);
        }
        public ActionResult Details(int id)
        { 
            if(id<=0)
            {
                TempData["ErrorMessage"] = "Invalid Plan ID.";
                return RedirectToAction(nameof(Index));
            }
         var Plan = _planService.GetPlanById(id);
            if(Plan is null)
            {
                    TempData["ErrorMessage"] = "Plan not found.";
                    return RedirectToAction(nameof(Index));
            }
            return View(Plan);
        }
        public ActionResult Edit(int id)
        {
            if (id <= 0)
            {
                TempData["ErrorMessage"] = "Invalid Plan ID.";
                return RedirectToAction(nameof(Index));
            }
            var plan = _planService.GetPlanForUpdate(id);
            if (plan is null)
            {
                TempData["ErrorMessage"] = "Plan Can Not Be Updated ";
                return RedirectToAction(nameof(Index));
            }
            return View(plan);

        }
        [HttpPost]
        public ActionResult Edit([FromRoute] int id, UpdatePlanViewModel updatedPlan)
        { 
            if(!ModelState.IsValid)
            {
                ModelState.AddModelError("WrongData", "Check Data Validation ..!");
                return View(updatedPlan);
            }
            var isUpdated = _planService.UpdatePlan(id, updatedPlan);
            if(isUpdated)
            {
                TempData["SuccessMessage"] = "Plan  Updated ";
            }
            else
            {
                TempData["ErrorMessage"] = "Plan  Not Updated ";
            }
            return RedirectToAction(nameof(Index));

        }

        [HttpPost]
        public ActionResult Activate (int id)
        {
            var Result = _planService.TogglePlanStatus(id);
            if (Result)
            {
                TempData["SuccessMessage"] = "Plan Status Toggled Successfully.";
            }
            else
            {
                TempData["ErrorMessage"] = "Failed to Toggle Plan Status.";
            }
            return RedirectToAction(nameof(Index));
        }


    }
}
