using AutoMapper;
using ProjectService.Application.Study.Models;

namespace ProjectService.Application.Study;

public class StudyMapper :Profile
{
    public StudyMapper()
    {
        CreateMap<Domain.Study.Study , StudyDto>();
        CreateMap<Domain.Study.Study, StudyWithTasksDto>();
        CreateMap<Domain.Study.Study, StudyWithResultDto>();
    }
}