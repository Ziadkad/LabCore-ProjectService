using ProjectService.Domain.Common;

namespace ProjectService.Domain.StudyResult;

public class StudyResult : BaseModel
{
    public Guid Id { get; private set; }
    public string Result { get; private set; } = string.Empty;
    public Guid StudyId { get; private set; }
    public Study.Study Study { get; private set; }
    public List<string> PathsFiles { get; private set; } = new();
    
    
    public void AddPathsFile(string pathsFile)
    {
        PathsFiles.Add(pathsFile);
    }
}