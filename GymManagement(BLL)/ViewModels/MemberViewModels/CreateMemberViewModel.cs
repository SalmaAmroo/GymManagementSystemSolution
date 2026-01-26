using GymManagementDAL.Entities.Enums;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagementBLL.ViewModels.MemberViewModels
{
    public class CreateMemberViewModel
    {
        [Required(ErrorMessage ="Profile Photo Is Required")]
        [Display(Name = "Profile Photo")]
        public IFormFile PhotoFile { get; set; } = null!;


        [Required(ErrorMessage ="Name id Required")]
        [StringLength (maximumLength:50 , MinimumLength = 2, ErrorMessage ="Name Must Be Between 2 And 50 Char")]
        [RegularExpression(@"^[a-zA-Z\s]+$" , ErrorMessage = "Name Can Only Letters And Spaces")]
        public string Name { get; set; } = null!;

        [Required(ErrorMessage = "Email is Required")]
        [EmailAddress(ErrorMessage = "Invalid Email Format")]
        [DataType(DataType.EmailAddress)]
        [StringLength(100,MinimumLength =5 ,ErrorMessage = "Email Must Be Between 5 and 100 Char")]
        public string Email { get; set; } = null!;

        [Required(ErrorMessage = "Phone is Required")]
        [Phone(ErrorMessage = "Invalid Phone Formate")]
        [RegularExpression(@"^(010|011|012|015)\d{8}$",ErrorMessage = "Phone Number Must Be Valid Egyptian Phone Number")]
        [DataType(DataType.PhoneNumber)]
        public string Phone { get; set; } = null!;

        [Required(ErrorMessage = "Date Of Birth is Required")]
        [DataType(DataType.Date)]
        public DateOnly DateOfBirth { get; set; }

        [Required(ErrorMessage = "Gender is Required")]
        public Gender Gender { get; set; }

        [Required(ErrorMessage = "BuildNumber is Required")]
        [Range(1,9000,ErrorMessage = "BuildNumber Must Be Between 1 And 9000")]
        public int BuildNumber { get; set; }

        [Required(ErrorMessage = "Street is Required")]
        [StringLength(30, MinimumLength = 2, ErrorMessage = "Street Must Be Between 2 And 30 Char")]
        public string Street { get; set; } = null!;

        [Required(ErrorMessage = "City is Required")]
        [StringLength(30, MinimumLength = 2, ErrorMessage = "City Must Be Between 2 And 30 Char")]
        [RegularExpression(@"^[a-zA-Z\s]+$", ErrorMessage = "City Can Only Letters And Spaces")]

        public string City { get; set; } = null!;

        [Required(ErrorMessage = "Health Record is Required")]
        public HealthRecordViewModel HealthRecordViewModel { get; set; } = null!;

    }
}
