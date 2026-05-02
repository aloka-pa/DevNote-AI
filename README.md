# DevNote AI

A **.NET 10** learning project focused on applying **Clean Architecture** correctly in a small, end-to-end app: a browser extension talks to an API that rewrites rough developer text via an AI provider.

## Clean Architecture: principles in this repo

| Principle | How it shows up here |
|---|---|
| **Inner layers know nothing about outer layers** | `Domain` knows nothing. `Application` knows only `Domain`. `Infrastructure` and `WebApi` know `Application`. Never the other way. |
| **Application is not web-aware** | `RewriteTextUseCase` and `IAiRewriteService` are plain C# — no ASP.NET, no `HttpContext`. The application layer has no idea it's running inside a web API. |
| **Ports and Adapters** | `IAiRewriteService` is the port — defined in `Application`, not Infrastructure. `OpenAiRewriteService` is the adapter. Application dictates the contract, Infrastructure fulfils it. |
| **Infrastructure is swappable** | Swap Ollama → Groq → OpenAI by changing config and DI registration. `Application` and `Domain` are never touched. |
| **No business logic in controllers** | `RewriteController` only maps HTTP input and calls the use case. No decisions, no rules, no AI calls at the controller level. |
| **Composition root** | All wiring happens in one place — `DevNoteAI.WebApi`. `AddApplication()` and `AddInfrastructure()` register everything. Inner projects never reference `WebApi`. |

### Dependency direction (project references)

```text
DevNoteAI.WebApi  →  DevNoteAI.Application
DevNoteAI.WebApi  →  DevNoteAI.Infrastructure
DevNoteAI.Infrastructure  →  DevNoteAI.Application
DevNoteAI.Application  →  DevNoteAI.Domain
DevNoteAI.Domain  →  (none)
```

## Project structure

| Layer | Project | Responsibility |
|---|---|---|
| Domain | `DevNoteAI.Domain` | Core models |
| Application | `DevNoteAI.Application` | Use cases (`IRewriteTextUseCase`), port (`IAiRewriteService`) |
| Infrastructure | `DevNoteAI.Infrastructure` | AI adapter (`OpenAiRewriteService`), options binding |
| Presentation | `DevNoteAI.WebApi` | Controllers, Swagger/OpenAPI, DI registration |

## What it does (product)

- **Browser extension** (Chrome/Edge): rough text, tone, and context → **Rewrite** → copy polished text.
- **API**: `POST /api/rewrite` delegates to the use case, which calls the configured AI.

> ⚠️ **Ollama (local AI) is the default** — not publicly hosted by design. For a hosted or key-based provider, use **Groq** (or similar) — see [Configuration](#configuration).

### Tone options

Professional, Technical, Concise, Friendly, Bug Report, RCA, PR Description, Daily Scrum Update.

### Context options

General, QA Testing, Software Development, HR Software, Client Communication, Azure DevOps, Release Notes.

## API contract

### `POST /api/rewrite`

Request:

```json
{
  "text": "pls check this bug quick",
  "tone": "Bug Report",
  "context": "QA Testing"
}
```

Response:

```json
{
  "rewrittenText": "Please review this bug as soon as possible."
}
```

## Configuration

### Default (Ollama — local only)

Install and run [Ollama](https://ollama.com), then:

```bash
ollama pull mistral
```

Configure in `appsettings.json`, `appsettings.Development.json`, or user secrets:

```json
"Ai": {
  "Provider": "Ollama",
  "ApiKey": "ollama",
  "Model": "mistral",
  "Endpoint": "http://localhost:11434/v1/chat/completions"
}
```

### Groq (alternative — no local Ollama)

Free tier API key: [console.groq.com](https://console.groq.com)

```json
"Ai": {
  "Provider": "Groq",
  "ApiKey": "YOUR_GROQ_API_KEY",
  "Model": "llama-3.3-70b-versatile",
  "Endpoint": "https://api.groq.com/openai/v1/chat/completions"
}
```

> Use .NET User Secrets so keys are not committed:
> ```bash
> cd DevNoteAI.WebApi
> dotnet user-secrets init
> dotnet user-secrets set "Ai:ApiKey" "your-key-here"
> ```

## Run backend

```bash
dotnet restore DevNoteAI.slnx
dotnet build DevNoteAI.slnx
dotnet run --project DevNoteAI.WebApi --launch-profile https
```

Default URL: `https://localhost:7251`

## Load browser extension

1. Chrome or Edge → `chrome://extensions` or `edge://extensions`
2. Enable **Developer mode**
3. **Load unpacked** → select the `browser-extension` folder

If the API URL differs, edit `browser-extension/popup.js` (`apiBaseUrl`).

## Usage

1. Start the API.
2. Open the extension popup, enter text, choose tone and context.
3. **Rewrite**, then **Copy** if you want the result on the clipboard.
