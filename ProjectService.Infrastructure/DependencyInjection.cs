using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ProjectService.Application.Common.Interfaces;
using ProjectService.Infrastructure.Data;
using ProjectService.Infrastructure.ExternalServices.BrokerService;
using ProjectService.Infrastructure.ExternalServices.UserContext;
using ProjectService.Infrastructure.Repositories;

namespace ProjectService.Infrastructure;

public static class DependencyInjection
{
    public static void RegisterDataServices(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("DefaultConnection");
        services.AddDbContext<AppDbContext>(options =>
            options.UseSqlServer(connectionString));
        
        services.AddScoped<IUnitOfWork>(c => c.GetRequiredService<AppDbContext>());
        services.AddScoped<IProjectRepository, ProjectRepository>();
        services.AddScoped<IStudyRepository, StudyRepository>();
        services.AddScoped<ITaskItemRepository, TaskItemRepository>();
        services.AddScoped<IStudyResultRepository, StudyResultRepository>();
        
        
        services.AddHttpContextAccessor();
        services.AddScoped<IUserContext, UserContext>();
        
        
        services.RegisterBrokerServices(configuration);
    }
}