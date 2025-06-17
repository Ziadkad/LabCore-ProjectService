using MassTransit;
using Microsoft.Extensions.Logging;
using ProjectService.Application.Common.Exceptions;
using ProjectService.Application.Common.Interfaces;
using ProjectService.Application.Common.Models;
using ProjectService.Domain.Project;
using ProjectService.Domain.Study;
using ProjectService.Domain.StudyResult;
using ProjectService.Domain.Task;
using Shared.Models;

namespace ProjectService.Infrastructure.ExternalServices.BrokerService.Consumers;

public class DeleteFileConsumer(
    ILogger<DeleteFileConsumer> logger, 
    IProjectRepository projectRepository,
    IStudyRepository studyRepository,
    ITaskItemRepository taskItemRepository,
    IStudyResultRepository studyResultRepository,
    IUnitOfWork unitOfWork
) : IConsumer<FileMessage>
{
    public async Task Consume(ConsumeContext<FileMessage> context)
    {
        logger.LogInformation($"consumed the file {context.Message.ProjectId} + { context.Message.Path} ");
        FileMessage fileMessage = context.Message;
        if (fileMessage.Context == FileContext.Project)
        {
            Project? project = await projectRepository.FindAsync(fileMessage.ProjectId);
            if (project is null)
            {
                throw new NotFoundException("Project", fileMessage.ProjectId);
            }
            project.PathsFiles.Remove(fileMessage.Path);
        }

        if (fileMessage.Context == FileContext.Study)
        {
            Guid studyId = fileMessage.StudyId ?? throw new BadRequestException("StudyId is null");
            Study? study = await studyRepository.FindAsync(studyId);
            if (study is null)
            {
                throw new NotFoundException("Study", studyId);
            }
            if (study.ProjectId != fileMessage.ProjectId)
            {
                throw new BadRequestException("The study does not belong to project.");
            }
            study.PathsFiles.Remove(fileMessage.Path);
        }

        if (fileMessage.Context == FileContext.Task)
        {
            Guid taskId = fileMessage.TaskId ?? throw new BadRequestException("TaskId is null");
            TaskItem? taskItem = await taskItemRepository.FindAsync(taskId);
            if (taskItem is null)
            {
                throw new NotFoundException("Task", taskId);
            }
            if (taskItem.StudyId != fileMessage.StudyId)
            {
                throw new BadRequestException("The task does not belong to study.");
            }
            if (taskItem.Study.ProjectId != fileMessage.ProjectId)
            {
                throw new BadRequestException("The study does not belong to project.");
            }
            taskItem.PathsFiles.Remove(fileMessage.Path);
        }
        
        if (fileMessage.Context == FileContext.Result)
        {
            Guid studyResultId = fileMessage.TaskId ?? throw new BadRequestException("studyResultId is null");
            StudyResult? studyResult = await studyResultRepository.FindAsync(studyResultId);
            if (studyResult is null)
            {
                throw new NotFoundException("studyResult", studyResultId);
            }
            if (studyResult.StudyId != fileMessage.StudyId)
            {
                throw new BadRequestException("The studyResult does not belong to study.");
            }
            if (studyResult.Study.ProjectId != fileMessage.ProjectId)
            {
                throw new BadRequestException("The study does not belong to project.");
            }
            studyResult.PathsFiles.Remove(fileMessage.Path);
        }
        
        var isSaved = await unitOfWork.SaveChangesAsync();
        if (isSaved <= 0)
        {
            throw new InternalServerException();
        }
    }
}