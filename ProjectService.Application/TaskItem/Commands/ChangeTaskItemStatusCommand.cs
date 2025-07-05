using AutoMapper;
using MediatR;
using ProjectService.Application.Common.Exceptions;
using ProjectService.Application.Common.Extensions;
using ProjectService.Application.Common.Interfaces;
using ProjectService.Application.Common.Services.Interfaces;
using ProjectService.Application.TaskItem.Models;
using ProjectService.Domain.Task;

namespace ProjectService.Application.TaskItem.Commands;

public record ChangeTaskItemStatusCommand(Guid Id, TaskItemStatus Status) : IRequest<TaskItemDto>;

public class ChangeTaskItemStatusCommandHandler(ITaskItemRepository taskItemRepository, IUnitOfWork unitOfWork, IMapper mapper, IUserContext userContext, IProjectService projectService)  : IRequestHandler<ChangeTaskItemStatusCommand, TaskItemDto>
{
    public async Task<TaskItemDto> Handle(ChangeTaskItemStatusCommand request, CancellationToken cancellationToken)
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
        
        taskItem.setStatus(request.Status);
        await projectService.UpdateProjectProgressPercentage(taskItem.Study.Project.Id, cancellationToken);
        var isSaved = await unitOfWork.SaveChangesAsync(cancellationToken);
        if (isSaved <= 0)
        {
            throw new InternalServerException();
        }
        return mapper.Map<TaskItemDto>(taskItem);
    }
}