using AutoMapper;
using FluentValidation;
using MediatR;
using ProjectService.Application.Common.Exceptions;
using ProjectService.Application.Common.Extensions;
using ProjectService.Application.Common.Interfaces;
using ProjectService.Application.Common.Services.Interfaces;
using ProjectService.Application.TaskItem.Models;

namespace ProjectService.Application.TaskItem.Commands;

public record CreateTaskItemCommand(string Label, string Description, DateTime StartDate, DateTime EndDate,Guid StudyId) : IRequest<TaskItemDto>;

public class CreateTaskItemCommandValidator : AbstractValidator<CreateTaskItemCommand>
{
    public CreateTaskItemCommandValidator()
    {
        RuleFor(x => x.Label)
            .NotEmpty().WithMessage("Label is required.")
            .MinimumLength(3).WithMessage("Label must be at least 3 characters long.");

        RuleFor(x => x.Description)
            .NotEmpty().WithMessage("Description is required.");
        
        RuleFor(x => x.StartDate)
            .NotEmpty().WithMessage("Start date is required.");

        RuleFor(x => x.EndDate)
            .NotEmpty().WithMessage("End date is required.")
            .GreaterThan(x => x.StartDate).WithMessage("End date must be after start date.");

        RuleFor(x => x.StudyId)
            .NotEmpty().WithMessage("Study ID is required.");
    }
}

public class CreateTaskItemCommandHandler(
    IStudyRepository studyRepository, 
    IUserContext userContext, 
    IUnitOfWork unitOfWork, 
    ITaskItemRepository taskItemRepository, 
    IMapper mapper,
    IProjectService projectService) : IRequestHandler<CreateTaskItemCommand, TaskItemDto>
{
    public async Task<TaskItemDto> Handle(CreateTaskItemCommand request, CancellationToken cancellationToken)
    {
        Domain.Study.Study? study = await studyRepository.FindStudyIncludeAllAsync(request.StudyId, cancellationToken);
        if (study is null || study.IsArchived)
        {
            throw new NotFoundException("Study", request.StudyId);
        }
        if (study.Project is null || study.Project.IsArchived)
        {
            throw new NotFoundException("Project", study.ProjectId);
        }

        UserExtention.CheckProjectUserRoleandForbidden(study.Project, userContext.GetUserRole(),
            userContext.GetCurrentUserId());
        Guid taskId = Guid.NewGuid();
        Domain.Task.TaskItem taskItem = new Domain.Task.TaskItem(taskId,request.Label, request.Description, request.StartDate, request.EndDate,request.StudyId);
        await taskItemRepository.AddAsync(taskItem, cancellationToken); 
        await projectService.UpdateProjectProgressPercentage(taskItem.Study.Project.Id, cancellationToken);
        var isSaved = await unitOfWork.SaveChangesAsync(cancellationToken);
        if (isSaved <= 0)
        {
            throw new InternalServerException();
        }
        
        return mapper.Map<TaskItemDto>(taskItem);
    }
}