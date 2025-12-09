using GymManagement_DAL_.Data.DbContexts;
using GymManagementDAL.Entities;
using GymManagementDAL.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace GymManagementDAL.Repositories.Classes
{
    public class SessionRepository : GenericRepository<Session>, ISessionRepository
    {
        private readonly GymDbContext _dbContext;

        public SessionRepository(GymDbContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }
        public IEnumerable<Session> GetAllSessionsWithTrainerAndCategory()
        {
            return _dbContext.Sessions
                .Include(s => s.TrainerSession)
                .Include(s => s.SessionCategory)
                .ToList();
        }

        public int GetCountOfBookedSlots(int sessionId)
        {
            return _dbContext.MemberSessions.Count(ms => ms.SessionId == sessionId);
        }

        public Session? GetSessionWithTrainerAndCategory(int id)
        {
            return _dbContext.Sessions
                .Include(s => s.TrainerSession)
                .Include(s => s.SessionCategory)
                .FirstOrDefault(s => s.Id == id);
        }
    }
}
