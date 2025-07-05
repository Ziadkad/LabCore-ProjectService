using MediatR;
using ProjectService.Application.Common.Exceptions;
using ProjectService.Application.Common.Extensions;
using ProjectService.Application.Common.Interfaces;

namespace ProjectService.Application.StudyResult.Commands;

public record DeleteStudyResultCommand(Guid Id) : IRequest;

public class DeleteStudyResultCommandHandler(IUserContext userContext, IUnitOfWork unitOfWork, IStudyResultRepository studyResultRepository) : IRequestHandler<DeleteStudyResultCommand>
{
    public async Task Handle(DeleteStudyResultCommand request, CancellationToken cancellationToken)
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
        studyResultRepository.Remove(studyResult);
        var isSaved = await unitOfWork.SaveChangesAsync(cancellationToken);
        if (isSaved <= 0)
        {
            throw new InternalServerException();
        }
    }
}