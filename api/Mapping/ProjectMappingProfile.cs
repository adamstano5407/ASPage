using APIKros.DTOs;
using APIKros.Models;
using APIKros.Requests;
using AutoMapper;

namespace APIKros.Mapping;

public class ProjectMappingProfile : Profile
{
    public ProjectMappingProfile()
    {
        CreateMap<Project, ProjectDto>();

        CreateMap<CreateProjectRequest, Project>();

        CreateMap<UpdateProjectRequest, Project>()
            .ForAllMembers(opt =>
                opt.Condition((src, dest, srcMember) => srcMember is not null));
    }
}