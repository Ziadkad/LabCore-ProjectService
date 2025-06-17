using AutoMapper;
using MediatR;
using ProjectService.Application.Common.Exceptions;
using ProjectService.Application.Common.Interfaces;
using ProjectService.Application.Common.Models;
using ProjectService.Application.Project.Models;

namespace ProjectService.Application.Project.Queries;

public record GetProjectQuery(Guid Id):IRequest<ProjectDto>;

public class GetProjectQueryHandler(IProjectRepository projectRepository, IMapper mapper, IUserContext userContext) : IRequestHandler<GetProjectQuery, ProjectDto>
{
    public async Task<ProjectDto> Handle(GetProjectQuery request, CancellationToken cancellationToken)
    {
        Domain.Project.Project? project = await projectRepository.FindAsync(request.Id,cancellationToken);
        
        var userId = userContext.GetCurrentUserId();
        var userRole = userContext.GetUserRole();

        if (project is null || project.IsArchived)
        {
            throw new NotFoundException("Project", request.Id);
        }

        if (!project.IsPublic)
        {
            if (userRole == UserRole.Manager && project.ManagerId != userId)
            {
                throw new NotFoundException("Project", request.Id);
            }

            if (userRole == UserRole.Researcher && (project.Researchers == null || !project.Researchers.Contains(userId)))
            {
                throw new NotFoundException("Project", request.Id);
            }
        }
        return mapper.Map<ProjectDto>(project);
    }
}