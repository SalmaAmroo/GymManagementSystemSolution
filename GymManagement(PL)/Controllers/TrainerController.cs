using GymManagementBLL.Services.Interfaces;
using GymManagementBLL.Services.ServicesImplementation;
using GymManagementBLL.ViewModels.MemberViewModels;
using GymManagementBLL.ViewModels.TrainerViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
namespace GymManagementPL.Controllers
{

    [Authorize(Roles ="SuperAdmin")]
    public class TrainerController : Controller
    {
        private readonly ITrainerService _trainerService;

        public TrainerController(ITrainerService trainerService)
        {
            _trainerService = trainerService;
        }
        public ActionResult Index()
        {
            var trainers = _trainerService.GetAllTrainers();
            return View(trainers);
        }
        public ActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Create(CreateTrainerViewModel creatingTrainer)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("DataInValid", "Check Data And Missing Fields");
                return View(nameof(Create), creatingTrainer);
            }
            bool Result = _trainerService.CreateTrainer(creatingTrainer);
            if (Result)
            {
                TempData["SuccessMessage"] = "Trainer Created Successfully";
            }
            else
            {
                TempData["ErrorMessage"] = "Failed To Create Trainer , Check Phone And Email";
            }

            return RedirectToAction(nameof(Index));
        }

        public ActionResult Details(int id)
        {
            if (id <= 0)
            {
                TempData["ErrorMessage"] = "Id Number Of Trainer Can Not Be 0 Or Negative Number";
                return RedirectToAction(nameof(Index));
            }
            var Trainer = _trainerService.GetTrainerDetails(id);

            if (Trainer is null)
            {
                TempData["ErrorMessage"] = "Trainer Not Found";
                return RedirectToAction(nameof(Index));
            }
            return View(Trainer);
        }

        public ActionResult Edit(int id)
        {
            if (id <= 0)
            {
                TempData["ErrorMessage"] = "Id Number Of Trainer Can Not Be 0 Or Negative Number";
                return RedirectToAction(nameof(Index));
            }
            var Trainer = _trainerService.GetTrainerToUpdate(id);
            if (Trainer is null)
            {
                TempData["ErrorMessage"] = "Trainer Not Found";
                return RedirectToAction(nameof(Index));
            }
            return View(Trainer);
        }

        [HttpPost]
        public ActionResult Edit([FromRoute] int id, TrainerToUpdateViewModel TrainerToEdit)
        {
            if (!ModelState.IsValid)
            {
                return View(TrainerToEdit);
            }
            var Result = _trainerService.UpdateTrainer(id, TrainerToEdit);
            if (Result)
            {
                TempData["SuccessMessage"] = "Trainer Updated Successfully";
            }
            else
            {
                TempData["ErrorMessage"] = "Failed To Update Trainer ";
            }
            return RedirectToAction(nameof(Index));
        }
        public ActionResult Delete(int id)
        {
            if (id <= 0)
            {
                TempData["ErrorMessage"] = "Id Number Of Trainer Can Not Be 0 Or Negative Number";
                return RedirectToAction(nameof(Index));
            }
            var Trainer = _trainerService.GetTrainerDetails(id);
            if (Trainer is null)
            {
                TempData["SuccessMessage"] = "Trainer Not Found";
                return RedirectToAction(nameof(Index));
            }
            ViewBag.TrainerID = Trainer.Id;
            return View();
        }
        [HttpPost]
        public ActionResult DeleteConfirmed([FromForm] int id)
        {
            var Result = _trainerService.RemoveTrainer(id);
            if (Result)
            {
                TempData["SuccessMessage"] = "Trainer Deleted Successfully";
            }
            else
            {
                TempData["ErrorMessage"] = "Failed To Delete Trainer ";
            }
            return RedirectToAction(nameof(Index));
        }
    }
}
