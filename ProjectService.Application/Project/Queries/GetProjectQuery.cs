using AutoMapper;
using MediatR;
using ProjectService.Application.Common.Exceptions;
using ProjectService.Application.Common.Extensions;
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
        if (project is null || project.IsArchived)
        {
            throw new NotFoundException("Project", request.Id);
        }

        UserExtention.CheckProjectUserRoleandNotFound(project, userContext.GetUserRole(), userContext.GetCurrentUserId());
        return mapper.Map<ProjectDto>(project);
    }
}