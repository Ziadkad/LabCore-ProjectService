using AutoMapper;
using FluentValidation;
using MediatR;
using ProjectService.Application.Common.Exceptions;
using ProjectService.Application.Common.Interfaces;
using ProjectService.Application.Common.Models;
using ProjectService.Application.Project.Models;
using ProjectService.Domain.Project;

namespace ProjectService.Application.Project.Commands;

public record UpdateProjectCommand(Guid Id, string Name, string Description, DateTime StartDate, DateTime EndDate, ProjectStatus Status, bool IsPublic, List<string> Tags, List<Guid> Researchers):IRequest<ProjectDto>;


public class UpdateProjectCommandValidator : AbstractValidator<UpdateProjectCommand>
{
    public UpdateProjectCommandValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Name is required")
            .MaximumLength(200).WithMessage("Name must not exceed 200 characters");

        RuleFor(x => x.Description)
            .NotEmpty().WithMessage("Description is required");

        RuleFor(x => x.StartDate)
            .LessThan(x => x.EndDate).WithMessage("Start date must be before end date");

        RuleFor(x => x.EndDate)
            .GreaterThan(x => x.StartDate).WithMessage("End date must be after start date");
    }
}
public class UpdateProjectCommandHandler(IProjectRepository projectRepository, IUnitOfWork unitOfWork, IMapper mapper,IUserContext userContext) : IRequestHandler<UpdateProjectCommand, ProjectDto>
{
    public async Task<ProjectDto> Handle(UpdateProjectCommand request, CancellationToken cancellationToken)
    {
        Domain.Project.Project? project = await projectRepository.FindAsync(request.Id,cancellationToken);
        if (project is null)
        {
            throw new NotFoundException("Project", request.Id);
        }
        if (project.ManagerId != userContext.GetCurrentUserId())
        {
            throw new ForbiddenException();
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