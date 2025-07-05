using ProjectService.Domain.Common;

namespace ProjectService.Domain.StudyResult;

public class StudyResult : BaseModel
{
    public Guid Id { get; private set; }
    public string Result { get; private set; }
    public Guid StudyId { get; private set; }
    public Study.Study Study { get; private set; }
    public List<string> PathsFiles { get; private set; } = new();

    public StudyResult(Guid id, string result, Guid studyId)
    {
        Id = id;
        Result = result;
        StudyId = studyId;
    }

    public void setResult (string result)
    {
        Result = result;
    }

    public void AddPathsFile(string pathsFile)
    {
        PathsFiles.Add(pathsFile);
    }
}