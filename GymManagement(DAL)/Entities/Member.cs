using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagementDAL.Entities
{
    public class Member : GymUser
    {
        // Join Date == CreatedAt 
        public string Photo { get; set; } = null!;

        #region Realationships

        #region Member - HealthRecord (1-1)
        public HealthRecord HealthRecord { get; set; } = null!;
        #endregion

        #region Member - MemberShip
        public ICollection<MemberShip> MemberShips { get; set; } = null!;
        #endregion

        #region Member - Session (Many to Many)
        public ICollection<MemberSession> MemberSessions { get; set; } = null!;
        #endregion



        #endregion
    }
}
