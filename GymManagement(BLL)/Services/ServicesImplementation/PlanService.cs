using GymManagementBLL.Services.Interfaces;
using GymManagementBLL.ViewModels.PlanViewModels;
using GymManagementDAL.Entities;
using GymManagementDAL.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagementBLL.Services.ServicesImplementation
{
    internal class PlanService : IPlanService
    {
        private readonly IUnitOfWork _unitOfWork;

        public PlanService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public IEnumerable<PlanViewModel> GetAllPlans()
        {
            var Plans = _unitOfWork.GetRepository<Plan>().GetAll();
            if (Plans is null || Plans.Any()) return [];
            return Plans.Select(p => new PlanViewModel
            {
                Id = p.Id,
                Name = p.Name,
                Description = p.Description,
                Price = p.Price,
                DurationDays = p.DurationDays,
                IsActive = p.IsActive
            });

        }

        public PlanViewModel? GetPlanById(int id)
        {
            var Plan = _unitOfWork.GetRepository<Plan>().GetById(id);
            if (Plan is null) return null;
            return new PlanViewModel
            {
                Id = Plan.Id,
                Name = Plan.Name,
                Description = Plan.Description,
                Price = Plan.Price,
                DurationDays = Plan.DurationDays,
                IsActive = Plan.IsActive
            };

        }

        public UpdatePlanViewModel? GetPlanForUpdate(int id)
        {
            var Plan = _unitOfWork.GetRepository<Plan>().GetById(id);
            if (Plan is null || Plan.IsActive == false || HasActiveMemberships(id) ) return null;
            return new UpdatePlanViewModel
            {
                PlanName = Plan.Name,
                Description = Plan.Description,
                Price = Plan.Price,
                DurationDays = Plan.DurationDays
            };


        }

        public bool UpdatePlan(int id, UpdatePlanViewModel updatedPlan)
        {
            var Plan = _unitOfWork.GetRepository<Plan>().GetById(id);
            if (Plan is null || HasActiveMemberships(id)) return false;
            try
            {
                (Plan.Description, Plan.Price, Plan.DurationDays, Plan.UpdatedAt) =
               (updatedPlan.Description, updatedPlan.Price, updatedPlan.DurationDays, DateTime.Now);

                _unitOfWork.GetRepository<Plan>().Update(Plan);
                return _unitOfWork.SaveChanges() > 0;
            }
            catch
            {        
                return false;
            }

        }
        public bool TogglePlanStatus(int id)
        {
            var Repo = _unitOfWork.GetRepository<Plan>();
            var Plan = Repo.GetById(id);
            if (Plan is null || HasActiveMemberships(id)) return false;

            Plan.IsActive = Plan.IsActive == true ? false : true;
            Plan.UpdatedAt = DateTime.Now;
            try
            {
               Repo.Update(Plan);
                return _unitOfWork.SaveChanges() > 0;
            }
            catch
            {
                return false;
            }


        }



        #region HelperMethods
        private bool HasActiveMemberships(int planId)
        {
            var ActiveMemberShips = _unitOfWork.GetRepository<MemberShip>()
                .GetAll(m => m.PlanId == planId && m.Status == "Active");
            return ActiveMemberShips.Any();
        }
        #endregion
    }

   
}
