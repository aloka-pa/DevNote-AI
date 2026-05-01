namespace DevNoteAI.Domain.Models;

public sealed record RewriteRequest(
    string Text,
    string Tone,
    string Context
);
