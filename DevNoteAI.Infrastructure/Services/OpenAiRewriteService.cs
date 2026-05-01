using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using DevNoteAI.Application.Contracts;
using DevNoteAI.Domain.Models;
using DevNoteAI.Infrastructure.Options;
using Microsoft.Extensions.Options;

namespace DevNoteAI.Infrastructure.Services;

public sealed class OpenAiRewriteService(
    HttpClient httpClient,
    IOptions<AiOptions> options) : IAiRewriteService
{
    private static readonly JsonSerializerOptions JsonOptions = new(JsonSerializerDefaults.Web);
    private readonly AiOptions _options = options.Value;

    public async Task<string> RewriteAsync(RewriteRequest request, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(_options.ApiKey))
        {
            throw new InvalidOperationException("AI API key is not configured.");
        }

        var prompt = BuildPrompt(request);
        var payload = new
        {
            model = _options.Model,
            temperature = 0.2,
            messages = new object[]
            {
                new
                {
                    role = "system",
                    content = "You are a writing assistant. Correct grammar and improve clarity while preserving meaning. Do not add facts. Return only the rewritten text."
                },
                new
                {
                    role = "user",
                    content = prompt
                }
            }
        };

        using var requestMessage = new HttpRequestMessage(HttpMethod.Post, _options.Endpoint)
        {
            Content = new StringContent(JsonSerializer.Serialize(payload, JsonOptions), Encoding.UTF8, "application/json")
        };
        requestMessage.Headers.Authorization = new AuthenticationHeaderValue("Bearer", _options.ApiKey);

        using var response = await httpClient.SendAsync(requestMessage, cancellationToken);
        response.EnsureSuccessStatusCode();

        var rawJson = await response.Content.ReadAsStringAsync(cancellationToken);
        var parsed = JsonSerializer.Deserialize<ChatCompletionResponse>(rawJson, JsonOptions);
        var rewritten = parsed?.Choices?.FirstOrDefault()?.Message?.Content?.Trim();

        if (string.IsNullOrWhiteSpace(rewritten))
        {
            throw new InvalidOperationException("AI provider returned an empty rewrite response.");
        }

        return rewritten;
    }

    private static string BuildPrompt(RewriteRequest request) =>
        $$"""
        Rewrite the following text.
        Tone: {{request.Tone}}
        Context/Industry: {{request.Context}}
        
        Rules:
        - Correct grammar and improve clarity.
        - Keep original meaning.
        - Do not add new facts.
        - Return only the rewritten sentence or paragraph.
        
        Text:
        {{request.Text}}
        """;

    private sealed record ChatCompletionResponse(List<Choice>? Choices);

    private sealed record Choice(Message? Message);

    private sealed record Message(string? Content);
}
