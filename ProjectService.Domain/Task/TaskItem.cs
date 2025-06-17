using ProjectService.Domain.Common;

namespace ProjectService.Domain.Task;

public class TaskItem : BaseModel
{
    public Guid Id { get; private set; }
    public string Label { get; private set; } = string.Empty;
    public string Description { get; private set; } = string.Empty;
    public List<Guid> AssignedTo { get; private set; } = new();
    public DateTime StartDate { get; private set; }
    public DateTime EndDate { get; private set; }
    public List<string> PathsFiles { get; private set; } = new();
    public List<long> Resources { get; private set; } = new();
    public string ReviewNotes { get; private set; } = string.Empty;

    public List<Guid> PredecessorTaskIds { get; private set; } = new();
    
    public Guid StudyId { get; private set; }
    public Study.Study Study { get; private set; } 
    
    public void AddPathsFile(string pathsFile)
    {
        PathsFiles.Add(pathsFile);
    }
}