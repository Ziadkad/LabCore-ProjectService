using ProjectService.Application.Common.Exceptions;
using ProjectService.Application.Common.Interfaces;
using ProjectService.Application.Common.Services.Interfaces;

namespace ProjectService.Application.Common.Services;

public class ProjectService(IProjectRepository projectRepository) : IProjectService
{
    public async Task UpdateProjectProgressPercentage(Guid projectId, CancellationToken cancellationToken)
    {
        Domain.Project.Project? project = await projectRepository.GetProjectByIdIncludeStudiesAll(projectId,cancellationToken);
        if (project is null || project.IsArchived)
        {
            throw new NotFoundException("Project", projectId);
        }
        project.UpdateProjectProgressPercentage();
    }
}