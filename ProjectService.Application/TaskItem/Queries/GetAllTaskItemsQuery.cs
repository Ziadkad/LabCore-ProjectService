using AutoMapper;
using MediatR;
using ProjectService.Application.Common.Interfaces;
using ProjectService.Application.Common.Models;
using ProjectService.Application.TaskItem.Models;
using ProjectService.Domain.Task;


namespace ProjectService.Application.TaskItem.Queries;

public record GetAllTaskItemsQuery(
    string? Keyword,
    List<Guid>? AssignedTo,
    DateTime? StartDate,
    DateTime? EndDate,
    TaskItemStatus? TaskItemStatus,
    PageQueryRequest PageQuery) : IRequest<PageQueryResult<TaskItemDto>>;

public class GetAllTaskItemsQueryHandler(ITaskItemRepository taskItemRepository, IMapper mapper , IUserContext userContext) : IRequestHandler<GetAllTaskItemsQuery, PageQueryResult<TaskItemDto>>
{
    public async Task<PageQueryResult<TaskItemDto>> Handle(GetAllTaskItemsQuery request, CancellationToken cancellationToken)
    {
        List<Domain.Task.TaskItem> taskItems = new List<Domain.Task.TaskItem>();
        int count = 0;
        if (userContext.GetUserRole() == UserRole.Manager)
        {
             (taskItems, count) = await taskItemRepository.GetFilteredTaskItemsAsync(
                request.Keyword,
                request.AssignedTo,
                request.StartDate,
                request.EndDate,
                request.TaskItemStatus,
                null,
                userContext.GetCurrentUserId(),
                request.PageQuery,
                cancellationToken
            );
        }
        else if (userContext.GetUserRole() == UserRole.Researcher)
        {
            (taskItems, count) = await taskItemRepository.GetFilteredTaskItemsAsync(
                request.Keyword,
                request.AssignedTo,
                request.StartDate,
                request.EndDate,
                request.TaskItemStatus,
                userContext.GetCurrentUserId(),
                null,
                request.PageQuery,
                cancellationToken
            );
        }
        else
        {
            (taskItems, count) = await taskItemRepository.GetFilteredTaskItemsAsync(
                request.Keyword,
                request.AssignedTo,
                request.StartDate,
                request.EndDate,
                request.TaskItemStatus,
                userContext.GetCurrentUserId(),
                null,
                request.PageQuery,
                cancellationToken
            );
        }

        var taskItemDtos = mapper.Map<List<TaskItemDto>>(taskItems);

        return new PageQueryResult<TaskItemDto>(taskItemDtos,count);
    }
}