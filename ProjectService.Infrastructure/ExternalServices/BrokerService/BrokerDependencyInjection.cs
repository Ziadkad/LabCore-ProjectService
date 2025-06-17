using MassTransit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ProjectService.Infrastructure.ExternalServices.BrokerService.Consumers;

namespace ProjectService.Infrastructure.ExternalServices.BrokerService;

public static class BrokerDependencyInjection
{
    public static void RegisterBrokerServices(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddMassTransit(x =>
        {
            var brokerSettings = configuration.GetSection("BrokerSettings");
            var brokerUsername = brokerSettings["Username"];
            var brokerPassword = brokerSettings["Password"];
            var brokerHost = brokerSettings["Host"];
            string addFileQueue = brokerSettings["AddFileEndpoint"];
            string deleteFileQueue = brokerSettings["DeleteFileEndpoint"];
            x.AddConsumer<FileServiceConsumer>();
            x.AddConsumer<DeleteFileConsumer>();

            x.UsingRabbitMq((context, cfg) =>
            {
                cfg.Host(brokerHost, "/", h =>
                {
                    h.Username(brokerUsername);
                    h.Password(brokerPassword);
                });
                
                cfg.ReceiveEndpoint(addFileQueue, e =>
                {
                    e.ConfigureConsumer<FileServiceConsumer>(context);
                });

                cfg.ReceiveEndpoint(deleteFileQueue, e =>
                {
                    e.ConfigureConsumer<DeleteFileConsumer>(context);
                });
            });
        });

        services.AddMassTransitHostedService();

    }
}