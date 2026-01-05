using GymManagementBLL.Services.Interfaces;
using GymManagementBLL.ViewModels.MemberViewModels;
using Microsoft.AspNetCore.Mvc;

namespace GymManagementPL.Controllers
{
    public class MemberController : Controller
    {
        private readonly IMemberService _memberservice;

        public MemberController(IMemberService memberservice)
        {
            _memberservice = memberservice;
        }
        public ActionResult Index()
        {
            var members = _memberservice.GetAllMembers();
            return View(members);
        }
        public ActionResult MemberDetails(int id)
        {
            if (id <= 0)
            {
                TempData["ErrorMessage"] = "Id Number Of Member Can Not Be 0 Or Negative Number";
                return RedirectToAction(nameof(Index));
            }
            var Member = _memberservice.GetMemberDetails(id);

            if (Member is null)
            {
                TempData["ErrorMessage"] = "Member Not Found";
                return RedirectToAction(nameof(Index));
            }
            return View(Member);
        }
        public ActionResult HealthRecordDetails(int id)
        {
            if (id <= 0)
            {
                TempData["ErrorMessage"] = "Id Number Of Member Can Nor Be 0 Or Negative Number";
                return RedirectToAction(nameof(Index));
            }
            var HealthRecord = _memberservice.GetHealthRecord(id);
            if (HealthRecord is null)
            {
                TempData["ErrorMessage"] = "HealthRecord Not Found";
                return RedirectToAction(nameof(Index));
            }
                return View(HealthRecord);

        }

        #region Create Member
        public ActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Create(CreateMemberViewModel CreatedMember)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("DataInValid", "Check Data And Missing Fields");
                return View(nameof(Create), CreatedMember);
            }
           bool Result= _memberservice.CreateMember(CreatedMember);
            if (Result)
            {
                TempData["SuccessMessage"] = "Member Created Successfully";
            }
            else
            {
                TempData["ErrorMessage"] = "Failed To Create Member , Check Phone And Email";
            }

                return RedirectToAction(nameof(Index));
        }
        #endregion

        #region Edit Member
        public ActionResult MemberEdit (int id)
        {
            if(id<=0)
            {
                TempData["ErrorMessage"] = "Id Number Of Member Can Nor Be 0 Or Negative Number";
                return RedirectToAction(nameof(Index));
            }
            var Member = _memberservice.GetMemberToUpdateViewModel(id);
            if(Member is null)
            {
                TempData["ErrorMessage"] = "Member Not Found";
                return RedirectToAction(nameof(Index));
            }
            return View(Member);
        }

        [HttpPost]
        public ActionResult MemberEdit([FromRoute]int id ,MemberToUpdateViewModel MemberToEdit)
        {
            if(!ModelState.IsValid)
            {
                return View(MemberToEdit);
            }
            var Result = _memberservice.UpdateMember(id, MemberToEdit);
            if (Result)
            {
                TempData["SuccessMessage"] = "Member Updated Successfully";
            }
            else
            {
                TempData["ErrorMessage"] = "Failed To Update Member ";
            }
            return  RedirectToAction(nameof(Index));
        }
        #endregion

        #region Delete Member
        public ActionResult DeleteMember(int id)
        {
            if (id <= 0)
            {
                TempData["ErrorMessage"] = "Id Number Of Member Can Nor Be 0 Or Negative Number";
                return RedirectToAction(nameof(Index));
            }
            var Member = _memberservice.GetMemberDetails(id);
            if (Member is null)
            {
                TempData["SuccessMessage"] = "Member Not Found";
                return RedirectToAction(nameof(Index));
            }
            ViewBag.MemberId = Member.Id;
            return View();
        }
        [HttpPost]
        public ActionResult DeleteConfirmed([FromForm] int id)
        {
            var Result = _memberservice.RemoveMember(id);
            if (Result)
            {
                TempData["SuccessMessage"] = "Member Deleted Successfully";
            }
            else
            {
                TempData["ErrorMessage"] = "Failed To Delete Member ";
            }
            return RedirectToAction(nameof(Index));
        }
        #endregion
    }
}
    
