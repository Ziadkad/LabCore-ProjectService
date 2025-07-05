using AutoMapper;
using MediatR;
using ProjectService.Application.Common.Exceptions;
using ProjectService.Application.Common.Extensions;
using ProjectService.Application.Common.Interfaces;
using ProjectService.Application.TaskItem.Models;

namespace ProjectService.Application.TaskItem.Queries;

public record GetTaskItemQuery(Guid Id) : IRequest<TaskItemDto>;

public class GetTaskItemQueryHandler(ITaskItemRepository taskItemRepository, IUserContext userContext, IMapper mapper ) : IRequestHandler<GetTaskItemQuery, TaskItemDto>
{
    public async Task<TaskItemDto> Handle(GetTaskItemQuery request, CancellationToken cancellationToken)
    {
        Domain.Task.TaskItem?  taskItem = await taskItemRepository.FindTaskItemIncludeAllAsync(request.Id, cancellationToken);
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
        UserExtention.CheckProjectUserRoleandNotFound(taskItem.Study.Project, userContext.GetUserRole(),userContext.GetCurrentUserId());
        return mapper.Map<TaskItemDto>(taskItem);
    }
}

