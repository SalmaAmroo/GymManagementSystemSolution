using GymManagementBLL.Services.Interfaces;
using GymManagementBLL.Services.ServicesImplementation;
using GymManagementBLL.ViewModels.BookingViewModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace GymManagementPL.Controllers
{
    public class BookingController : Controller
    {
        public BookingController(IBookingSsrvice bookingSsrvice)
        {
            _bookingSsrvice = bookingSsrvice;
        }

        public IBookingSsrvice _bookingSsrvice { get; }

        public ActionResult Index()
        {
            var MemberSssions = _bookingSsrvice.GetSessionsWithTrainerAndCategories();
            return View(MemberSssions);
        }
        public ActionResult GetMembersForUpcomingSession(int Id)
        {
            var MembersForSession = _bookingSsrvice.GetMembersBookingSessions(Id);
            return View(MembersForSession);
        }
        public ActionResult GetMembersForOngoingSession(int Id)
        {
            var MembersForSession = _bookingSsrvice.GetMembersBookingSessions(Id);
            return View(MembersForSession);
        }
        public ActionResult Create(int id)
        {
            var MembersForDropdown = _bookingSsrvice.GetMemberForDropdown(id);
            var MemberSelectList = new SelectList(MembersForDropdown, "Id", "Name");
            ViewBag.Members = MemberSelectList;
            return View();
        }
        [HttpPost]
        public ActionResult Create(CreateBookingViewModel model)
        {
            var result = _bookingSsrvice.CreateBooking(model);
            if (result)
            {
                TempData["SuccessMessage"] = "Booking created successfully.";

            }
            else
            {
                TempData["ErrorMessage"] = "Failed to create booking.";
            }
            return RedirectToAction(nameof(GetMembersForUpcomingSession), new { id = model.SessionId });
        }
        [HttpPost]
        public ActionResult Cancel(MemberForSessionViewModel model)
        {
            var Result = _bookingSsrvice.Cancel(model);
            if (Result)
            {
                TempData["SuccessMessage"] = "Booking Cancled Successfully";

            }
            else
            {
                TempData["ErrorMessage"] = "Failed To Cancle Booking ";

            }
            return RedirectToAction(nameof(GetMembersForUpcomingSession), new { id = model.SessionId });

        }
    }
}
