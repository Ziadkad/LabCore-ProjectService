using System.Runtime.InteropServices.ComTypes;
using AutoMapper;
using FluentValidation;
using MediatR;
using ProjectService.Application.Common.Exceptions;
using ProjectService.Application.Common.Extensions;
using ProjectService.Application.Common.Interfaces;
using ProjectService.Application.Study.Models;
using ProjectService.Domain.Study;

namespace ProjectService.Application.Study.Commands;

public record UpdateStudyCommand(Guid Id, string Title, string Objective, string Description, DateTime StartDate, DateTime EndDate, RiskLevel RiskLevel): IRequest<StudyDto>;
public class UpdateStudyCommandValidator : AbstractValidator<UpdateStudyCommand>
{
    public UpdateStudyCommandValidator()
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

public class UpdateStudyCommandHandler(IStudyRepository studyRepository, IMapper mapper, IUnitOfWork unitOfWork, IUserContext userContext): IRequestHandler<UpdateStudyCommand, StudyDto>
{
    public async Task<StudyDto> Handle(UpdateStudyCommand request, CancellationToken cancellationToken)
    {
        Domain.Study.Study? study = await studyRepository.FindStudyIncludeAllAsync(request.Id, cancellationToken);
        if (study is null || study.IsArchived)
        {
            throw new NotFoundException("Study", request.Id);
        }
        if (study.Project is null || study.Project.IsArchived)
        {
            throw new NotFoundException("Project", study.ProjectId);
        }
        UserExtention.CheckProjectUserRoleandForbidden(study.Project, userContext.GetUserRole(), userContext.GetCurrentUserId());
        study.Update(request.Title, request.Objective,request.Description, request.StartDate, request.EndDate, request.RiskLevel);
        var isSaved = await unitOfWork.SaveChangesAsync(cancellationToken);
        if (isSaved <= 0)
        {
            throw new InternalServerException();
        }
        return mapper.Map<StudyDto>(study);
    }
}