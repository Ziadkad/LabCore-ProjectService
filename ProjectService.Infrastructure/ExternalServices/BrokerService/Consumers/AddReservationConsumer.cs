using MassTransit;
using Microsoft.Extensions.Logging;
using ProjectService.Application.Common.Exceptions;
using ProjectService.Application.Common.Interfaces;
using ProjectService.Domain.Task;
using ProjectService.Infrastructure.Data;
using Shared.Models;

namespace ProjectService.Infrastructure.ExternalServices.BrokerService.Consumers;

public class AddReservationConsumer(    
    ITaskItemRepository taskItemRepository,
    IUnitOfWork unitOfWork,
    ILogger<AddReservationConsumer> logger,
    AppDbContext dbContext): IConsumer<ResourceReservationMessage>
{
    public async Task Consume(ConsumeContext<ResourceReservationMessage> context)
    {
        var resourceReservationMessage = context.Message;
        logger.LogInformation("Consuming AddReservation: TaskItemId={TaskItemId}, ResourceId={ResourceId}",
            resourceReservationMessage.TaskItemId, resourceReservationMessage.ResourceId);

        TaskItem? taskItem = await taskItemRepository.FindAsync(resourceReservationMessage.TaskItemId);
        if (taskItem is null)
        {
            throw new NotFoundException(nameof(TaskItem), resourceReservationMessage.TaskItemId);
        }
        taskItem.Resources.Add(resourceReservationMessage.ResourceId);
        using (dbContext.TemporarilySkipAudit())
        {
            var isSaved = await unitOfWork.SaveChangesAsync();
            if (isSaved <= 0)
            {
                throw new InternalServerException();
            }
        }
    }
}