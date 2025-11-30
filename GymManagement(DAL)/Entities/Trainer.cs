using GymManagementDAL.Entities.Enums;

namespace GymManagementDAL.Entities
{
    public class Trainer : GymUser
    {
        //Hire Date == CreatedAt of BaseEntity
        public Specialties Specialtyies { get; set; }

        public ICollection<Session> TrainerSessions { get; set; } = null!;


    }
}
