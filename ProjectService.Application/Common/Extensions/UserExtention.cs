using ProjectService.Application.Common.Exceptions;
using ProjectService.Application.Common.Models;

namespace ProjectService.Application.Common.Extensions;

public static class UserExtention
{
    public static void CheckProjectUserRoleandNotFound(Domain.Project.Project project, UserRole userRole, Guid userId)
    {
        if (!project.IsPublic)
        {
            if (userRole == UserRole.Manager && project.ManagerId != userId)
            {
                throw new NotFoundException("Project", project.Id);
            }

            if (userRole == UserRole.Researcher && (project.Researchers == null || !project.Researchers.Contains(userId)))
            {
                throw new NotFoundException("Project", project.Id);
            }
        }
    }
    public static void CheckProjectUserRoleandForbidden(Domain.Project.Project project, UserRole userRole, Guid userId)
    {
        if (userRole == UserRole.Manager && project.ManagerId != userId)
        {
            throw new ForbiddenException();
        }

        if (userRole == UserRole.Researcher && (project.Researchers == null || !project.Researchers.Contains(userId)))
        {
            throw new ForbiddenException();
        }
    }
    
}