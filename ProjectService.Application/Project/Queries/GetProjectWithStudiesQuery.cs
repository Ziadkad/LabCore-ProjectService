using AutoMapper;
using MediatR;
using ProjectService.Application.Common.Exceptions;
using ProjectService.Application.Common.Extensions;
using ProjectService.Application.Common.Interfaces;
using ProjectService.Application.Project.Models;

namespace ProjectService.Application.Project.Queries;

public record GetProjectWithStudiesQuery(Guid Id) : IRequest<ProjectWithStudiesDto>;

public class GetProjectWithStudiesQueryHandler(IProjectRepository projectRepository, IMapper mapper, IUserContext userContext) : IRequestHandler<GetProjectWithStudiesQuery, ProjectWithStudiesDto>
{
    public async Task<ProjectWithStudiesDto> Handle(GetProjectWithStudiesQuery request, CancellationToken cancellationToken)
    {
        Domain.Project.Project? project = await projectRepository.GetProjectByIdIncludeStudiesAll(request.Id, cancellationToken);
        if (project is null || project.IsArchived)
        {
            throw new NotFoundException("Project", request.Id);
        }
        UserExtention.CheckProjectUserRoleandNotFound(project, userContext.GetUserRole(), userContext.GetCurrentUserId());
        project.Studies.RemoveAll(study => study.IsArchived);
        return mapper.Map<ProjectWithStudiesDto>(project);
    }
}