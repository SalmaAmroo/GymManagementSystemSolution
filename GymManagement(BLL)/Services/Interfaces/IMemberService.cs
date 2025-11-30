using GymManagementBLL.ViewModels.MemberViewModels;


namespace GymManagementBLL.Services.Interfaces
{
    internal interface IMemberService
    {
        IEnumerable<MemberViewModels> GetAllMembers();

        bool CreateMember(CreateMemberViewModel CreatingMember);

        MemberViewModels? GetMemberDetails(int id);

        HealthRecordViewModel? GetHealthRecord(int id);

        MemberToUpdateViewModel? GetMemberToUpdateViewModel(int id);

        bool UpdateMember(int id, MemberToUpdateViewModel memberToUpdate);

        bool RemoveMember(int id);

    }
}
