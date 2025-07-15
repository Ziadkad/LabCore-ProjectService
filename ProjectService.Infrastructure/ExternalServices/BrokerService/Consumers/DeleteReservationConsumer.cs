using MassTransit;
using ProjectService.Application.Common.Exceptions;
using ProjectService.Application.Common.Interfaces;
using ProjectService.Domain.Task;
using ProjectService.Infrastructure.Data;
using Shared.Models;

namespace ProjectService.Infrastructure.ExternalServices.BrokerService.Consumers;

public class DeleteReservationConsumer(    
    ITaskItemRepository taskItemRepository,
    IUnitOfWork unitOfWork,
    AppDbContext dbContext): IConsumer<ResourceReservationMessage>
{
    public async Task Consume(ConsumeContext<ResourceReservationMessage> context)
    {
        var resourceReservationMessage = context.Message;
        TaskItem? taskItem = await taskItemRepository.FindAsync(resourceReservationMessage.TaskItemId);
        if (taskItem is null)
        {
            throw new NotFoundException(nameof(TaskItem), resourceReservationMessage.TaskItemId);
        }
        taskItem.Resources.Remove(resourceReservationMessage.ResourceId);
        using (dbContext.TemporarilySkipAudit())
        {
            var isSaved = await unitOfWork.SaveChangesAsync();
            if (isSaved <= 0)
            {
                throw new InternalServerException();
            }
        }    }
}