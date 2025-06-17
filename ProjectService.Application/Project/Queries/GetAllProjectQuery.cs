using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using ProjectService.Application.Common.Interfaces;
using ProjectService.Application.Common.Models;
using ProjectService.Application.Project.Models;
using ProjectService.Domain.Project;

namespace ProjectService.Application.Project.Queries;

public record GetAllProjectQuery(string? Keyword, DateTime? StartDate, DateTime? EndDate, ProjectStatus? Status, float? ProgressPercentageMin,
float? ProgressPercentageMax, List<string>? Tags, List<Guid>? Researchers, Guid? Manager, PageQueryRequest PageQuery) : IRequest<PageQueryResult<ProjectDto>>;

public class GetAllProjectQueryHandler(IProjectRepository projectRepository, IMapper mapper, IUserContext userContext, ILogger<GetAllProjectQueryHandler> logger) : IRequestHandler<GetAllProjectQuery, PageQueryResult<ProjectDto>>
{
    public async Task<PageQueryResult<ProjectDto>> Handle(GetAllProjectQuery request, CancellationToken cancellationToken)
    {
        (List<Domain.Project.Project> projects, int count) = await projectRepository.FindAllAsyncWithFilters(request.Keyword, request.StartDate,request.EndDate, request.Status, request.ProgressPercentageMin, request.ProgressPercentageMax, request.Tags, request.Researchers, request.Manager, request.PageQuery,true, cancellationToken);
        
        List<ProjectDto> projectDtos = mapper.Map<List<ProjectDto>>(projects);
        return new PageQueryResult<ProjectDto>(projectDtos, count);
    }
}
