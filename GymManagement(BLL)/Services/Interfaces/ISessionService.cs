using GymManagementBLL.ViewModels.ServiceViewModels;
using GymManagementSystemBLL.ViewModels.SessionViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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






    }
}
