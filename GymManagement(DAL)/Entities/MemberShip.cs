using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagementDAL.Entities
{
    public class MemberShip :BaseEntity
    {
        // StartDate = CreatedAt of BaseEntity

        public DateTime EndDate { get; set; }

        // Readonly property to check if the membership is active

        public string Status
        {
            get
            {
                return EndDate >= DateTime.Now ? "Active" : "Expired";
            }
        }
        public int MemberId { get; set; }
        public Member Member { get; set; } = null!;

        public int PlanId { get; set; }
        public Plan Plan { get; set; } = null!;

    }
}
