using AutoMapper;
using ProjectService.Application.StudyResult.Models;

namespace ProjectService.Application.StudyResult;

public class StudyResultMapper : Profile
{
    public StudyResultMapper()
    {
        CreateMap<Domain.StudyResult.StudyResult, StudyResultDto>();
    }
}