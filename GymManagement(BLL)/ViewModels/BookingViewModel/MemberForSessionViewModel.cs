using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagementBLL.ViewModels.BookingViewModel
{
    public class MemberForSessionViewModel
    {
        public string MemberName { get; set; }
        public int MemberId { get; set; }
        public bool IsAttended { get; set; }
        public string BookingDate { get; set; }
        public int SessionId { get; set; }
    }
}
