using APIKros.DTOs;
using APIKros.Models;
using APIKros.Requests;
using AutoMapper;

namespace APIKros.Mapping;

public class CompanyMappingProfile : Profile
{
    public CompanyMappingProfile()
    {
        CreateMap<Company, CompanyDto>();

        CreateMap<CreateCompanyRequest, Company>();

        CreateMap<UpdateCompanyRequest, Company>()
            .ForAllMembers(opt =>
                opt.Condition((src, dest, srcMember) => srcMember != null));
    }
}