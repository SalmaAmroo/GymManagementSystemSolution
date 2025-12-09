using GymManagementBLL.Services.Interfaces;
using GymManagementBLL.ViewModels.TrainerViewModels;
using GymManagementDAL.Entities;
using GymManagementDAL.Repositories.Interfaces;


namespace GymManagementBLL.Services.ServicesImplementation
{
    internal class TrainerService : ITrainerService
    {
        private readonly IUnitOfWork _UnitOfWork;

        public TrainerService(IUnitOfWork UnitOfWork)
        {
            _UnitOfWork = UnitOfWork;
        }
        public bool CreateTrainer(CreateTrainerViewModel creatingTrainer)
        {
            try
            {
                if (IsEmailExists(creatingTrainer.Email) || IsPhoneExists(creatingTrainer.Phone)) return false;
                Trainer trainer = new Trainer()
                {
                    Name = creatingTrainer.Name,
                    Email = creatingTrainer.Email,
                    PhoneNumber = creatingTrainer.Phone,
                    DateOfBirth = creatingTrainer.DateOfBirth,
                    Gender = creatingTrainer.Gender,
                    Address = new Address()
                    {
                        BuildNumber = creatingTrainer.BuildNumber,
                        City = creatingTrainer.City,
                        Street = creatingTrainer.Street
                    },
                    Specialtyies = creatingTrainer.Specialties
                };
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
            if (Trainers is null || Trainers.Any()) return [];
            var TrainerViewModels = Trainers.Select(Trainer => new TrainerViewModel()
            {
                Name = Trainer.Name,
                Email = Trainer.Email,
                Phone = Trainer.PhoneNumber,
                Specialty = Trainer.Specialtyies.ToString()

            });
            return TrainerViewModels;
        }
        public TrainerViewModel? GetTrainerDetails(int id)
        {
            var Trainer = _UnitOfWork.GetRepository<Trainer>().GetById(id);
            if (Trainer is null) return null;

            return new TrainerViewModel()
            {
                Name = Trainer.Name,
                Email = Trainer.Email,
                Phone = Trainer.PhoneNumber,
                Specialty = Trainer.Specialtyies.ToString()
            };
        }

        public TrainerToUpdateViewModel? GetTrainerToUpdate(int id)
        {
            var Trainer = _UnitOfWork.GetRepository<Trainer>().GetById(id);
            if (Trainer is null) return null;
            return new TrainerToUpdateViewModel()
            {
                Name = Trainer.Name,
                Email = Trainer.Email,
                Phone = Trainer.PhoneNumber,
                BuildNumber = Trainer.Address.BuildNumber,
                Street = Trainer.Address.Street,
                City = Trainer.Address.City,
                Specialties = Trainer.Specialtyies
            };
        }

        public bool UpdateTrainer(int id, TrainerToUpdateViewModel trainerToUpdate)
        {
            try
            {
                if (IsEmailExists(trainerToUpdate.Email) || IsPhoneExists(trainerToUpdate.Phone)) return false;
                var TrainerRepo = _UnitOfWork.GetRepository<Trainer>();
                var Trainer = TrainerRepo.GetById(id);
                if (Trainer is null) return false;
                Trainer.Email = trainerToUpdate.Email;
                Trainer.PhoneNumber = trainerToUpdate.Phone;
                Trainer.Address.BuildNumber = trainerToUpdate.BuildNumber;
                Trainer.Address.Street = trainerToUpdate.Street;
                Trainer.Address.City = trainerToUpdate.City;
                Trainer.Specialtyies = trainerToUpdate.Specialties;
                Trainer.UpdatedAt = DateTime.Now;
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
