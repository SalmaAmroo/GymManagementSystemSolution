using GymManagementBLL.ViewModels.BookingViewModel;
using GymManagementBLL.ViewModels.MemberShipViewModels;
using GymManagementBLL.ViewModels.ServiceViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagementBLL.Services.Interfaces
{
    public interface IBookingSsrvice
    {
        IEnumerable<SessionViewModel> GetSessionsWithTrainerAndCategories();
        IEnumerable<MemberForSessionViewModel> GetMembersBookingSessions(int Id);
        public bool CreateBooking(CreateBookingViewModel model);
        IEnumerable<NameForSelectListViewModel> GetMemberForDropdown(int Id);
        public bool Cancel(MemberForSessionViewModel model);



    }
}
