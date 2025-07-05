using AutoMapper;
using MediatR;
using ProjectService.Application.Common.Exceptions;
using ProjectService.Application.Common.Extensions;
using ProjectService.Application.Common.Interfaces;
using ProjectService.Application.Study.Models;

namespace ProjectService.Application.Study.Queries;

public record GetStudyQuery(Guid Id)  : IRequest<StudyDto>;

public class GetStudyQueryHandler(IStudyRepository studyRepository, IUserContext userContext, IMapper mapper) : IRequestHandler<GetStudyQuery, StudyDto>
{
    public async Task<StudyDto> Handle(GetStudyQuery request, CancellationToken cancellationToken)
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
        UserExtention.CheckProjectUserRoleandNotFound(study.Project, userContext.GetUserRole(), userContext.GetCurrentUserId());
        return mapper.Map<StudyDto>(study);
    }
}