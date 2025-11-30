using GymManagementDAL.Entities.Enums;
using Microsoft.EntityFrameworkCore;


namespace GymManagementDAL.Entities
{
    public abstract class GymUser : BaseEntity
    {
        public string Name { get; set; } = null!; // required
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public DateOnly DateOfBirth { get; set; }
        public Gender Gender { get; set; }
        public Address Address { get; set; } = null!;

    }
    [Owned]
    public class Address
    {
        public int BuildNumber { get; set; }
        public string Street { get; set; } = null!;
        public string City { get; set; } = null!; 
    }
}
