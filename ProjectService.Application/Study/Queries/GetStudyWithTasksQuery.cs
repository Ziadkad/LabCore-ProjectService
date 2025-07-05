using AutoMapper;
using MediatR;
using ProjectService.Application.Common.Exceptions;
using ProjectService.Application.Common.Extensions;
using ProjectService.Application.Common.Interfaces;
using ProjectService.Application.Common.Models;
using ProjectService.Application.Study.Models;
using ProjectService.Application.TaskItem.Models;
using ProjectService.Domain.Task;

namespace ProjectService.Application.Study.Queries;

public record GetStudyWithTasksQuery(Guid Id, string? Keyword, List<Guid>? AssignedTo, DateTime? StartDate, DateTime? EndDate, TaskItemStatus? TaskItemStatus, PageQueryRequest pageQuery) :IRequest<StudyWithTasksDto>;

public class GetStudyWithTasksQueryHandler(IStudyRepository studyRepository, IMapper mapper, IUserContext userContext) : IRequestHandler<GetStudyWithTasksQuery, StudyWithTasksDto>
{
    public async Task<StudyWithTasksDto> Handle(GetStudyWithTasksQuery request, CancellationToken cancellationToken)
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
        
        var filteredTasks = study.TaskItems.AsQueryable();

        if (!string.IsNullOrWhiteSpace(request.Keyword))
        {
            filteredTasks = filteredTasks.Where(t => t.Label.Contains(request.Keyword, StringComparison.OrdinalIgnoreCase) 
                                                     || t.Description.Contains(request.Keyword, StringComparison.OrdinalIgnoreCase));
        }

        if (request.AssignedTo is not null && request.AssignedTo.Any())
        {
            filteredTasks = filteredTasks.Where(t => t.AssignedTo.Any(id => request.AssignedTo.Contains(id)));
        }

        if (request.StartDate.HasValue)
        {
            filteredTasks = filteredTasks.Where(t => t.StartDate >= request.StartDate.Value);
        }

        if (request.EndDate.HasValue)
        {
            filteredTasks = filteredTasks.Where(t => t.EndDate <= request.EndDate.Value);
        }

        if (request.TaskItemStatus.HasValue)
        {
            filteredTasks = filteredTasks.Where(t => t.TaskItemStatus == request.TaskItemStatus);
        }
        
        filteredTasks = request.pageQuery.SortColumn?.ToLower() switch
        {
            "label" => request.pageQuery.SortAscending
                ? filteredTasks.OrderBy(t => t.Label)
                : filteredTasks.OrderByDescending(t => t.Label),

            "startdate" => request.pageQuery.SortAscending
                ? filteredTasks.OrderBy(t => t.StartDate)
                : filteredTasks.OrderByDescending(t => t.StartDate),

            "enddate" => request.pageQuery.SortAscending
                ? filteredTasks.OrderBy(t => t.EndDate)
                : filteredTasks.OrderByDescending(t => t.EndDate),

            "taskstatus" => request.pageQuery.SortAscending
                ? filteredTasks.OrderBy(t => t.TaskItemStatus)
                : filteredTasks.OrderByDescending(t => t.TaskItemStatus),

            _ => filteredTasks.OrderBy(t => t.EndDate)
        };
        

        // Apply pagination
        var totalCount = filteredTasks.Count();
        var pagedTasks = filteredTasks
            .Skip((request.pageQuery.Page - 1) * request.pageQuery.PageSize)
            .Take(request.pageQuery.PageSize)
            .ToList();
        
        StudyWithTasksDto studyWithTasks = mapper.Map<StudyWithTasksDto>(study);
        studyWithTasks.TaskItems = mapper.Map<List<TaskItemDto>>(pagedTasks);
        studyWithTasks.TaskCount = totalCount;
        return studyWithTasks;
    }
}