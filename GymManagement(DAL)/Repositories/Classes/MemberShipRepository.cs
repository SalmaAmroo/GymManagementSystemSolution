using GymManagement_DAL_.Data.DbContexts;
using GymManagementDAL.Entities;
using GymManagementDAL.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagementDAL.Repositories.Classes
{
    public class MemberShipRepository : GenericRepository<MemberShip>,IMemberShipRepository
    {
        private readonly GymDbContext _gymDbContext;

        public MemberShipRepository(GymDbContext gymDbContext):base(gymDbContext)
        {
            _gymDbContext = gymDbContext;
        }

        public MemberShip? GetFirstorDefault(Func<MemberShip, bool>? filter = null)
        {
            var memberShip = _gymDbContext.MemberShips.FirstOrDefault(filter??(_=>true));
            return memberShip;
        }

        IEnumerable<MemberShip> IMemberShipRepository.GetAllMemberShipsWithMembersAndPlans(Func<MemberShip, bool>? filter)
        {
            var MemberShips = _gymDbContext.MemberShips
                                           .Include(ms => ms.Member)
                                           .Include(ms => ms.Plan)
                                           .Where(filter ?? (_ => true));
            return MemberShips;
        }
    }
}
