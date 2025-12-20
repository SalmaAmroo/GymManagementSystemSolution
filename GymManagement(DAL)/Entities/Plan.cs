using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagementDAL.Entities
{
    public class Plan : BaseEntity
    {
        public string Name { get; set; } = null!;
        public string Description { get; set; } = null!;
        public decimal Price { get; set; }
        
        [Column("DurationDate")]
        public int DurationDays { get; set; }
        public bool IsActive { get; set; }
        public ICollection<MemberShip> PlanMembers { get; set; } = null!;



    }
}
