using AutoMapper;
using FluentValidation;
using MediatR;
using ProjectService.Application.Common.Exceptions;
using ProjectService.Application.Common.Extensions;
using ProjectService.Application.Common.Interfaces;
using ProjectService.Application.StudyResult.Models;

namespace ProjectService.Application.StudyResult.Commands;

public record CreateStudyResultCommand(string Result, Guid StudyId):IRequest<StudyResultDto>;

public class CreateStudyResultCommandValidator: AbstractValidator<CreateStudyResultCommand>
{

}

public class CreateStudyResultCommandHandler(IUserContext userContext, IUnitOfWork unitOfWork, IMapper mapper, IStudyResultRepository studyResultRepository, IStudyRepository studyRepository)  : IRequestHandler<CreateStudyResultCommand, StudyResultDto>
{
    public async Task<StudyResultDto> Handle(CreateStudyResultCommand request, CancellationToken cancellationToken)
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

        UserExtention.CheckProjectUserRoleandForbidden(study.Project, userContext.GetUserRole(), userContext.GetCurrentUserId());
        if (study.StudyResult is not null)
        {
            throw new BadRequestException("This Study already have a result");
        }
        Guid id = Guid.NewGuid();
        Domain.StudyResult.StudyResult studyResult = new Domain.StudyResult.StudyResult(id, request.Result, request.StudyId);
        await studyResultRepository.AddAsync(studyResult, cancellationToken);
        var isSaved = await unitOfWork.SaveChangesAsync(cancellationToken);
        if (isSaved <= 0)
        {
            throw new InternalServerException();
        }
        
        return mapper.Map<StudyResultDto>(studyResult);
    }
}