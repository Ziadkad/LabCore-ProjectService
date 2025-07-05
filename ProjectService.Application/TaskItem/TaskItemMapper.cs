using AutoMapper;
using ProjectService.Application.TaskItem.Models;

namespace ProjectService.Application.TaskItem;

public class TaskItemMapper : Profile
{
    public TaskItemMapper()
    {
        CreateMap<Domain.Task.TaskItem, TaskItemDto>();
    }
}