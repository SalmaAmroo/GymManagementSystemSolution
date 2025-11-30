using GymManagementBLL.Services.Interfaces;
using GymManagementBLL.ViewModels.MemberViewModels;
using GymManagementDAL.Entities;
using GymManagementDAL.Repositories.Interfaces;

namespace GymManagementBLL.Services.ServicesImplementation
{
    internal class MemberService : IMemberService
    {
        private readonly IUnitOfWork _UnitOfWork;

        public MemberService(IUnitOfWork UnitOfWork) 
        {
            _UnitOfWork = UnitOfWork;
        }

        public bool CreateMember(CreateMemberViewModel CreatingMember)
        {
           try
           {
                if (IsEmailExists(CreatingMember.Email) || IsPhoneExists(CreatingMember.Phone)) return false;
                var member = new Member()
                {
                    Name = CreatingMember.Name,
                    Email = CreatingMember.Email,
                    PhoneNumber = CreatingMember.Phone,
                    DateOfBirth = CreatingMember.DateOfBirth,
                    Gender = CreatingMember.Gender,
                    Address = new Address()
                    {
                        BuildNumber = CreatingMember.BuildNumber,
                        City = CreatingMember.City,
                        Street = CreatingMember.Street
                    },
                    HealthRecord = new HealthRecord()
                    {
                        Height = CreatingMember.HealthRecordViewModel.Height,
                        Weight = CreatingMember.HealthRecordViewModel.Weight,
                        BloodType = CreatingMember.HealthRecordViewModel.BloodType,
                        Note = CreatingMember.HealthRecordViewModel.Note
                    }
                };

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
           if (Members is null || Members.Any()) return [];
            // Mapping from type to type
            #region Manual Mapping way 01
            //var MemberViewModels =  new List<MemberViewModels>();
            //foreach (var member in Members)
            //{
            //    var memberViewModel = new MemberViewModels()
            //    {
            //        Id = member.Id,
            //        Name = member.Name,
            //        Email = member.Email,
            //        Phone = member.PhoneNumber,
            //        Photo = member.Photo,
            //        Gender = member.Gender.ToString()
            //    };
            //    MemberViewModels.Add(memberViewModel);

            //}
            #endregion

            #region Manual Mapping way 02 using LINQ
            var MemberViewModels = Members.Select(member => new MemberViewModels()
            {
                Id = member.Id,
                Name = member.Name,
                Email = member.Email,
                Phone = member.PhoneNumber,
                Photo = member.Photo,
                Gender = member.Gender.ToString()
            });
            #endregion
            return MemberViewModels;
        }

        public HealthRecordViewModel? GetHealthRecord(int id)
        {
            var MemberHealthRecord = _UnitOfWork.GetRepository<HealthRecord>().GetById(id);
            if (MemberHealthRecord is null) return null;
            return new HealthRecordViewModel()
            {
                BloodType = MemberHealthRecord.BloodType,
                Height = MemberHealthRecord.Height,
                Weight = MemberHealthRecord.Weight,
                Note = MemberHealthRecord.Note
            };

        }

        public MemberViewModels? GetMemberDetails(int id)
        {
            var member = _UnitOfWork.GetRepository<Member>().GetById(id);
            if (member is null) return null;
            var memberDetails = new MemberViewModels()
            {
                Name = member.Name,
                Email = member.Email,
                Phone = member.PhoneNumber,
                Gender = member.Gender.ToString(),
                DateOfBirth = member.DateOfBirth.ToShortDateString(),
                Address = $"{member.Address.BuildNumber}-{member.Address.Street}-{member.Address.City}",
                Photo = member.Photo,

            };

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
                return new MemberToUpdateViewModel()
                {
                 Name = member.Name,
                 Photo = member.Photo,
                 Email = member.Email,
                 Phone = member.PhoneNumber,
                 BuildNumber = member.Address.BuildNumber,
                 Street = member.Address.Street,
                 City = member.Address.City
                };
               
        }

        public bool RemoveMember(int id)
        {
            var MemberRepo = _UnitOfWork.GetRepository<Member>();
            var member = MemberRepo.GetById(id);
            if (member is null) return false;
            var HasActiveMemberSessions = _UnitOfWork.GetRepository<MemberSession>().GetAll(x => x.MemberId == id && x.Session.StartDate > DateTime.Now).Any();
            if (HasActiveMemberSessions) return false;
            
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
               if (IsEmailExists(memberToUpdate.Email) || IsPhoneExists(memberToUpdate.Phone)) return false;
               var MemberRepo = _UnitOfWork.GetRepository<Member>();
               var Member = MemberRepo.GetById(id);
               if (Member is null) return false;
               Member.Email = memberToUpdate.Email;
               Member.PhoneNumber = memberToUpdate.Phone;
               Member.Address.BuildNumber = memberToUpdate.BuildNumber;
               Member.Address.Street = memberToUpdate.Street;
               Member.Address.City = memberToUpdate.City;
               Member.UpdatedAt= DateTime.Now;
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
