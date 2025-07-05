using System.Reflection;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using ProjectService.Application.Common.Services.Interfaces;
using ProjectService.Application.Project;
using ProjectService.Application.Study;
using ProjectService.Application.StudyResult;
using ProjectService.Application.TaskItem;
using ProjectService.Domain.Common;

namespace ProjectService.Application;

public static class DependencyInjection
{
    public static void RegisterApplicationServices(
        this IServiceCollection services)
    {
        services.AddMediatR(x => x.RegisterServicesFromAssemblies(Assembly.GetExecutingAssembly(), Assembly.GetAssembly(typeof(BaseModel))!));
        services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
        services.AddAutoMapper(typeof(ProjectMapper).Assembly);
        services.AddAutoMapper(typeof(StudyMapper).Assembly);
        services.AddAutoMapper(typeof(TaskItemMapper).Assembly);
        services.AddAutoMapper(typeof(StudyResultMapper).Assembly);


        services.AddScoped<IProjectService, Common.Services.ProjectService>();
    }
}