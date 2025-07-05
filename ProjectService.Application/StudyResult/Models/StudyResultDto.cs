namespace ProjectService.Application.StudyResult.Models;

public class StudyResultDto
{
    public Guid Id { get; set; }
    public string Result { get; set; }
    public Guid StudyId { get; set; }
    public List<string> PathsFiles { get; set; }
}