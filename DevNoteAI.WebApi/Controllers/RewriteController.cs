using DevNoteAI.Application.Rewrite;
using DevNoteAI.Domain.Models;
using DevNoteAI.WebApi.Models;
using Microsoft.AspNetCore.Mvc;

namespace DevNoteAI.WebApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public sealed class RewriteController(IRewriteTextUseCase rewriteTextUseCase) : ControllerBase
{
    [HttpPost]
    [ProducesResponseType(typeof(RewriteTextResponseDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<RewriteTextResponseDto>> RewriteAsync(
        [FromBody] RewriteTextRequestDto request,
        CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(request.Text))
        {
            return BadRequest(new { message = "Text is required." });
        }

        var useCaseRequest = new RewriteRequest(request.Text, request.Tone, request.Context);
        var result = await rewriteTextUseCase.ExecuteAsync(useCaseRequest, cancellationToken);

        return Ok(new RewriteTextResponseDto(result.RewrittenText));
    }
}
