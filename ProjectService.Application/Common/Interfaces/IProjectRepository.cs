using ProjectService.Application.Common.Models;
using ProjectService.Domain.Project;

namespace ProjectService.Application.Common.Interfaces;

public interface IProjectRepository : IBaseRepository<Domain.Project.Project, Guid>
{
    Task<bool> ExistsByNameAsync(string name, CancellationToken cancellationToken = default);
    Task<bool> ExistsByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<(List<Domain.Project.Project> projects, int count)> FindAllAsyncWithFilters(string? keyword, DateTime? startDate, DateTime? endDate, ProjectStatus? status, float? progressPercentageMin, float? progressPercentageMax, List<string>? tags, List<Guid>? researchers, Guid? manager, PageQueryRequest pageQuery, bool connected, CancellationToken cancellationToken = default);
    Task<Domain.Project.Project?> GetProjectByIdIncludeStudiesAll(Guid id, CancellationToken cancellationToken = default);
}