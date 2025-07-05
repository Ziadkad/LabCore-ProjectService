using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProjectService.Api.Security;
using ProjectService.Application.Common.Exceptions;
using ProjectService.Application.Common.Models;
using ProjectService.Application.Study.Commands;
using ProjectService.Application.Study.Models;
using ProjectService.Application.Study.Queries;
using ProjectService.Domain.Task;

namespace ProjectService.Api.Controllers;

public class StudyController :BaseController
{
    [Authorize]
    [HttpGet("{id}")]
    public async Task<IActionResult> GetStudy(Guid id)
    {
        var result = await Mediator.Send(new GetStudyQuery(id));
        return Ok(result);
    }
    
    [Authorize(Roles = $"{AuthPolicyName.Manager},{AuthPolicyName.Researcher}")]
    [HttpPost]
    public async Task<IActionResult> CreateStudy([FromBody] CreateStudyCommand command)
    {
        var result = await Mediator.Send(command);
        return Ok(result);
    }
    
    [Authorize]
    [HttpGet("TaskItems/{id}")]
    public async Task<IActionResult> GetStudyWithTaskItems(Guid id, [FromQuery] string? keyword, [FromQuery] List<Guid>? assignedTo, [FromQuery] DateTime? startDate, [FromQuery] DateTime? endDate, [FromQuery] TaskItemStatus? taskItemStatus, [FromQuery] PageQueryRequest pageQuery)
    {
        var result = await Mediator.Send(new GetStudyWithTasksQuery(id, keyword, assignedTo, startDate, endDate, taskItemStatus, pageQuery));
        return Ok(result);
    }

    [Authorize(Roles = $"{AuthPolicyName.Manager},{AuthPolicyName.Researcher}")]
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateStudy(Guid id, [FromBody] UpdateStudyCommand command)
    {
        if (id != command.Id)
        {
            throw new BadRequestException("Ids do not match");
        }
        var result = await Mediator.Send(command);
        return Ok(result);
    }

    [Authorize(Roles = $"{AuthPolicyName.Manager},{AuthPolicyName.Researcher}")]
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteStudy(Guid id)
    {
        await Mediator.Send(new DeleteStudyCommand(id));
        return Ok();
    }

    [Authorize(Roles = $"{AuthPolicyName.Manager},{AuthPolicyName.Researcher}")]
    [HttpGet("StudyResult/{id}")]
    public async Task<IActionResult> GetStudyResult(Guid id)
    {
        var result = await Mediator.Send(new GetStudyWithStudyResult(id));
        return Ok(result);
    }
}