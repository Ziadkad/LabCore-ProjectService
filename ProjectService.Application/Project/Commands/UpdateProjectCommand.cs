using AutoMapper;
using MediatR;
using ProjectService.Application.Common.Exceptions;
using ProjectService.Application.Common.Interfaces;
using ProjectService.Application.Project.Models;
using ProjectService.Domain.Project;

namespace ProjectService.Application.Project.Commands;

public record UpdateProjectCommand(Guid Id, string Name, string Description, DateTime StartDate, DateTime EndDate, ProjectStatus Status, bool IsPublic, List<string> Tags, List<Guid> Researchers):IRequest<ProjectDto>;

public class UpdateProjectCommandHandler(IProjectRepository projectRepository, IUnitOfWork unitOfWork, IMapper mapper) : IRequestHandler<UpdateProjectCommand, ProjectDto>
{
    public async Task<ProjectDto> Handle(UpdateProjectCommand request, CancellationToken cancellationToken)
    {
        if (request.EndDate <= request.StartDate)
        {
            throw new BadRequestException("End date cannot be before start date");
        }
        Domain.Project.Project? project = await projectRepository.FindAsync(request.Id,cancellationToken);
        if (project is null)
        {
            throw new NotFoundException("Project", request.Id);
        }
        project.Update(request.Name, request.Description, request.StartDate, request.EndDate, request.Status, request.IsPublic, request.Tags, request.Researchers);
        var isSaved = await unitOfWork.SaveChangesAsync(cancellationToken);
        if (isSaved <= 0)
        {
            throw new InternalServerException();
        }
        return mapper.Map<ProjectDto>(project);
    }
}