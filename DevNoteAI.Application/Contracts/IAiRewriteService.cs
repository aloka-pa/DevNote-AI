using DevNoteAI.Domain.Models;

namespace DevNoteAI.Application.Contracts;

public interface IAiRewriteService
{
    Task<string> RewriteAsync(RewriteRequest request, CancellationToken cancellationToken);
}
