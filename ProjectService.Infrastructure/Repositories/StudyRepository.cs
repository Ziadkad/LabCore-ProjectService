using Microsoft.EntityFrameworkCore;
using ProjectService.Application.Common.Interfaces;
using ProjectService.Application.Study.Models;
using ProjectService.Domain.Study;
using ProjectService.Infrastructure.Data;

namespace ProjectService.Infrastructure.Repositories;

public class StudyRepository(AppDbContext dbContext) : BaseRepository<Study,Guid>(dbContext),IStudyRepository
{
    public async Task<Study?> FindStudyIncludeAllAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await DbSet
            .Include(s => s.TaskItems)
            .Include(s => s.StudyResult)
            .Include(s => s.Project)
            .FirstOrDefaultAsync(s => s.Id == id, cancellationToken);
    }
    

}