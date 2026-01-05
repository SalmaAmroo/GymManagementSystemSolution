using AutoMapper;
using GymManagementBLL.ViewModels.MemberViewModels;
using GymManagementBLL.ViewModels.PlanViewModels;
using GymManagementBLL.ViewModels.ServiceViewModels;
using GymManagementBLL.ViewModels.TrainerViewModels;
using GymManagementDAL.Entities;
using GymManagementSystemBLL.ViewModels.SessionViewModels;
using System.Net;
using System.Runtime.Serialization;


namespace GymManagementBLL
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles()
        {
            MapSession();
            MapMember();
            MapPlan();
            MapTrainer();


        }
        private void MapSession()
        {
            CreateMap<Session, SessionViewModel>()
                 .ForMember(dest => dest.CategoryName, Options => Options.MapFrom(srs => srs.SessionCategory.CategoryName))
                 .ForMember(dest => dest.TrainerName, Options => Options.MapFrom(srs => srs.TrainerSession.Name))
                 .ForMember(dest => dest.AvailableSlots, Options => Options.Ignore());

            CreateMap<CreateSessionViewModel, Session>();
            CreateMap<Session, UpdateSessionViewModel>().ReverseMap();
        }
        private void MapMember()
        {
            CreateMap<CreateMemberViewModel, Member>()
            .ForMember(dest => dest.Address, opt => opt.MapFrom(src => src))
            .ForMember(dest=>dest.HealthRecord,opt=>opt.MapFrom(src=>src.HealthRecordViewModel))
            .ForMember(dest=>dest.PhoneNumber,opt=>opt.MapFrom(src=>src.Phone));
            
            CreateMap<CreateMemberViewModel, Address>()
                .ForMember(dest => dest.BuildNumber, opt => opt.MapFrom(src => src.BuildNumber))
                .ForMember(dest => dest.Street, opt => opt.MapFrom(src => src.Street))
                .ForMember(dest => dest.City, opt => opt.MapFrom(src => src.City));

            CreateMap<HealthRecordViewModel, HealthRecord>().ReverseMap();

            CreateMap<Member, MemberViewModels>()
                .ForMember(dest => dest.Gender,opt=>opt.MapFrom(src=>src.Gender.ToString()))
                .ForMember(dest=>dest.DateOfBirth,opt=>opt.MapFrom(src=>src.DateOfBirth.ToShortDateString()))
                .ForMember(dest=>dest.Address,opt=>opt.MapFrom(src=>$"{src.Address.BuildNumber} - {src.Address.Street} - {src.Address.City}"))
                .ForMember(dest => dest.Phone,opt => opt.MapFrom(src => src.PhoneNumber));
            ;

            CreateMap<Member, MemberToUpdateViewModel>()
                .ForMember(dest => dest.BuildNumber, opt => opt.MapFrom(src => src.Address.BuildNumber))
                .ForMember(dest => dest.Street, opt => opt.MapFrom(src => src.Address.Street))
                .ForMember(dest => dest.City, opt => opt.MapFrom(src => src.Address.City))
                .ForMember(dest => dest.Phone, opt => opt.MapFrom(src => src.PhoneNumber));


            CreateMap<MemberToUpdateViewModel, Member>()
                .ForMember(dest => dest.Name, opt => opt.Ignore())
                .ForMember(dest => dest.Photo, opt => opt.Ignore())
                .AfterMap((src, dest) =>
                {
                    dest.Address.BuildNumber = src.BuildNumber;
                    dest.Address.Street = src.Street;
                    dest.Address.City = src.City;
                    dest.UpdatedAt = DateTime.Now;
                });
            


        }
        private void MapPlan()
        {
            CreateMap<Plan, PlanViewModel>();
            CreateMap <Plan, UpdatePlanViewModel>() 
                .ForMember(dest => dest.PlanName, opt => opt.MapFrom(src => src.Name));
            CreateMap<UpdatePlanViewModel, Plan>()
                .ForMember(dest => dest.Name, opt => opt.Ignore())
                .ForMember(dest=>dest.UpdatedAt,opt=>opt.MapFrom(src=>DateTime.Now));
        }
        private void MapTrainer()
        {
            CreateMap<CreateTrainerViewModel, Trainer>()
                .ForMember(dest=>dest.PhoneNumber,opt=>opt.MapFrom(src=>src.Phone))
                .ForMember(dest=>dest.Address, opt=>opt.MapFrom(src=>new Address
                {
                    BuildNumber = src.BuildNumber,
                    Street = src.Street,
                    City = src.City,
                }));
            CreateMap<Trainer, TrainerViewModel>()
                .ForMember(dest => dest.Specialties, opt => opt.MapFrom(src => src.Specialtyies.ToString()))
                .ForMember(dest => dest.Phone, opt => opt.MapFrom(src => src.PhoneNumber))
                .ForMember(dest=>dest.Address,opt=>opt.MapFrom(src=>$"{src.Address.BuildNumber}-{src.Address.Street}-{src.Address.City}"));

            CreateMap<Trainer, TrainerToUpdateViewModel>()
                .ForMember(dest=>dest.BuildNumber , opt=>opt.MapFrom(src=>src.Address.BuildNumber))
                .ForMember(dest => dest.Street, opt => opt.MapFrom(src => src.Address.Street))
                .ForMember(dest => dest.City, opt => opt.MapFrom(src => src.Address.City));

            CreateMap<TrainerToUpdateViewModel, Trainer>()
                .ForMember(dest => dest.Name, opt => opt.Ignore())
                .AfterMap((src, dest) =>
                {
                    dest.Address.BuildNumber = src.BuildNumber;
                    dest.Address.Street = src.Street;
                    dest.Address.City = src.City;
                    dest.UpdatedAt = DateTime.Now;
                });




        }

    }
}
