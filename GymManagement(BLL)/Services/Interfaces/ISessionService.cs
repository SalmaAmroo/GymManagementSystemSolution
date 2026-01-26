using GymManagementBLL.ViewModels.ServiceViewModels;
using GymManagementBLL.ViewModels.SessionViewModels;
using GymManagementSystemBLL.ViewModels.SessionViewModels;


namespace GymManagementBLL.Services.Interfaces
{
    public interface ISessionService
    {
        IEnumerable <SessionViewModel> GetAllSessions();

        SessionViewModel? GetSessionById(int id);
        bool CreateSession(CreateSessionViewModel CreatedSession);
        UpdateSessionViewModel? GetSessionToUpdate(int id);

        bool UpdateSession (UpdateSessionViewModel UpdatedSession , int id);

        bool RemoveSession(int id);
        IEnumerable<TrainerSelectViewModel> GetTrainersForDropDown();

        IEnumerable<CategorySelectViewModel> GetCategoriesForDropDown();





    }
}
