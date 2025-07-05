
namespace ProjectService.Application.Common.Interfaces;

public interface IStudyResultRepository : IBaseRepository<Domain.StudyResult.StudyResult,Guid>
{
    Task<Domain.StudyResult.StudyResult?> FindStudyResultIncludeAllAsync(Guid id, CancellationToken cancellationToken);
}