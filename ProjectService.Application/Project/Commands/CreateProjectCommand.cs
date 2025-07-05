using AutoMapper;
using FluentValidation;
using MediatR;
using ProjectService.Application.Common.Exceptions;
using ProjectService.Application.Common.Interfaces;
using ProjectService.Application.Common.Models;
using ProjectService.Application.Project.Models;

namespace ProjectService.Application.Project.Commands;

public record CreateProjectCommand(string Name, string Description, DateTime StartDate, DateTime EndDate, bool IsPublic) : IRequest<ProjectDto>;

public class CreateProjectCommandValidator : AbstractValidator<CreateProjectCommand>
{
    public CreateProjectCommandValidator()
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
public class CreateProjectCommandHandler(
    IUserContext userContext,
    IUnitOfWork unitOfWork,
    IProjectRepository projectRepository,
    IMapper mapper) : IRequestHandler<CreateProjectCommand, ProjectDto>
{
    public async Task<ProjectDto> Handle(CreateProjectCommand request, CancellationToken cancellationToken)
    {
        bool exists = await projectRepository.ExistsByNameAsync(request.Name,cancellationToken);
        if (exists)
        {
            throw new BadRequestException("Project With this Name already exists");
        }
        Guid projectId = Guid.NewGuid();
        Domain.Project.Project project = new Domain.Project.Project(projectId, request.Name, request.Description,
            request.StartDate, request.EndDate, request.IsPublic, userContext.GetCurrentUserId());
        await projectRepository.AddAsync(project, cancellationToken);
        var isSaved = await unitOfWork.SaveChangesAsync(cancellationToken);
        if (isSaved <= 0)
        {
            throw new InternalServerException();
        }
        return mapper.Map<ProjectDto>(project);
    }
}
