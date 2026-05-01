using DevNoteAI.Application.Contracts;
using DevNoteAI.Domain.Models;

namespace DevNoteAI.Application.Rewrite;

public sealed class RewriteTextUseCase(IAiRewriteService aiRewriteService) : IRewriteTextUseCase
{
    public async Task<RewriteResult> ExecuteAsync(RewriteRequest request, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(request.Text))
        {
            throw new ArgumentException("Text is required.", nameof(request.Text));
        }

        var rewrittenText = await aiRewriteService.RewriteAsync(request, cancellationToken);
        return new RewriteResult(rewrittenText);
    }
}
