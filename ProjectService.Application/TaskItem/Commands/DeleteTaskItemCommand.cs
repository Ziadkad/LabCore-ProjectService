using MediatR;
using ProjectService.Application.Common.Exceptions;
using ProjectService.Application.Common.Extensions;
using ProjectService.Application.Common.Interfaces;
using ProjectService.Application.Common.Services.Interfaces;

namespace ProjectService.Application.TaskItem.Commands;

public record DeleteTaskItemCommand(Guid Id) : IRequest;

public class DeleteTaskItemCommandHandler(IUserContext userContext, ITaskItemRepository taskItemRepository, IUnitOfWork unitOfWork, IProjectService projectService) : IRequestHandler<DeleteTaskItemCommand>
{
    public async Task Handle(DeleteTaskItemCommand request, CancellationToken cancellationToken)
    {
        Domain.Task.TaskItem? taskItem = await taskItemRepository.FindTaskItemIncludeAllAsync(request.Id, cancellationToken);
        if (taskItem is null)
        {
            throw new NotFoundException("TaskItem", request.Id);
        }
        if (taskItem.Study is null || taskItem.Study.IsArchived)
        {
            throw new NotFoundException("Study", taskItem.StudyId);
        }
        if (taskItem.Study.Project is null || taskItem.Study.Project.IsArchived)
        {
            throw new NotFoundException("Project", taskItem.Study.ProjectId);
        }
        UserExtention.CheckProjectUserRoleandForbidden(taskItem.Study.Project, userContext.GetUserRole(),userContext.GetCurrentUserId());
         taskItemRepository.Remove(taskItem);
         await projectService.UpdateProjectProgressPercentage(taskItem.Study.Project.Id, cancellationToken);
         var isSaved = await unitOfWork.SaveChangesAsync(cancellationToken);
         if (isSaved <= 0)
         {
             throw new InternalServerException();
         }
    }
}