namespace DevNoteAI.WebApi.Models;

public sealed record RewriteTextRequestDto(
    string Text,
    string Tone,
    string Context
);
