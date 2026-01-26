

using GymManagementDAL.Entities;

namespace GymManagementDAL.Repositories.Interfaces
{
    public interface IMemberShipRepository : IGenericRepository<MemberShip>
    {
        IEnumerable<MemberShip> GetAllMemberShipsWithMembersAndPlans(Func<MemberShip,bool>?filter = null);

        MemberShip? GetFirstorDefault(Func<MemberShip, bool>? filter = null);
    }
}
