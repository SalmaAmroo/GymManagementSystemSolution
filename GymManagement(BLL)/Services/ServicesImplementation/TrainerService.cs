using AutoMapper;
using GymManagementBLL.Services.Interfaces;
using GymManagementBLL.ViewModels.TrainerViewModels;
using GymManagementDAL.Entities;
using GymManagementDAL.Repositories.Interfaces;


namespace GymManagementBLL.Services.ServicesImplementation
{
    public class TrainerService : ITrainerService
    {
        private readonly IUnitOfWork _UnitOfWork;
        private readonly IMapper _mapper;

        public TrainerService(IUnitOfWork UnitOfWork , IMapper mapper)
        {
            _UnitOfWork = UnitOfWork;
            _mapper = mapper;
        }
        public bool CreateTrainer(CreateTrainerViewModel creatingTrainer)
        {
            try
            {
                if (IsEmailExists(creatingTrainer.Email) || IsPhoneExists(creatingTrainer.Phone)) return false;
                Trainer trainer = _mapper.Map<Trainer>(creatingTrainer);
                _UnitOfWork.GetRepository<Trainer>().Add(trainer);
                return _UnitOfWork.SaveChanges() > 0;
            }
            catch (Exception)
            {
                return false;
            }
        }
        public IEnumerable<TrainerViewModel> GetAllTrainers()
        {
            var Trainers = _UnitOfWork.GetRepository<Trainer>().GetAll();
            if (Trainers is null || !Trainers.Any()) return [];
            var TrainerViewModels = _mapper.Map<IEnumerable<TrainerViewModel>>(Trainers);
            return TrainerViewModels;
        }
        public TrainerViewModel? GetTrainerDetails(int id)
        {
            var Trainer = _UnitOfWork.GetRepository<Trainer>().GetById(id);
            if (Trainer is null) return null;

            return _mapper.Map<TrainerViewModel>(Trainer);
        }

        public TrainerToUpdateViewModel? GetTrainerToUpdate(int id)
        {
            var Trainer = _UnitOfWork.GetRepository<Trainer>().GetById(id);
            if (Trainer is null) return null;
            return _mapper.Map<TrainerToUpdateViewModel>(Trainer);
        }

        public bool UpdateTrainer(int id, TrainerToUpdateViewModel trainerToUpdate)
        {
            try
            {
                if (IsEmailExists(trainerToUpdate.Email) || IsPhoneExists(trainerToUpdate.Phone)) return false;
                var TrainerRepo = _UnitOfWork.GetRepository<Trainer>();
                var Trainer = TrainerRepo.GetById(id);
                if (Trainer is null) return false;
                _mapper.Map(trainerToUpdate,Trainer);
                TrainerRepo.Update(Trainer);
                return _UnitOfWork.SaveChanges() > 0;
            }
            catch (Exception)
            {
                return false;
            }
        }
        public bool RemoveTrainer(int id)
        {
            var TrainerRepo = _UnitOfWork.GetRepository<Trainer>();
            var Trainer = TrainerRepo.GetById(id);
            if (Trainer is null) return false;
            var HasActiveTrainerSessions = _UnitOfWork.GetRepository<Session>().GetAll(x => x.TrainerId == id && x.TrainerSession.CreatedAt > DateTime.Now).Any();
            if (Trainer is null || HasActiveTrainerSessions) return false;
            _UnitOfWork.GetRepository<Trainer>().Delete(Trainer);
            return _UnitOfWork.SaveChanges() > 0;
        }


        #region Helper Methods

        private bool IsEmailExists(string email)
        {
            return _UnitOfWork.GetRepository<Member>().GetAll(x => x.Email == email).Any();
        }
        private bool IsPhoneExists(string Phone)
        {
            return _UnitOfWork.GetRepository<Member>().GetAll(x => x.PhoneNumber == Phone).Any();
        }
        #endregion
    }
}
