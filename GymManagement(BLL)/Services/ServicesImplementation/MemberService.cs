using AutoMapper;
using GymManagementBLL.Services.Interfaces;
using GymManagementBLL.ViewModels.MemberViewModels;
using GymManagementDAL.Entities;
using GymManagementDAL.Repositories.Interfaces;

namespace GymManagementBLL.Services.ServicesImplementation
{
    public class MemberService : IMemberService
    {
        private readonly IUnitOfWork _UnitOfWork;
        private readonly IMapper _mapper;

        public MemberService(IUnitOfWork UnitOfWork , IMapper mapper) 
        {
            _UnitOfWork = UnitOfWork;
            _mapper = mapper;
        }

        public bool CreateMember(CreateMemberViewModel CreatingMember)
        {
           try
           {
                if (IsEmailExists(CreatingMember.Email) || IsPhoneExists(CreatingMember.Phone)) return false;
                var member = _mapper.Map<Member>(CreatingMember);

                _UnitOfWork.GetRepository<Member>().Add(member);
                return _UnitOfWork.SaveChanges() > 0;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public IEnumerable<MemberViewModels> GetAllMembers()
        {
           var Members = _UnitOfWork.GetRepository<Member>().GetAll();
           if (Members is null || !Members.Any()) return [];
            #region outo Mapper Way
            var MemberViewModels = _mapper.Map<IEnumerable<MemberViewModels>>(Members);
            #endregion
            return MemberViewModels;
        }

        public HealthRecordViewModel? GetHealthRecord(int id) 
        {
            var MemberHealthRecord = _UnitOfWork.GetRepository<HealthRecord>().GetById(id);
            if (MemberHealthRecord is null) return null;
            return _mapper.Map<HealthRecordViewModel>(MemberHealthRecord);

        }

        public MemberViewModels? GetMemberDetails(int id)
        {
            var member = _UnitOfWork.GetRepository<Member>().GetById(id);
            if (member is null) return null;
            var memberDetails = _mapper.Map<MemberViewModels>(member);

            var memberShip = _UnitOfWork.GetRepository<MemberShip>().GetAll(x => x.MemberId == id && x.Status == "Active")
                                                  .FirstOrDefault();
            if (memberShip is not null)
            {
                memberDetails.MemberShipStartDate = memberShip.CreatedAt.ToShortDateString();
                memberDetails.MemberShipEndDate = memberShip.EndDate.ToShortDateString();

                var Plan = _UnitOfWork.GetRepository<Plan>().GetById(memberShip.PlanId);
                memberDetails.PlanName = Plan?.Name;
            }

            return memberDetails;
        }

        public MemberToUpdateViewModel? GetMemberToUpdateViewModel(int id)
        {
           var member = _UnitOfWork.GetRepository<Member>().GetById(id);
           if (member is null) return null;
                return _mapper.Map<MemberToUpdateViewModel>(member);

        }

        public bool RemoveMember(int id)
        {
            var MemberRepo = _UnitOfWork.GetRepository<Member>();
            var member = MemberRepo.GetById(id);
            if (member is null) return false;
            var SessionsIds = _UnitOfWork.GetRepository<MemberSession>().GetAll(x => x.MemberId == id).Select(x=>x.SessionId);
            var HasFutureMemberSessions = _UnitOfWork.GetRepository<Session>().GetAll(x => SessionsIds.Contains(x.Id) && x.StartDate>DateTime.Now).Any();
            if (HasFutureMemberSessions) return false;
            
            var MemberShipRepo = _UnitOfWork.GetRepository<MemberShip>();
            var MemberShips = MemberShipRepo.GetAll(x => x.MemberId == id);
            try
            {
                if (MemberShips.Any())
                {
                    foreach (var memberShip in MemberShips)
                    {
                        MemberShipRepo.Delete(memberShip);
                    }
                }
              MemberRepo.Delete(member) ;
              return _UnitOfWork.SaveChanges() > 0;
            }
            catch 
            { 
                return false;
            }

        }

        public bool UpdateMember(int id, MemberToUpdateViewModel memberToUpdate)
        {
            try 
            {
                var EmailExists = _UnitOfWork.GetRepository<Member>().GetAll(x => x.Email == memberToUpdate.Email && x.Id != id);
                var PhoneExists = _UnitOfWork.GetRepository<Member>().GetAll(x => x.PhoneNumber == memberToUpdate.Phone && x.Id != id);
                if (EmailExists.Any() || PhoneExists.Any()) return false;

               var MemberRepo = _UnitOfWork.GetRepository<Member>();
               var Member = MemberRepo.GetById(id);
               if (Member is null) return false;
               _mapper.Map(memberToUpdate, Member);

                MemberRepo.Update(Member);
               return _UnitOfWork.SaveChanges() > 0;


            }
            catch { return false; } 
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
