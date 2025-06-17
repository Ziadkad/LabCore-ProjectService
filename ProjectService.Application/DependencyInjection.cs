using System.Reflection;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using ProjectService.Application.Project;
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

    }
}