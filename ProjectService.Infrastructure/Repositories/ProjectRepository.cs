using Microsoft.EntityFrameworkCore;
using ProjectService.Application.Common.Interfaces;
using ProjectService.Application.Common.Models;
using ProjectService.Domain.Project;
using ProjectService.Infrastructure.Data;

namespace ProjectService.Infrastructure.Repositories;

public class ProjectRepository(AppDbContext dbContext, IUserContext userContext) : BaseRepository<Project,Guid>(dbContext),IProjectRepository
{
    public async Task<bool> ExistsByNameAsync(string name, CancellationToken cancellationToken = default)
    {
        return await DbSet.AnyAsync(u => u.Name == name, cancellationToken);
    }

    public async Task<(List<Project> projects, int count)> FindAllAsyncWithFilters(
        string? keyword,
        DateTime? startDate,
        DateTime? endDate,
        ProjectStatus? status,
        float? progressPercentageMin,
        float? progressPercentageMax,
        List<string>? tags,
        List<Guid>? researchers,
        Guid? manager,
        PageQueryRequest pageQuery,
        bool connected,
        CancellationToken cancellationToken = default)
    {
        IQueryable<Project> query = DbSet.AsQueryable();

        // Keyword filter
        if (!string.IsNullOrWhiteSpace(keyword))
        {
            query = query.Where(p => p.Name.Contains(keyword) || p.Description.Contains(keyword));
        }

        // Date filters
        if (startDate.HasValue)
            query = query.Where(p => p.StartDate >= startDate.Value);
        if (endDate.HasValue)
            query = query.Where(p => p.EndDate <= endDate.Value);

        // Status filter
        if (status.HasValue)
            query = query.Where(p => p.Status == status.Value);

        // Progress percentage filters
        if (progressPercentageMin.HasValue)
            query = query.Where(p => p.ProgressPercentage >= progressPercentageMin.Value);
        if (progressPercentageMax.HasValue)
            query = query.Where(p => p.ProgressPercentage <= progressPercentageMax.Value);

        // Tags filter
        if (tags != null && tags.Any())
        {
            query = query.Where(p => p.Tags.Any(tag => tags.Contains(tag)));
        }

        // Researchers filter
        if (researchers != null && researchers.Any())
        {
            query = query.Where(p => p.Researchers.Any(r => researchers.Contains(r)));
        }

        // Manager filter
        if (manager.HasValue && manager != Guid.Empty)
        {
            query = query.Where(p => p.ManagerId == manager.Value);
        }

        
        query = query.Where(p => !p.IsArchived);

        if (connected)
        {
            var userId = userContext.GetCurrentUserId();
            var userRole = userContext.GetUserRole();
        
            if (userRole == UserRole.Manager)
            {
                query = query.Where(p => p.IsPublic || p.ManagerId == userId);
            }
            else if (userRole == UserRole.Researcher)
            {
                query = query.Where(p => p.IsPublic || p.Researchers.Contains(userId));
            }
        }

        
        int totalCount = await query.CountAsync(cancellationToken);

        var projects = await query
            .Skip((pageQuery.Page - 1) * pageQuery.PageSize)
            .Take(pageQuery.PageSize)
            .ToListAsync(cancellationToken);
        
        return (projects, totalCount);
    }


}