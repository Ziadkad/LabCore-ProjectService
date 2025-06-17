using ProjectService.Application.Common.Interfaces;
using ProjectService.Domain.Study;
using ProjectService.Infrastructure.Data;

namespace ProjectService.Infrastructure.Repositories;

public class StudyRepository(AppDbContext dbContext) : BaseRepository<Study,Guid>(dbContext),IStudyRepository
{
    
}