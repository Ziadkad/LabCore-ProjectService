using ProjectService.Domain.Task;

namespace ProjectService.Application.TaskItem.Models;

public class TaskItemDto
{
    public Guid Id { get; set; }
    public string Label { get; set; }
    public string Description { get; set; }
    public List<Guid> AssignedTo { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public TaskItemStatus TaskItemStatus { get; set; }
    public List<string> PathsFiles { get; set; }
    public List<long> Resources { get; set; }
    public string ReviewNotes { get; set; }
    public Guid StudyId {get; set; }
}