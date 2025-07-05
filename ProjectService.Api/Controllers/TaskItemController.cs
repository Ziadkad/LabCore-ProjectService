using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProjectService.Api.Security;
using ProjectService.Application.Common.Exceptions;
using ProjectService.Application.Common.Models;
using ProjectService.Application.TaskItem.Commands;
using ProjectService.Application.TaskItem.Queries;
using ProjectService.Domain.Task;

namespace ProjectService.Api.Controllers;

public class TaskItemController : BaseController
{
    
    [Authorize(Roles = $"{AuthPolicyName.Manager},{AuthPolicyName.Researcher}")]
    [HttpGet]
    public async Task<IActionResult> GetTaskItems( [FromQuery] string? keyword, [FromQuery] List<Guid>? assignedTo, [FromQuery] DateTime? startDate, [FromQuery] DateTime? endDate, [FromQuery] TaskItemStatus? taskItemStatus, [FromQuery] PageQueryRequest pageQuery)
    {
        var result = await Mediator.Send(new GetAllTaskItemsQuery(keyword, assignedTo, startDate, endDate, taskItemStatus, pageQuery));
        return Ok(result);
    }
    
    
    [Authorize]
    [HttpGet("{id}")]
    public async Task<IActionResult> GetTaskItem(Guid id)
    {
        var result = await Mediator.Send(new GetTaskItemQuery(id));
        return Ok(result);
    }
    
    [Authorize(Roles = $"{AuthPolicyName.Manager},{AuthPolicyName.Researcher}")]
    [HttpPost]
    public async Task<IActionResult> CreateTaskItem(CreateTaskItemCommand command)
    {
        var result = await Mediator.Send(command);
        return Ok(result);
    }
    
    [Authorize(Roles = $"{AuthPolicyName.Manager},{AuthPolicyName.Researcher}")]
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteTaskItem(Guid id)
    {
        await Mediator.Send(new DeleteTaskItemCommand(id));
        return NoContent();
    }

    [Authorize(Roles = $"{AuthPolicyName.Manager},{AuthPolicyName.Researcher}")]
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateTaskItem(Guid id, [FromBody] UpdateTaskItemCommand command)
    {
        if (id != command.Id)
        {
            throw new BadRequestException("Ids do not match");
        }
        var result = await Mediator.Send(command);
        return Ok(result);
    }
    
    [Authorize(Roles = $"{AuthPolicyName.Manager},{AuthPolicyName.Researcher}")]
    [HttpPut("status/{id}")]
    public async Task<IActionResult> UpdateStatus(Guid id, [FromBody] ChangeTaskItemStatusCommand command)
    {
        if  (id != command.Id)
        {
            throw new BadRequestException("Ids do not match");
        }
        var result = await Mediator.Send(command);
        return Ok(result);
    }
    
}