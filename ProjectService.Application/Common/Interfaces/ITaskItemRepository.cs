

using ProjectService.Application.Common.Models;
using ProjectService.Domain.Task;

namespace ProjectService.Application.Common.Interfaces;

public interface ITaskItemRepository : IBaseRepository<Domain.Task.TaskItem,Guid>
{
    Task<Domain.Task.TaskItem?> FindTaskItemIncludeAllAsync(Guid id, CancellationToken cancellationToken);

    Task<(List<Domain.Task.TaskItem> projects, int count)> GetFilteredTaskItemsAsync(
        string? keyword,
        List<Guid>? assignedTo,
        DateTime? startDate,
        DateTime? endDate,
        TaskItemStatus? taskItemStatus,
        Guid? researcherId,
        Guid? managerId,
        PageQueryRequest pageQuery,
        CancellationToken cancellationToken = default);
}