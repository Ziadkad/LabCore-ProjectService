using ProjectService.Domain.Common;

namespace ProjectService.Domain.Task;

public class TaskItem : BaseModel
{
    public Guid Id { get; private set; }
    public string Label { get; private set; }
    public string Description { get; private set; }
    public List<Guid> AssignedTo { get; private set; } = new();
    public DateTime StartDate { get; private set; }
    public DateTime EndDate { get; private set; }
    public TaskItemStatus TaskItemStatus { get; private set; } = TaskItemStatus.NotStarted;
    public List<string> PathsFiles { get; private set; } = new();
    public List<long> Resources { get; private set; } = new();
    public string ReviewNotes { get; private set; } = string.Empty;
    public Guid StudyId { get; private set; }
    public Study.Study Study { get; private set; } 
    
    public void AddPathsFile(string pathsFile)
    {
        PathsFiles.Add(pathsFile);
    }

    public TaskItem(Guid id, string label, string description, DateTime startDate, DateTime endDate, Guid studyId)
    {
        Id = id;
        Label = label;
        Description = description;
        StartDate = startDate;
        EndDate = endDate;
        StudyId = studyId;
    }

    public void Update( string label, string description, List<Guid> assignedTo, DateTime startDate, DateTime endDate, TaskItemStatus taskItemStatus, string reviewNotes)
    {
      Label = label;
      Description = description;
      AssignedTo = assignedTo;
      StartDate = startDate;
      EndDate = endDate;
      TaskItemStatus = taskItemStatus;
      ReviewNotes = reviewNotes;
    }

    public void setStatus(TaskItemStatus taskItemStatus)
    {
        TaskItemStatus = taskItemStatus;
    }
    
}