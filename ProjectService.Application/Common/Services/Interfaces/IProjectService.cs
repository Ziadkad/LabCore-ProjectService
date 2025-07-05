namespace ProjectService.Application.Common.Services.Interfaces;

public interface IProjectService
{
    Task UpdateProjectProgressPercentage(Guid projectId, CancellationToken cancellationToken);
}