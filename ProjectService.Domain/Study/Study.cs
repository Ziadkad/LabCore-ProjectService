using ProjectService.Domain.Common;
using ProjectService.Domain.Task;

namespace ProjectService.Domain.Study;

public class Study : BaseModel
{
    public Guid Id { get; private set; }
    public string Title { get; private set; }
    public string Objective { get; private set; }
    public string Description { get; private set; }
    public DateTime StartDate { get; private set; }
    public DateTime EndDate { get; private set; }
    public RiskLevel RiskLevel { get; private set; }
    public List<string> PathsFiles { get; private set; } = new();
    public List<long> Resources { get; private set; } = new();
    public StudyResult.StudyResult?  StudyResult { get; private set; }

    public Guid ProjectId { get; private set; }
    public Project.Project Project { get; private set; }
    public List<TaskItem> TaskItems { get; private set; } = new();

    public Study(Guid id, string title, string objective, string description, DateTime startDate, DateTime endDate, RiskLevel riskLevel, Guid projectId)
    {
        Id = id;
        Title = title;
        Objective = objective;
        Description = description;
        StartDate = startDate;
        EndDate = endDate;
        RiskLevel = riskLevel;
        ProjectId = projectId;
    }

    public void AddPathsFile(string pathsFile)
    {
        PathsFiles.Add(pathsFile);
    }
    public void Update(string title, string objective, string description, DateTime startDate, DateTime endDate, RiskLevel riskLevel)
    {
        Title = title;
        Objective = objective;
        Description = description;
        StartDate = startDate;
        EndDate = endDate;
        RiskLevel = riskLevel;
    }
}