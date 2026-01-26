using GymManagementBLL.Services.Interfaces;
using GymManagementSystemBLL.ViewModels.SessionViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace GymManagementPL.Controllers
{
    [Authorize]
    public class SessionController : Controller
    {
        private readonly ISessionService _sessionService;

        public SessionController(ISessionService sessionService)
        {
            _sessionService = sessionService;
        }
        public ActionResult Index()
        {
            var Sessions = _sessionService.GetAllSessions();
            return View(Sessions);
        }
        public ActionResult Details(int id)
        {
            if (id <= 0)
            {
                TempData["ErrorMessage"] = "Invalid Session Id";
                return RedirectToAction("Index");
            }
            var session = _sessionService.GetSessionById(id);
            if (session is null)
            {
                TempData["ErrorMessage"] = "Session Not Found";
                return RedirectToAction("Index");
            }
            return View(session);
        }
        public ActionResult Create() 
        {
            LoadDropDownsForTrainers();
            LoadDropDownsForCategories();
            return View(); 
        }
        [HttpPost]
        public ActionResult create(CreateSessionViewModel CreatedSession)
        {
            if(!ModelState.IsValid)
            {
                LoadDropDownsForCategories();
                LoadDropDownsForTrainers();
                return View(CreatedSession);
            }
            var Result = _sessionService.CreateSession(CreatedSession);
            if(Result)
            {
                TempData["SuccessMessage"] = "Session Create Successfully";
                return RedirectToAction(nameof(Index));

            }
            else
            {
                TempData["ErrorMessage"] = "Failed to Create Session ";
                LoadDropDownsForCategories();
                LoadDropDownsForTrainers();
                return View(CreatedSession);
            }
        }

        public ActionResult Edit (int id)
        {
            if (id <= 0)
            {
                TempData["ErrorMessage"] = "Invalid Session Id";
                return RedirectToAction("Index");
            }
            var session = _sessionService.GetSessionToUpdate(id);
            if(session is null)
            {
                TempData["ErrorMessage"] = "Session Can Not Be Updated";
                return RedirectToAction("Index");
            }
            LoadDropDownsForTrainers();
            return View(session);
        }
        [HttpPost]
        public ActionResult Edit([FromRoute]int id ,UpdateSessionViewModel UpdatedSession)
        {
            if(!ModelState.IsValid)
            {
                LoadDropDownsForTrainers();
                return View(UpdatedSession);
            }
            var Result = _sessionService.UpdateSession(UpdatedSession,id);
            if(Result)
            {
                TempData["SuccessMessage"] = "Session Updated Successfully";
            }
            else
            {
                TempData["ErrorMessage"] = "Failed to Update Session ";
            }
            return RedirectToAction(nameof(Index));
        }

        public ActionResult Delete(int id)
        {
            if (id <= 0)
            {
                TempData["ErrorMessage"] = "Invalid Session Id";
                return RedirectToAction("Index");
            }
            var Session = _sessionService.GetSessionById(id);
            if (Session is null)
            {
                TempData["ErrorMessage"] = "Session Not Found";
            }
            ViewBag.SessionId = Session.Id;
            return View();
        }
        [HttpPost]
        public ActionResult DeleteConfirmed(int id)
        {
            var Result = _sessionService.RemoveSession(id);
            if (Result)
            {
                TempData["SuccessMessage"] = "Session Deleted Successfully";
            }
            else
            {
                TempData["ErrorMessage"] = "Failed to Delete Session ";
            }
            return RedirectToAction(nameof(Index)); 
        }


        #region HelperMethods
        private void LoadDropDownsForTrainers()
        {
            var trainers = _sessionService.GetTrainersForDropDown();
            ViewBag.trainers = new SelectList(trainers, "Id", "Name");
        }
        private void LoadDropDownsForCategories()
        {
            var categories = _sessionService.GetCategoriesForDropDown();
            ViewBag.Categories = new SelectList(categories, "Id", "Name");
        }
        #endregion
    }
}
