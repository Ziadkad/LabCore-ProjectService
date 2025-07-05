using MediatR;
using ProjectService.Application.Common.Exceptions;
using ProjectService.Application.Common.Extensions;
using ProjectService.Application.Common.Interfaces;

namespace ProjectService.Application.Study.Commands;

public record DeleteStudyCommand(Guid Id): IRequest;

public class DeleteStudyCommandHandler(IStudyRepository studyRepository, IUnitOfWork unitOfWork, IUserContext userContext): IRequestHandler<DeleteStudyCommand>
{
    public async Task Handle(DeleteStudyCommand request, CancellationToken cancellationToken)
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
        study.SetArchived();
        var isSaved = await unitOfWork.SaveChangesAsync(cancellationToken);
        if (isSaved <= 0)
        {
            throw new InternalServerException();
        }
    }
}