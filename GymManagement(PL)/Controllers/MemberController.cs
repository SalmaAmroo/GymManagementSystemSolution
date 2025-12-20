using GymManagementBLL.Services.Interfaces;
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
            if(id==0) 
                return RedirectToAction(nameof(Index));

            var Member = _memberservice.GetMemberDetails(id);

            if(Member is null)
                return RedirectToAction(nameof(Index));

            return View(Member);
        }
    }
}
    
