using AutoMapper;
using Contacts.Application.Features.People.Commands;
using Contacts.Application.Features.People.DTOs;
using Contacts.Domain.Entities;

namespace Contacts.Application.Mapping
{
    public class ContactsMappingProfile : Profile
    {
        public ContactsMappingProfile()
        {
            CreateMap<Person, PersonForGettingDto>()
                .ForMember(dest => dest.Id, options => options.MapFrom(src => src.Id))
                .ForMember(dest => dest.FullName, options => options.MapFrom(src => src.FullName))
                .ForMember(dest => dest.DateOfBirth, options => options.MapFrom(src => src.DateOfBirth));

            CreateMap<CreatePersonCommand, Person>()
                .ForMember(dest => dest.FullName, options => options.MapFrom(src => src.FullName))
                .ForMember(dest => dest.DateOfBirth, options => options.MapFrom(src => src.DateOfBirth));

            CreateMap<UpdatePersonCommand, Person>()
                .ForMember(dest => dest.Id, options => options.MapFrom(src => src.Id))
                .ForMember(dest => dest.FullName, options => options.MapFrom(src => src.FullName))
                .ForMember(dest => dest.DateOfBirth, options => options.MapFrom(src => src.DateOfBirth));
        }
    }
}
