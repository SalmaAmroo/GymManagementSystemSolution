using AutoMapper;
using GymManagementBLL.ViewModels.ServiceViewModels;
using GymManagementDAL.Entities;
using GymManagementSystemBLL.ViewModels.SessionViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagementBLL
{
    public class MappingProfiles  : Profile
    {
        public MappingProfiles()
        {
            CreateMap<Session, SessionViewModel>()
                .ForMember(dest=>dest.CategoryName , Options => Options.MapFrom(srs=>srs.SessionCategory.CategoryName))
                .ForMember(dest => dest.TrainerName, Options => Options.MapFrom(srs => srs.TrainerSession.Name))
                .ForMember(dest => dest.AvailableSlots , Options =>Options.Ignore()); 
        
           CreateMap<CreateSessionViewModel, Session>();
           CreateMap<Session, UpdateSessionViewModel>().ReverseMap();


        }
    }
}
