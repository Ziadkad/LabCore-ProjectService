using Microsoft.EntityFrameworkCore;
using ProjectService.Application.Common.Interfaces;
using ProjectService.Domain.StudyResult;
using ProjectService.Infrastructure.Data;

namespace ProjectService.Infrastructure.Repositories;

public class StudyResultRepository(AppDbContext dbContext) : BaseRepository<StudyResult,Guid>(dbContext),IStudyResultRepository
{
    public async Task<StudyResult?> FindStudyResultIncludeAllAsync(Guid id,CancellationToken cancellationToken)
    {
        return await DbSet.Include(sr => sr.Study)
            .ThenInclude(s => s.Project).FirstOrDefaultAsync(sr => sr.Id == id, cancellationToken);
    }
}