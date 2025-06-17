using ProjectService.Domain.Common;
using ProjectService.Domain.Task;

namespace ProjectService.Domain.Study;

public class Study : BaseModel
{
    public Guid Id { get; private set; }
    public string Title { get; private set; } = string.Empty;
    public string Objective { get; private set; } = string.Empty;
    public string Description { get; private set; } = string.Empty;
    public DateTime StartDate { get; private set; }
    public DateTime EndDate { get; private set; }
    public RiskLevel RiskLevel { get; private set; }
    public List<string> PathsFiles { get; private set; } = new();
    public List<long> Resources { get; private set; } = new();
    public StudyResult.StudyResult?  StudyResult { get; private set; }

    public Guid ProjectId { get; private set; }
    public Project.Project Project { get; private set; }
    public List<TaskItem> TaskItems { get; private set; } = new();
    
    
    public void AddPathsFile(string pathsFile)
    {
        PathsFiles.Add(pathsFile);
    }
}