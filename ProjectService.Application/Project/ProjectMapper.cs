using AutoMapper;
using ProjectService.Application.Project.Models;

namespace ProjectService.Application.Project;

public class ProjectMapper : Profile
{
    public ProjectMapper()
    {
        CreateMap<Domain.Project.Project, ProjectDto>();
        CreateMap<Domain.Project.Project, ProjectWithStudiesDto>();

    }
}