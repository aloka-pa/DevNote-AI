using DevNoteAI.Domain.Models;

namespace DevNoteAI.Application.Rewrite;

public interface IRewriteTextUseCase
{
    Task<RewriteResult> ExecuteAsync(RewriteRequest request, CancellationToken cancellationToken);
}
