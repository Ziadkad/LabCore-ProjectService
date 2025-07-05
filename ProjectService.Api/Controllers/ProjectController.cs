using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProjectService.Api.Security;
using ProjectService.Application.Common.Exceptions;
using ProjectService.Application.Common.Models;
using ProjectService.Application.Project.Commands;
using ProjectService.Application.Project.Queries;
using ProjectService.Domain.Project;

namespace ProjectService.Api.Controllers;

public class ProjectController : BaseController
{
    [Authorize]
    [HttpGet("{id}")]
    public async Task<IActionResult> GetProject([FromRoute] Guid id)
    {
        var result = await Mediator.Send(new GetProjectQuery(id));
        return Ok(result);
    }
    
    [Authorize]
    [HttpGet("Studies/{id}")]
    public async Task<IActionResult> GetProjectWithStudies([FromRoute] Guid id)
    {
        var result = await Mediator.Send(new GetProjectWithStudiesQuery(id));
        return Ok(result);
    }

    [Authorize]
    [HttpGet]
    public async Task<IActionResult> GetAllProject(
        [FromQuery] string? keyword,
        [FromQuery] DateTime? startDate,
        [FromQuery] DateTime? endDate,
        [FromQuery] ProjectStatus? status,
        [FromQuery] float? progressPercentageMin,
        [FromQuery] float? progressPercentageMax,
        [FromQuery] List<string>? tags,
        [FromQuery] List<Guid>? researchers,
        [FromQuery] Guid? manager,
        [FromQuery] PageQueryRequest pageQuery)
    {
        var query = new GetAllProjectQuery(
            keyword,
            startDate,
            endDate,
            status,
            progressPercentageMin,
            progressPercentageMax,
            tags,
            researchers,
            manager,
            pageQuery
        );

        var result = await Mediator.Send(query);
        return Ok(result);
    }
    
    [Authorize(AuthPolicyName.Manager)]
    [HttpPost]
    public async Task<IActionResult> CreateProject([FromBody] CreateProjectCommand command)
    {
        var result = await Mediator.Send(command);
        return Ok(result);
    }
    
    
    [Authorize(AuthPolicyName.Manager)]
    [HttpPut("AddResearchers/{projectId}")]
    public async Task<IActionResult> AddResearchers(Guid projectId, [FromBody] List<Guid> researcherIds)
    {
        var result = await Mediator.Send(new AddResearchersCommand(projectId, researcherIds));
        return Ok(result);
    }

    [Authorize(AuthPolicyName.Manager)]
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateProject(Guid id, [FromBody] UpdateProjectCommand command)
    {
        if (id != command.Id)
        {
            throw new BadRequestException("Ids do not match");
        }
        var result = await Mediator.Send(command);
        return Ok(result);
    }
    
    
    [Authorize(AuthPolicyName.Manager)]
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteProject(Guid id)
    {
        await Mediator.Send(new DeleteProjectCommand(id));
        return NoContent();
    }
    
    
}