using AutoMapper;
using MediatR;
using ProjectService.Application.Common.Exceptions;
using ProjectService.Application.Common.Interfaces;
using ProjectService.Application.Project.Models;

namespace ProjectService.Application.Project.Commands;

public record AddResearchersCommand(Guid ProjectId, List<Guid> ResearcherIds) : IRequest<ProjectDto>;

public class AddResearchersCommandHandler(IProjectRepository projectRepository, IUnitOfWork unitOfWork, IMapper mapper, IUserContext userContext) : IRequestHandler<AddResearchersCommand, ProjectDto>
{
    public  async Task<ProjectDto> Handle(AddResearchersCommand request, CancellationToken cancellationToken)
    {
        Domain.Project.Project? project = await projectRepository.FindAsync(request.ProjectId,cancellationToken);
        if (project is null)
        {
            throw new NotFoundException("Project", request.ProjectId);
        }
        if (project.ManagerId != userContext.GetCurrentUserId())
        {
            throw new ForbiddenException();
        }
        project.AddResearchers(request.ResearcherIds);
        var isSaved = await unitOfWork.SaveChangesAsync(cancellationToken);
        if (isSaved <= 0)
        {
            throw new InternalServerException();
        }
        return mapper.Map<ProjectDto>(project);
    }
}