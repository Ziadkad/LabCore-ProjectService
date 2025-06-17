using ProjectService.Domain.Task;

namespace ProjectService.Application.Common.Interfaces;

public interface ITaskItemRepository : IBaseRepository<TaskItem,Guid>
{
    
}