using ProjectService.Application.StudyResult.Models;
using ProjectService.Domain.Study;

namespace ProjectService.Application.Study.Models;

public class StudyWithResultDto
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
    public StudyResultDto StudyResult { get; set; }
}