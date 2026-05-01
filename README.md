# DevNote AI

DevNote AI is a personal project built to practise **.NET 10** and **Clean Architecture** patterns.

It consists of:
- A browser extension (Chrome/Edge) for rewriting rough developer text.
- A `.NET 10` backend API built with Clean Architecture.

> ⚠️ **This project uses Ollama (local AI) by default.** It is intentionally not publicly hosted.  
> To run it yourself, you'll need to swap Ollama for a cloud provider like [Groq](https://console.groq.com) (free) — see configuration below.

## Features

- Enter rough text with grammar mistakes.
- Choose tone and context from dropdowns.
- Click **Rewrite** to get corrected, professional output.
- Copy final output with one click.

## Tone Options

- Professional
- Technical
- Concise
- Friendly
- Bug Report
- RCA
- PR Description
- Daily Scrum Update

## Context Options

- General
- QA Testing
- Software Development
- HR Software
- Client Communication
- Azure DevOps
- Release Notes

## Backend Architecture

Projects:
- `DevNoteAI.Domain` — Core models (`RewriteRequest`, `RewriteResult`).
- `DevNoteAI.Application` — Use case/handler (`IRewriteTextUseCase`, `RewriteTextUseCase`) and service contract (`IAiRewriteService`).
- `DevNoteAI.Infrastructure` — AI integration service (`OpenAiRewriteService`) and AI options binding.
- `DevNoteAI.WebApi` — Exposes `POST /api/rewrite` and composes dependencies via DI.

Rules enforced:
- No business logic in controllers.
- Application does not depend on Infrastructure or WebApi.
- Strongly typed request/response DTOs.
- Validation for empty input text.

## API Contract

### POST `/api/rewrite`

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

Make sure [Ollama](https://ollama.com) is installed and running, then pull a model:

```bash
ollama pull llama3.2
```

Set in `appsettings.Development.json` or via user secrets:

```json
"Ai": {
  "Provider": "Ollama",
  "ApiKey": "ollama",
  "Model": "llama3.2",
  "Endpoint": "http://localhost:11434/v1/chat/completions"
}
```

### Alternative (Groq — if you want to share or run without Ollama)

Sign up for a free API key at [console.groq.com](https://console.groq.com), then set:

```json
"Ai": {
  "Provider": "Groq",
  "ApiKey": "YOUR_GROQ_API_KEY",
  "Model": "llama-3.3-70b-versatile",
  "Endpoint": "https://api.groq.com/openai/v1/chat/completions"
}
```

> Use .NET User Secrets to avoid committing API keys:
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

API runs on `https://localhost:7251` by default.

## Load Browser Extension

1. Open Chrome or Edge.
2. Go to `chrome://extensions` or `edge://extensions`.
3. Enable **Developer mode**.
4. Click **Load unpacked**.
5. Select the `browser-extension` folder.

> If your backend runs on a different URL, update the endpoint in `browser-extension/popup.js`.

## Usage

1. Start the backend API.
2. Open the extension popup.
3. Enter rough text.
4. Select tone and context.
5. Click **Rewrite**.
6. Click **Copy** to copy the output.