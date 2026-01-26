using AutoMapper;
using GymManagementBLL.Services.Interfaces;
using GymManagementBLL.ViewModels.ServiceViewModels;
using GymManagementBLL.ViewModels.SessionViewModels;
using GymManagementDAL.Entities;
using GymManagementDAL.Repositories.Interfaces;
using GymManagementSystemBLL.ViewModels.SessionViewModels;


namespace GymManagementBLL.Services.ServicesImplementation
{
    public class SessionService : ISessionService
    {
        private readonly IUnitOfWork _UnitOfWork;
        private readonly IMapper _mapper;

        public SessionService(IUnitOfWork UnitOfWork , IMapper mapper)
        {
            _UnitOfWork = UnitOfWork;
            _mapper = mapper;
        }

        public bool CreateSession(CreateSessionViewModel CreatedSession)
        {
            try
            {
                if (!IsTrainerExists(CreatedSession.TrainerId) || !IsCategoryExists(CreatedSession.CategoryId) || !IsDateTimeValid(CreatedSession.StartDate, CreatedSession.EndDate))
                    return false;

                if (CreatedSession.Capacity < 0 || CreatedSession.Capacity > 25)
                    return false;
                var MappedSession = _mapper.Map<Session>(CreatedSession);
                _UnitOfWork.GetRepository<Session>().Add(MappedSession);
                return _UnitOfWork.SaveChanges() > 0;
            }
            catch(Exception ex)
            { 
              Console.WriteLine($"Create Session Failed : {ex.Message}");
                return false;
            }


        }

        public IEnumerable<SessionViewModel> GetAllSessions()
        {
            var Sessions = _UnitOfWork.SessionRepository.GetAllSessionsWithTrainerAndCategory();
            if (!Sessions.Any()) return [];
            var MappedSessions = _mapper.Map<IEnumerable<Session>,IEnumerable<SessionViewModel>>(Sessions);
            foreach (var session in MappedSessions)
            {
                session.AvailableSlots = session.Capacity - _UnitOfWork.SessionRepository.GetCountOfBookedSlots(session.Id);
            }
            return MappedSessions;
        }

        public SessionViewModel? GetSessionById(int id)
        {
            var Session = _UnitOfWork.SessionRepository.GetSessionWithTrainerAndCategory(id);
            if (Session is null) return null;
            var MappedSession = _mapper.Map<Session, SessionViewModel>(Session);
            MappedSession.AvailableSlots = MappedSession.Capacity - _UnitOfWork.SessionRepository.GetCountOfBookedSlots(MappedSession.Id);
            return MappedSession;
        }

        public UpdateSessionViewModel? GetSessionToUpdate(int id)
        {
            var Session = _UnitOfWork.SessionRepository.GetById(id);
            if (!IsSessionAvailableForUpdating(Session!)) return null;

            return _mapper.Map<UpdateSessionViewModel>(Session);


        }

        public bool UpdateSession(UpdateSessionViewModel UpdatedSession, int id)
        {
            try
            {
                var Session = _UnitOfWork.SessionRepository.GetById(id);
                if (!IsSessionAvailableForUpdating(Session!)) return false;
                if (!IsTrainerExists(UpdatedSession.TrainerId)) return false;
                if (!IsDateTimeValid(UpdatedSession.StartDate, UpdatedSession.EndDate)) return false;

                _mapper.Map(UpdatedSession, Session);
                Session!.UpdatedAt = DateTime.Now;
                _UnitOfWork.SessionRepository.Update(Session);
                return _UnitOfWork.SaveChanges() > 0;


            }
            catch (Exception ex)
            {
                Console.WriteLine($"Update Session Failed : {ex}");
                return false;
            }

        }
        public bool RemoveSession(int id)
        {
            try 
            {
                var Session = _UnitOfWork.SessionRepository.GetById(id);
                if (!IsSessionAvailableForRemoving(Session!)) return false;
                _UnitOfWork.SessionRepository.Delete(Session!);

                return _UnitOfWork.SaveChanges() > 0;


            
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Remove Session Failed : {ex}");
                return false;
            }
        }

        #region Healper Methods

        private bool IsSessionAvailableForUpdating (Session session)
        {
            if (session is null) return false;
            if (session.EndDate < DateTime.Now) return false;
            if (session.StartDate <= DateTime.Now) return false;
            var HasActiveBooking = _UnitOfWork.SessionRepository.GetCountOfBookedSlots (session.Id) >0;
            if (HasActiveBooking) return false;

            return true;

        }
        private bool IsSessionAvailableForRemoving(Session session)
        {
            if (session is null) return false;
            if (session.StartDate <= DateTime.Now && session.EndDate > DateTime.Now) return false;
            if (session.StartDate > DateTime.Now) return false;
            var HasActiveBooking = _UnitOfWork.SessionRepository.GetCountOfBookedSlots(session.Id) > 0;
            if (HasActiveBooking) return false;

            return true;

        }

        public IEnumerable<TrainerSelectViewModel> GetTrainersForDropDown()
        {
            var Trainers = _UnitOfWork.GetRepository<Trainer>().GetAll();
            return _mapper.Map<IEnumerable<TrainerSelectViewModel>>(Trainers);
        }

        public IEnumerable<CategorySelectViewModel> GetCategoriesForDropDown()
        {
            var Categories = _UnitOfWork.GetRepository<Category>().GetAll();
            return _mapper.Map<IEnumerable<CategorySelectViewModel>>(Categories);
        }



        private bool IsTrainerExists(int TrainerId)
        {
            return _UnitOfWork.GetRepository<Trainer>().GetById(TrainerId) is not null;
        }
        private bool IsCategoryExists(int CategoryId)
        {
            return _UnitOfWork.GetRepository<Category>().GetById(CategoryId) is not null;
        }

        private bool IsDateTimeValid(DateTime StartDate , DateTime EndDate)
        {
            return StartDate < EndDate && DateTime.Now < StartDate;
        }




        #endregion
    }
}
