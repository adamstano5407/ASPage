using APIKros.DTOs;
using APIKros.Models;
using APIKros.Requests.Employee;
using AutoMapper;

namespace APIKros.Mapping;

public class EmployeeMappingProfile : Profile
{
    public EmployeeMappingProfile()
    {
        CreateMap<Employee, EmployeeDto>();

        CreateMap<CreateEmployeeRequest, Employee>();

        CreateMap<UpdateEmployeeRequest, Employee>()
            .ForAllMembers(opt =>
                opt.Condition((src, dest, srcMember) => srcMember is not null));
    }
}