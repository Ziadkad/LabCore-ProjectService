using ProjectService.Domain.Common;

namespace ProjectService.Domain.Project;

public class Project : BaseModel
{
    public Guid Id { get; private set; }
    public string Name { get; private set; }
    public string Description { get; private set; }
    public DateTime StartDate { get; private set; }
    public DateTime EndDate { get; private set; }
    public ProjectStatus Status { get; private set; } = ProjectStatus.Draft;
    public bool IsPublic { get; private set; } = false;
    public float ProgressPercentage { get; private set; } = 0.0f;
    public List<string> Tags { get; private set; } = new();
    public List<string> PathsFiles { get; private set; } = new();
    
    public List<Study.Study> Studies { get; private set; } = new();
    public List<Guid> Researchers { get; private set; } = new();
    public Guid ManagerId { get; private set; }
    public Guid? QAId { get; private set; } = null;

    public Project(Guid id, string name, string description, DateTime startDate, DateTime endDate, bool isPublic, Guid managerId)
    {
        Id = id;
        Name = name;
        Description = description;
        StartDate = startDate;
        EndDate = endDate;
        IsPublic = isPublic;
        ManagerId = managerId;
    }

    public void AddTags(List<string> tags)
    {
        Tags.AddRange(tags);
    }

    public void AddPathsFile(string pathsFile)
    {
        PathsFiles.Add(pathsFile);
    }
    
    public void AddResearchers(List<Guid> researchers)
    {
        Researchers.AddRange(researchers);
    }

    public void ChangeStatus(ProjectStatus status)
    {
        Status = status;
    }
    
    public void Update(
        string name,
        string description,
        DateTime startDate,
        DateTime endDate,
        ProjectStatus status,
        bool isPublic,
        List<string> tags,
        List<Guid> researchers)
    {
        Name = name;
        Description = description;
        StartDate = startDate;
        EndDate = endDate;
        Status = status;
        IsPublic = isPublic;
        Tags = tags;
        Researchers = researchers;
    }
    
}