# DevNote AI

A personal project built to practise **.NET 10** and **Clean Architecture** principles.

## Purpose

This is not a production app. The goal is to apply Clean Architecture patterns correctly in a real, working project — not just a tutorial clone.

## Architecture Highlights

- **Dependency Rule strictly enforced** — inner layers have zero knowledge of outer layers
- **Application layer is not web-aware** — no ASP.NET types, no HTTP concerns, no controller dependencies
- **Use cases are framework-agnostic** — `RewriteTextUseCase` depends only on interfaces, not implementations
- **Infrastructure is pluggable** — swap Groq, OpenAI, or Ollama without touching Application or Domain
- **Strongly typed DTOs** — request/response contracts defined at the boundary, not leaked inward
- **No business logic in controllers** — controllers only delegate and return

## Project Structure

| Layer | Project | Responsibility |
|---|---|---|
| Domain | `DevNoteAI.Domain` | Core models (`RewriteRequest`, `RewriteResult`) |
| Application | `DevNoteAI.Application` | Use cases, service contracts (`IAiRewriteService`) |
| Infrastructure | `DevNoteAI.Infrastructure` | AI provider integration (`OpenAiRewriteService`) |
| Presentation | `DevNoteAI.WebApi` | HTTP controllers, DI composition, entry point |

## What It Does

A browser extension (Chrome/Edge) sends rough developer text to the API, which rewrites it into clean professional output using an AI model.

> ⚠️ **Uses Ollama (local AI) by default** — not publicly hosted by design.  
> To run it yourself, swap Ollama for [Groq](https://console.groq.com) (free) — see configuration below.

## API Contract

### POST `/api/rewrite`

```json
{
  "text": "pls check this bug quick",
  "tone": "Bug Report",
  "context": "QA Testing"
}
```

```json
{
  "rewrittenText": "Please review this bug as soon as possible."
}
```

## Configuration

### Ollama (default — local only)

```bash
ollama pull llama3.2
```

```json
"Ai": {
  "Provider": "Ollama",
  "ApiKey": "ollama",
  "Model": "llama3.2",
  "Endpoint": "http://localhost:11434/v1/chat/completions"
}
```

### Groq (if you want to run without Ollama)

Sign up at [console.groq.com](https://console.groq.com) for a free key, then:

```json
"Ai": {
  "Provider": "Groq",
  "ApiKey": "YOUR_GROQ_API_KEY",
  "Model": "llama-3.3-70b-versatile",
  "Endpoint": "https://api.groq.com/openai/v1/chat/completions"
}
```

> Use .NET User Secrets to avoid committing keys:
> ```bash
> cd DevNoteAI.WebApi
> dotnet user-secrets init
> dotnet user-secrets set "Ai:ApiKey" "your-key-here"
> ```

## Run Backend

```bash
dotnet restore DevNoteAI.slnx
dotnet build DevNoteAI.slnx
dotnet run --project DevNoteAI.WebApi --launch-profile https
```

## Load Browser Extension

1. Open Chrome or Edge → `chrome://extensions`
2. Enable **Developer mode**
3. Click **Load unpacked** → select the `browser-extension` folder

> If your backend runs on a different URL, update the endpoint in `browser-extension/popup.js`.
