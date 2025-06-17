using ProjectService.Application.Common.Models;

namespace Shared.Models;

public class FileMessage(string path, Guid projectId, Guid? studyId, Guid? taskId, FileContext context)
{
    public string Path { get; set; } = path;
    public Guid ProjectId { get; set; } = projectId;
    public Guid? StudyId { get; set; } = studyId;
    public Guid? TaskId { get; set; } = taskId;
    public FileContext Context { get; set; } = context;
}