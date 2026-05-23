using Microsoft.AspNetCore.Mvc;
using task8.DTOs;
using task8.Services;

namespace task8.Controllers;

[ApiController]
[Route("api/submissions")]
public class SubmissionsController : ControllerBase
{
    private readonly SubmissionService _service;

    public SubmissionsController(SubmissionService service)
    {
        _service = service;
    }

    [HttpPost]
    public async Task<IActionResult> CreateSubmission([FromBody] CreateSubmissionDto dto)
    {
        var (result, status, error) = await _service.CreateAsync(dto);

        return status switch
        {
            400 => BadRequest(error),
            404 => NotFound(error),
            409 => Conflict(error),
            _ => CreatedAtAction(nameof(CreateSubmission), new { idSubmission = result!.SubmissionId }, result)
        };
    }

    [HttpPut("{idSubmission}/grade")]
    public async Task<IActionResult> GradeSubmission(int idSubmission, [FromBody] GradeSubmissionDto dto)
    {
        var (result, status, error) = await _service.GradeAsync(idSubmission, dto);

        return status switch
        {
            400 => BadRequest(error),
            404 => NotFound(error),
            _ => Ok(result)
        };
    }

    [HttpDelete("{idSubmission}")]
    public async Task<IActionResult> DeleteSubmission(int idSubmission)
    {
        var (status, error) = await _service.DeleteAsync(idSubmission);

        return status switch
        {
            400 => BadRequest(error),
            404 => NotFound(error),
            _ => NoContent()
        };
    }
}
