using AutoMapper;
using APIKros.DTOs;
using APIKros.Models;
using APIKros.Requests;

namespace APIKros.Mapping;

public class DepartmentMappingProfile : Profile
{
    public DepartmentMappingProfile()
    {
        CreateMap<Department, DepartmentDto>();

        CreateMap<CreateDepartmentRequest, Department>();

        CreateMap<UpdateDepartmentRequest, Department>()
            .ForAllMembers(opt =>
                opt.Condition((src, dest, srcMember) => srcMember != null));
    }
}