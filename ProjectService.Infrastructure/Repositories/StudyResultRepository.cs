using ProjectService.Application.Common.Interfaces;
using ProjectService.Domain.StudyResult;
using ProjectService.Infrastructure.Data;

namespace ProjectService.Infrastructure.Repositories;

public class StudyResultRepository(AppDbContext dbContext) : BaseRepository<StudyResult,Guid>(dbContext),IStudyResultRepository
{
    
}