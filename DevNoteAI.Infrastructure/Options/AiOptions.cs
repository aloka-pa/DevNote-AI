namespace DevNoteAI.Infrastructure.Options;

public sealed class AiOptions
{
    public const string SectionName = "Ai";

    public string Provider { get; set; } = "OpenAI-Compatible";

    public string ApiKey { get; set; } = string.Empty;

    public string Model { get; set; } = "gpt-4o-mini";

    public string Endpoint { get; set; } = "https://api.openai.com/v1/chat/completions";
}
