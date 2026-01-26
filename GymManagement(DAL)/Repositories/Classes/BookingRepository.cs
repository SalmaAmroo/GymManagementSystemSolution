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
    public class BookingRepository :GenericRepository<MemberSession>, IBookingRepository
    {
        private readonly GymDbContext _context;

        public BookingRepository(GymDbContext context):base(context)
        {
            _context = context;
        }

        public IEnumerable<MemberSession> GetSessionsById(int SessionId)
        {
            var MemberSessions = _context.MemberSessions.Where(ms => ms.SessionId == SessionId)
                                                       .Include(ms=>ms.Member)
                                                       .ToList();
            return MemberSessions;
        }
    }
}
