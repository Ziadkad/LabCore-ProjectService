using AutoMapper;
using FluentValidation;
using MediatR;
using ProjectService.Application.Common.Exceptions;
using ProjectService.Application.Common.Extensions;
using ProjectService.Application.Common.Interfaces;
using ProjectService.Application.Common.Services.Interfaces;
using ProjectService.Application.TaskItem.Models;
using ProjectService.Domain.Task;

namespace ProjectService.Application.TaskItem.Commands;

public record UpdateTaskItemCommand(Guid Id, string Label, string Description, List<Guid> AssignedTo, DateTime StartDate, DateTime EndDate, TaskItemStatus TaskItemStatus, string? ReviewNotes) : IRequest<TaskItemDto>;

public class UpdateTaskItemCommandValidator : AbstractValidator<UpdateTaskItemCommand>
{
    public UpdateTaskItemCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("TaskItem ID is required.");

        RuleFor(x => x.Label)
            .NotEmpty().WithMessage("Label is required.")
            .MinimumLength(3).WithMessage("Label must be at least 3 characters long.");

        RuleFor(x => x.Description)
            .NotEmpty().WithMessage("Description is required.");

        RuleFor(x => x.AssignedTo)
            .NotNull().WithMessage("AssignedTo list is required.")
            .Must(list => list.Count > 0).WithMessage("At least one researcher must be assigned.");

        RuleFor(x => x.StartDate)
            .NotEmpty().WithMessage("Start date is required.");

        RuleFor(x => x.EndDate)
            .NotEmpty().WithMessage("End date is required.")
            .GreaterThan(x => x.StartDate).WithMessage("End date must be after start date.");

        RuleFor(x => x.ReviewNotes)
            .MaximumLength(1000).WithMessage("Review notes must be 1000 characters or fewer.");
    }
}


public class UpdateTaskItemCommandHandler(ITaskItemRepository taskItemRepository, IUnitOfWork unitOfWork, IMapper mapper, IUserContext userContext, IProjectService projectService) : IRequestHandler<UpdateTaskItemCommand, TaskItemDto>
{
    public async Task<TaskItemDto> Handle(UpdateTaskItemCommand request, CancellationToken cancellationToken)
    {
        Domain.Task.TaskItem? taskItem = await taskItemRepository.FindTaskItemIncludeAllAsync(request.Id, cancellationToken);
        if (taskItem is null)
        {
            throw new NotFoundException("TaskItem", request.Id);
        }
        if (taskItem.Study is null || taskItem.Study.IsArchived)
        {
            throw new NotFoundException("Study", taskItem.StudyId);
        }
        if (taskItem.Study.Project is null || taskItem.Study.Project.IsArchived)
        {
            throw new NotFoundException("Project", taskItem.Study.ProjectId);
        }
        UserExtention.CheckProjectUserRoleandForbidden(taskItem.Study.Project, userContext.GetUserRole(),userContext.GetCurrentUserId());
        if (request.AssignedTo.Any())
        {
            foreach (var researcher in taskItem.AssignedTo)
            {
                if (!taskItem.Study.Project.Researchers.Contains(researcher))
                {
                    throw new BadRequestException($"This researcher with key {researcher} does not have access to this project");
                }
            }
        }

        taskItem.Update(request.Label,request.Description,request.AssignedTo,request.StartDate,request.EndDate,request.TaskItemStatus,request.ReviewNotes);
        await projectService.UpdateProjectProgressPercentage(taskItem.Study.Project.Id, cancellationToken);
        var isSaved = await unitOfWork.SaveChangesAsync(cancellationToken);
        if (isSaved <= 0)
        {
            throw new InternalServerException();
        }
        return mapper.Map<TaskItemDto>(taskItem);
    }
}