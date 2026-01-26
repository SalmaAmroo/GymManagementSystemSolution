using AutoMapper;
using GymManagementBLL.Services.Interfaces;
using GymManagementBLL.ViewModels.MemberShipViewModels;
using GymManagementDAL.Entities;
using GymManagementDAL.Repositories.Interfaces;

namespace GymManagementBLL.Services.ServicesImplementation
{
    public class MemberShipService : IMemberShipService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public MemberShipService(IUnitOfWork unitOfWork , IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        public IEnumerable<MemberShipViewModel> GetAllMemberShips()
        {
            var memberShips = _unitOfWork.MemberShipRepository.GetAllMemberShipsWithMembersAndPlans(MS => MS.Status != null && MS.Status == "Active");

            var memberShipViewModels = _mapper.Map<IEnumerable<MemberShipViewModel>>(memberShips);
            return memberShipViewModels;
        }
        public bool CreateMemberShip(CreateMemberShipViewModel model)
        {
            if (!IsMemberExists(model.MemberId)||!IsPlanExists(model.PlanId)||IsMemberHasActiveMemberShip(model.MemberId)) return false;
            var memberShipRepo = _unitOfWork.GetRepository<MemberShip>();
            var plan = _unitOfWork.GetRepository<Plan>().GetById(model.PlanId);
            var newMemberShip = _mapper.Map<MemberShip>(model);
            newMemberShip.EndDate = DateTime.UtcNow.AddDays(plan!.DurationDays);

            memberShipRepo.Add(newMemberShip);
            return _unitOfWork.SaveChanges() >0;

        }
        public IEnumerable<PlanForSelectListViewModel> GetPlanForSelectListViewModels()
        {
            var plans = _unitOfWork.GetRepository<Plan>().GetAll(P=>P.IsActive);
            var planForSelectListViewModels = _mapper.Map<IEnumerable<PlanForSelectListViewModel>>(plans);
            return planForSelectListViewModels;
        }

        public IEnumerable<NameForSelectListViewModel> GetMemberForSelectListViewModels()
        {
            var members = _unitOfWork.GetRepository<Member>().GetAll();
            var memberForSelectListViewModels = _mapper.Map<IEnumerable<NameForSelectListViewModel>>(members);
            return memberForSelectListViewModels;
        }
        public bool DeleteMemberShip(int memberId)
        {
            var memberShipRepo = _unitOfWork.MemberShipRepository;
            var memberShipToDelete = memberShipRepo.GetFirstorDefault(m=>m.MemberId == memberId && m.Status=="Active");

           if(memberShipToDelete is null) return false;
            memberShipRepo.Delete(memberShipToDelete);
            return _unitOfWork.SaveChanges() > 0;

        }


        #region HelperMethod
        private bool IsMemberExists (int memberId)
        =>  _unitOfWork.GetRepository<Member>().GetById(memberId) is not null;
        private bool IsPlanExists(int PlanId)
       => _unitOfWork.GetRepository<Plan>().GetById(PlanId) is not null;
        private bool IsMemberHasActiveMemberShip(int memberId)
        {
            var activeMemberShip = _unitOfWork.MemberShipRepository
                .GetAllMemberShipsWithMembersAndPlans(MS => MS.MemberId == memberId && MS.Status.ToLower() == "Active")
                .FirstOrDefault();
            return activeMemberShip is not null;
        }

        



        #endregion

    }
}
