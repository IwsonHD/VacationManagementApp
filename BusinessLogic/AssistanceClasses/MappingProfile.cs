using AutoMapper;
using BusinessLogic.DTOs;
using BusinessLogic.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.AssistanceClasses
{
    public class MappingProfile : Profile
    {
        public MappingProfile() 
        {
            CreateMap<Employee, EmployeeDTO>()
                .ForMember(dest => dest.FirstName,
                    src => src.MapFrom(x => x.FirstName))
                .ForMember(dest => dest.LastName,
                    src => src.MapFrom(x => x.LastName))
                .ForMember(dest => dest.Email,
                    src => src.MapFrom(x => x.Email))
                .ForMember(dest => dest.PhoneNumber,
                    src => src.MapFrom(x => x.PhoneNumber))
                .ForMember(dest => dest.Role,
                    src => src.MapFrom(x => "Employee"))
                .ForMember(dest => dest.EmployersEmail,
                    src => src.MapFrom(x => x.EmployersEmail));
            CreateMap<Employer, EmployerDTO>()
               .ForMember(dest => dest.FirstName,
                    src => src.MapFrom(x => x.FirstName))
                .ForMember(dest => dest.LastName,
                    src => src.MapFrom(x => x.LastName))
                .ForMember(dest => dest.Email,
                    src => src.MapFrom(x => x.Email))
                .ForMember(dest => dest.PhoneNumber,
                    src => src.MapFrom(x => x.PhoneNumber))
                .ForMember(dest => dest.Role,
                    src => src.MapFrom(x => "Employer"))
                .ForMember(dest => dest.CompanyName,
                    src => src.MapFrom(x => x.CompanyName));


        }


    }
}
