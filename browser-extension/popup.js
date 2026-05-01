const apiBaseUrl = "https://localhost:7251";

const inputText = document.getElementById("inputText");
const toneSelect = document.getElementById("toneSelect");
const contextSelect = document.getElementById("contextSelect");
const rewriteButton = document.getElementById("rewriteButton");
const outputText = document.getElementById("outputText");
const copyButton = document.getElementById("copyButton");
const statusText = document.getElementById("statusText");

rewriteButton.addEventListener("click", async () => {
  const text = inputText.value.trim();
  if (!text) {
    setStatus("Please enter text to rewrite.");
    return;
  }

  rewriteButton.disabled = true;
  copyButton.disabled = true;
  setStatus("Rewriting...");

  try {
    const response = await fetch(`${apiBaseUrl}/api/rewrite`, {
      method: "POST",
      headers: {
        "Content-Type": "application/json"
      },
      body: JSON.stringify({
        text,
        tone: toneSelect.value,
        context: contextSelect.value
      })
    });

    if (!response.ok) {
      throw new Error(`Rewrite failed with status ${response.status}.`);
    }

    const data = await response.json();
    outputText.value = data.rewrittenText ?? "";
    copyButton.disabled = !outputText.value.trim();
    setStatus("Rewrite complete.");
  } catch (error) {
    setStatus(error.message || "Rewrite failed.");
  } finally {
    rewriteButton.disabled = false;
  }
});

copyButton.addEventListener("click", async () => {
  const value = outputText.value.trim();
  if (!value) {
    return;
  }

  await navigator.clipboard.writeText(value);
  setStatus("Copied to clipboard.");
});

function setStatus(message) {
  statusText.textContent = message;
}
