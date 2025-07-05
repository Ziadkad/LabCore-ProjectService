using Microsoft.EntityFrameworkCore;
using ProjectService.Application.Common.Interfaces;
using ProjectService.Application.Common.Models;
using ProjectService.Domain.Task;
using ProjectService.Infrastructure.Data;

namespace ProjectService.Infrastructure.Repositories;

public class TaskItemRepository(AppDbContext dbContext, IUserContext userContext) : BaseRepository<TaskItem,Guid>(dbContext),ITaskItemRepository
{
    public async Task<TaskItem?> FindTaskItemIncludeAllAsync(Guid id,CancellationToken cancellationToken = default)
    {
        return await DbSet.Include(t => t.Study)
            .ThenInclude(s => s.Project).FirstOrDefaultAsync(t => t.Id == id, cancellationToken);
    }


    public async Task<(List<TaskItem> projects, int count)> GetFilteredTaskItemsAsync(
        string? keyword,
        List<Guid>? assignedTo,
        DateTime? startDate,
        DateTime? endDate,
        TaskItemStatus? taskItemStatus,
        Guid? researcherId,
        Guid? managerId,
        PageQueryRequest pageQuery,
        CancellationToken cancellationToken = default)
    {
        var query = DbSet
            .Include(t => t.Study)
            .ThenInclude(s => s.Project)
            .AsQueryable();

        if (!string.IsNullOrWhiteSpace(keyword))
        {
            query = query.Where(t =>
                t.Label.Contains(keyword) ||
                t.Description.Contains(keyword));
        }


        if (startDate.HasValue)
        {
            query = query.Where(t => t.StartDate >= startDate.Value);
        }

        if (endDate.HasValue)
        {
            query = query.Where(t => t.EndDate <= endDate.Value);
        }

        if (taskItemStatus.HasValue)
        {
            query = query.Where(t => t.TaskItemStatus == taskItemStatus);
        }

        if (managerId is null && researcherId is null)
        {
            query = query.Where(t => t.Study.Project.IsPublic);
        }
        
        if (researcherId != null)
        {
            query = query.Where(t =>
                t.Study.Project.IsPublic ||
                t.Study.Project.Researchers.Contains(researcherId.Value));
        }

        if (managerId != null)
        {
            query = query.Where(t =>
                t.Study.Project.IsPublic ||
                t.Study.Project.ManagerId == managerId);
        }

        
        query = query.Where(t=>!t.Study.IsArchived && !t.Study.Project.IsArchived);
        
        var allItems = await query.ToListAsync(cancellationToken);

        if (assignedTo != null && assignedTo.Any())
        {
            allItems = allItems
                .Where(t => t.AssignedTo.Any(a => assignedTo.Contains(a)))
                .ToList();
        }
        int totalCount = allItems.Count;

        // Apply pagination manually
        var paginatedItems = allItems
            .Skip((pageQuery.Page - 1) * pageQuery.PageSize)
            .Take(pageQuery.PageSize)
            .ToList();

        return (paginatedItems, totalCount);
    }
    
}