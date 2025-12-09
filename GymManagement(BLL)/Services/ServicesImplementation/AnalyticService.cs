using GymManagementBLL.Services.Interfaces;
using GymManagementBLL.ViewModels.AnalyticsViewModels;
using GymManagementDAL.Entities;
using GymManagementDAL.Repositories.Interfaces;


namespace GymManagementBLL.Services.ServicesImplementation
{
    public class AnalyticService : IAnalyticService
    {
        private readonly IUnitOfWork _unitOfWork;

        public AnalyticService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public AnalyticsViewModel GetAnalyticsData()
        {
            var Sessions = _unitOfWork.SessionRepository.GetAll();
            return new AnalyticsViewModel
            {
                ActiveMembers = _unitOfWork.GetRepository<MemberShip>().GetAll(MemberShip => MemberShip.Status == "Active").Count(),
                TotalMembers  = _unitOfWork.GetRepository<Member>().GetAll().Count(),
                TotalTrainers = _unitOfWork.GetRepository<Trainer>().GetAll().Count(),
                UpComingSessions = Sessions.Count(session => session.StartDate > DateTime.Now),
                OngoingSessions = Sessions.Count(session => session.StartDate <= DateTime.Now && session.EndDate >= DateTime.Now),
                CompletedTrainers = Sessions.Count(session => session.EndDate < DateTime.Now)

            };
        }
    }
}
