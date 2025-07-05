
namespace ProjectService.Application.Common.Interfaces;

public interface IStudyRepository : IBaseRepository<Domain.Study.Study,Guid>
{
    Task<Domain.Study.Study?> FindStudyIncludeAllAsync(Guid id, CancellationToken cancellationToken = default);
}