using MediatR;
using ProjectService.Application.Common.Exceptions;
using ProjectService.Application.Common.Interfaces;

namespace ProjectService.Application.Project.Commands;

public record DeleteProjectCommand(Guid ProjectId): IRequest;

public class DeleteProjectCommandHandler(IProjectRepository projectRepository, IUnitOfWork unitOfWork, IUserContext userContext) : IRequestHandler<DeleteProjectCommand>
{
    public async Task Handle(DeleteProjectCommand request, CancellationToken cancellationToken)
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
        project.SetArchived();
        var isSaved = await unitOfWork.SaveChangesAsync(cancellationToken);
        if (isSaved <= 0)
        {
            throw new InternalServerException();
        }
    }
}

