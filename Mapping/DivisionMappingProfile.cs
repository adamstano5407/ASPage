using APIKros.DTOs;
using APIKros.Models;
using APIKros.Requests;
using AutoMapper;

namespace APIKros.Mapping;

public class DivisionMappingProfile : Profile
{
    public DivisionMappingProfile()
    {
        CreateMap<Division, DivisionDto>();

        CreateMap<CreateDivisionRequest, Division>();

        CreateMap<UpdateDivisionRequest, Division>()
            .ForAllMembers(opt =>
                opt.Condition((src, dest, srcMember) => srcMember is not null));
    }
}