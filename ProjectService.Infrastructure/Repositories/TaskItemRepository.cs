using ProjectService.Application.Common.Interfaces;
using ProjectService.Domain.Task;
using ProjectService.Infrastructure.Data;

namespace ProjectService.Infrastructure.Repositories;

public class TaskItemRepository(AppDbContext dbContext) : BaseRepository<TaskItem,Guid>(dbContext),ITaskItemRepository
{
    
}