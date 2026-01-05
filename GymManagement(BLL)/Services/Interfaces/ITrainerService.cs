using GymManagementBLL.ViewModels.TrainerViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagementBLL.Services.Interfaces
{
    public interface ITrainerService
    {
        IEnumerable<TrainerViewModel> GetAllTrainers();

        bool CreateTrainer(CreateTrainerViewModel creatingTrainer);

        TrainerViewModel? GetTrainerDetails(int id);

        TrainerToUpdateViewModel? GetTrainerToUpdate(int id);

        bool UpdateTrainer(int id, TrainerToUpdateViewModel trainerToUpdate);

        bool RemoveTrainer(int id);
    }
}
