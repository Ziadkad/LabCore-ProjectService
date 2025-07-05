using AutoMapper;
using FluentValidation;
using MediatR;
using ProjectService.Application.Common.Exceptions;
using ProjectService.Application.Common.Extensions;
using ProjectService.Application.Common.Interfaces;
using ProjectService.Application.StudyResult.Models;

namespace ProjectService.Application.StudyResult.Commands;

public record UpdateStudyResultCommand(Guid Id, string Result): IRequest<StudyResultDto>;

public class UpdateStudyResultCommandValidator : AbstractValidator<UpdateStudyResultCommand>
{
    
}

public class UpdateStudyResultCommandHandler(IUserContext userContext, IUnitOfWork unitOfWork, IMapper mapper, IStudyResultRepository studyResultRepository) : IRequestHandler<UpdateStudyResultCommand, StudyResultDto>
{
    public async Task<StudyResultDto> Handle(UpdateStudyResultCommand request, CancellationToken cancellationToken)
    {
        Domain.StudyResult.StudyResult?  studyResult = await studyResultRepository.FindStudyResultIncludeAllAsync(request.Id, cancellationToken);
        if (studyResult is null)
        {
            throw new NotFoundException("studyResult", request.Id);
        }
        if (studyResult.Study is null || studyResult.Study.IsArchived)
        {
            throw new NotFoundException("Study", studyResult.StudyId);
        }
        if (studyResult.Study.Project is null || studyResult.Study.Project.IsArchived)
        {
            throw new NotFoundException("Project", studyResult.Study.ProjectId);
        }
        UserExtention.CheckProjectUserRoleandForbidden(studyResult.Study.Project, userContext.GetUserRole(),userContext.GetCurrentUserId());
        studyResult.setResult(request.Result);
        var isSaved = await unitOfWork.SaveChangesAsync(cancellationToken);
        if (isSaved <= 0)
        {
            throw new InternalServerException();
        }
        return mapper.Map<StudyResultDto>(studyResult);
    }
}