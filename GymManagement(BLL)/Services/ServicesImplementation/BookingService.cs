using AutoMapper;
using GymManagementBLL.Services.Interfaces;
using GymManagementBLL.ViewModels.BookingViewModel;
using GymManagementBLL.ViewModels.MemberShipViewModels;
using GymManagementBLL.ViewModels.ServiceViewModels;
using GymManagementDAL.Entities;
using GymManagementDAL.Repositories.Interfaces;
namespace GymManagementBLL.Services.ServicesImplementation
{
    public class BookingService : IBookingSsrvice
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public BookingService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public bool CreateBooking(CreateBookingViewModel model)
        {
            var Session = _unitOfWork.SessionRepository.GetById(model.SessionId);
            if (Session is null || Session.StartDate <= DateTime.UtcNow) return false;
            var MemberShipRepo = _unitOfWork.MemberShipRepository;
            var ActiveMemberShip = MemberShipRepo.GetFirstorDefault(ms => ms.MemberId == model.MemberId && ms.Status.ToLower() == "active");
            if (ActiveMemberShip is null) return false;
            var SessionRepo = _unitOfWork.SessionRepository;
            var BookedSlots = SessionRepo.GetCountOfBookedSlots(model.SessionId);

            var AvailableSlots = Session.Capacity - BookedSlots;
            if (AvailableSlots == 0) return false;

            var booking = _mapper.Map<MemberSession>(model);
            booking.IsAttended = false;

            _unitOfWork.BookingRepository.Add(booking);
            return _unitOfWork.SaveChanges() > 0;
        }

        public IEnumerable<MemberForSessionViewModel> GetMembersBookingSessions(int Id)
        {
            var BookingRepo = _unitOfWork.BookingRepository;
            var MembersForSeesion = BookingRepo.GetSessionsById(Id);
            var MemberForSessionViewModels = _mapper.Map<IEnumerable<MemberForSessionViewModel>>(MembersForSeesion);
            return MemberForSessionViewModels;

        }

        public IEnumerable<SessionViewModel> GetSessionsWithTrainerAndCategories()
        {
            var ISessionRepository = _unitOfWork.SessionRepository;
            var sessions = ISessionRepository.GetAllSessionsWithTrainerAndCategory();
            var SessionViewModels = _mapper.Map<IEnumerable<SessionViewModel>>(sessions);
            foreach (var session in SessionViewModels)
            {
                session.AvailableSlots = session.Capacity - ISessionRepository.GetCountOfBookedSlots(session.Id);
            }
            return SessionViewModels;

        }

        public bool Cancel(MemberForSessionViewModel model)
        {
            try
            {
                var session = _unitOfWork.SessionRepository.GetById(model.SessionId);
                if (session is null || session.StartDate <= DateTime.Now) return false;

                var Booking = _unitOfWork.BookingRepository.GetAll(X => X.MemberId == model.MemberId && X.SessionId == model.SessionId)
                                                           .FirstOrDefault();
                if (Booking is null) return false;
                _unitOfWork.BookingRepository.Delete(Booking);
                return _unitOfWork.SaveChanges() > 0;
            }
            catch
            {
                return false;
            }
        }
        #region Helper Methods
        public IEnumerable<NameForSelectListViewModel> GetMemberForDropdown(int Id)
        {
            var bookingRepo = _unitOfWork.BookingRepository;
            var bookedMembers = bookingRepo.GetAll(s => s.Id == Id)
                                            .Select(ms => ms.MemberId)
                                            .ToList();
            var membersAvailableForBooking = _unitOfWork.GetRepository<Member>()
                                                .GetAll(m => !bookedMembers.Contains(m.Id))
                                                .ToList();

            var memberForSelectList = _mapper.Map<IEnumerable<NameForSelectListViewModel>>(membersAvailableForBooking);
            return memberForSelectList;
        }


        #endregion
    }
}
