using ProjectService.Application.TaskItem.Models;
using ProjectService.Domain.Study;

namespace ProjectService.Application.Study.Models;

public class StudyWithTasksDto
{
    public Guid Id { get; set; }
    public string Title { get; set; }
    public string Objective { get; set; }
    public string Description { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public RiskLevel RiskLevel { get; set; }
    public List<string> PathsFiles { get; set; }
    public List<long> Resources { get; set; }
    public Guid ProjectId { get; set; }
    public List<TaskItemDto> TaskItems { get; set; }
    public int TaskCount { get; set; }
}