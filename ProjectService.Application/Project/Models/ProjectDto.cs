using ProjectService.Domain.Project;

namespace ProjectService.Application.Project.Models;

public class ProjectDto
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public ProjectStatus Status { get; set; }
    public bool IsPublic { get; set; }
    public float ProgressPercentage { get; set; }
    public List<string> Tags { get; set; }
    public List<string> PathsFiles { get; set; }
    public List<Guid> Researchers { get; set; }
    public Guid ManagerId { get; set; }
    public Guid? QId { get; set; }

}