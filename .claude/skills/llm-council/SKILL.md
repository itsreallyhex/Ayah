---
description: Set up and launch the llm-council web app by Andrej Karpathy — queries multiple LLMs simultaneously and has them evaluate each other's responses. Use when the user wants to run llm-council, start the council app, or compare multiple AI model responses.
argument-hint: [openrouter-api-key]
---

## LLM Council Setup & Launch

Your job is to set up and run https://github.com/karpathy/llm-council locally.

### Step 1 — Check dependencies

Run these checks and tell the user what's missing before proceeding:

```bash
which uv || echo "MISSING: uv (Python package manager)"
which node || echo "MISSING: node"
which npm || echo "MISSING: npm"
which git || echo "MISSING: git"
```

If anything is missing, tell the user and stop. Don't proceed until they install it.

Install uv if missing:
```bash
curl -LsSf https://astral.sh/uv/install.sh | sh
```

### Step 2 — Clone the repo (skip if already exists)

```bash
if [ ! -d "$HOME/llm-council" ]; then
  git clone https://github.com/karpathy/llm-council.git "$HOME/llm-council"
  echo "Cloned successfully."
else
  echo "Already cloned at $HOME/llm-council — pulling latest..."
  git -C "$HOME/llm-council" pull
fi
```

### Step 3 — Set up the API key

Check if a `.env` file already exists with an API key:

```bash
cat "$HOME/llm-council/.env" 2>/dev/null || echo "No .env found"
```

If `$ARGUMENTS` was provided, use it as the OpenRouter API key:
```bash
echo "OPENROUTER_API_KEY=$ARGUMENTS" > "$HOME/llm-council/.env"
echo ".env written."
```

If no key was provided and no `.env` exists, ask the user:
> "I need your OpenRouter API key to continue. Get one free at https://openrouter.ai — then run `/llm-council YOUR_KEY_HERE`"
> Stop here and wait.

### Step 4 — Install backend dependencies

```bash
cd "$HOME/llm-council" && uv sync
```

### Step 5 — Install frontend dependencies

```bash
cd "$HOME/llm-council" && npm install
```

### Step 6 — Launch

Tell the user you're about to start both servers, then run:

```bash
cd "$HOME/llm-council" && uv run python -m backend.main &
sleep 2
cd "$HOME/llm-council" && npm run dev &
```

Then tell the user:
> "LLM Council is running!
> - Frontend: http://localhost:5173
> - Backend: http://localhost:8000
>
> Open http://localhost:5173 in your browser.
> To stop it, run: `pkill -f 'backend.main'; pkill -f 'npm run dev'`"

### Error handling

- If `uv sync` fails: check Python version with `python3 --version` — needs 3.9+
- If `npm install` fails: check node version with `node --version` — needs 18+
- If port 5173 or 8000 is already in use: run `lsof -i :5173` and `lsof -i :8000` to find what's using them
- If the backend crashes on start: check the `.env` key is valid — test with `curl https://openrouter.ai/api/v1/models -H "Authorization: Bearer $(grep OPENROUTER_API_KEY $HOME/llm-council/.env | cut -d= -f2)"`
