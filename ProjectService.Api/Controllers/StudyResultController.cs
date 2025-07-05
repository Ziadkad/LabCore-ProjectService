using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProjectService.Api.Security;
using ProjectService.Application.Common.Exceptions;
using ProjectService.Application.StudyResult.Commands;

namespace ProjectService.Api.Controllers;

public class StudyResultController : BaseController
{
    [Authorize(Roles = $"{AuthPolicyName.Manager},{AuthPolicyName.Researcher}")]
    [HttpPost]
    public async Task<IActionResult> CreateStudyResult([FromBody]CreateStudyResultCommand command)
    {
        var result = await Mediator.Send(command);
        return Ok(result);
    }
    
    [Authorize(Roles = $"{AuthPolicyName.Manager},{AuthPolicyName.Researcher}")]
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateStudyResult(Guid id,[FromBody] UpdateStudyResultCommand command)
    {
        if (id != command.Id)
        {
            throw new BadRequestException("Ids do not match");
        }
        var result = await Mediator.Send(command);
        return Ok(result);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteStudyResult(Guid id)
    {
        await Mediator.Send(new DeleteStudyResultCommand(id));
        return Ok();
    }
}