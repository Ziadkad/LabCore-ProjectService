using AutoMapper;
using FluentValidation;
using MediatR;
using ProjectService.Application.Common.Exceptions;
using ProjectService.Application.Common.Extensions;
using ProjectService.Application.Common.Interfaces;
using ProjectService.Application.Common.Models;
using ProjectService.Application.Study.Models;
using ProjectService.Domain.Study;

namespace ProjectService.Application.Study.Commands;

public record CreateStudyCommand(string Title, string Objective, string Description, DateTime StartDate, DateTime EndDate, RiskLevel RiskLevel, Guid ProjectId):IRequest<StudyDto>;

public class CreateStudyCommandValidator : AbstractValidator<CreateStudyCommand>
{
    public CreateStudyCommandValidator()
    {
        RuleFor(x => x.Title)
            .NotEmpty().WithMessage("Title is required")
            .MaximumLength(200).WithMessage("Title must not exceed 200 characters");

        RuleFor(x => x.Objective)
            .NotEmpty().WithMessage("Objective is required");
        
        RuleFor(x => x.Description)
            .NotEmpty().WithMessage("Description is required");

        RuleFor(x => x.StartDate)
            .LessThan(x => x.EndDate).WithMessage("Start date must be before end date");

        RuleFor(x => x.EndDate)
            .GreaterThan(x => x.StartDate).WithMessage("End date must be after start date");
    }
}

public class CreateStudyCommandHandler(IStudyRepository studyRepository,IProjectRepository projectRepository, IUnitOfWork unitOfWork, IUserContext userContext, IMapper mapper) : IRequestHandler<CreateStudyCommand, StudyDto>
{
    public async Task<StudyDto> Handle(CreateStudyCommand request, CancellationToken cancellationToken)
    {
        Domain.Project.Project? project = await projectRepository.FindAsync(request.ProjectId, cancellationToken);
        if (project is null)
        {
            throw new NotFoundException("Project", request.ProjectId);
        }
        UserExtention.CheckProjectUserRoleandForbidden(project, userContext.GetUserRole(),  userContext.GetCurrentUserId());
        Guid studyId = Guid.NewGuid();
        Domain.Study.Study study = new Domain.Study.Study(studyId, request.Title, request.Objective, request.Description, request.StartDate, request.EndDate, request.RiskLevel, request.ProjectId);
        await studyRepository.AddAsync(study, cancellationToken);
        var isSaved = await unitOfWork.SaveChangesAsync(cancellationToken);
        if (isSaved <= 0)
        {
            throw new InternalServerException();
        }

        return mapper.Map<StudyDto>(study);
    }
}