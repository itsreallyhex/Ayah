---
description: Run an LLM council — send a question to multiple AI models simultaneously, have them critique each other's answers, then synthesize a final response. No web app needed. Use when the user wants multiple AI opinions, a council of models, or to compare LLM responses on any question.
argument-hint: <your question here>
---

## LLM Council — Native Claude Code Skill

Query multiple LLMs, let them judge each other, synthesize a final answer. All in the terminal via OpenRouter.

---

### Step 0 — Get the question

The user's question is: `$ARGUMENTS`

If `$ARGUMENTS` is empty, ask the user:
> "What question should I put to the council?"
Then stop and wait for their input.

---

### Step 1 — Check for API key

```bash
echo "OpenRouter key: ${OPENROUTER_API_KEY:0:8}..."
```

If `OPENROUTER_API_KEY` is not set, tell the user:
> "I need an OpenRouter API key. Set it with:
> `export OPENROUTER_API_KEY=your_key_here`
> Get one free at https://openrouter.ai"
Then stop.

---

### Step 2 — Define the council

Use these 4 models as council members (via OpenRouter model IDs):

| # | Name | Model ID |
|---|------|----------|
| 1 | GPT-4o | `openai/gpt-4o` |
| 2 | Gemini Flash | `google/gemini-flash-1.5` |
| 3 | Llama 3 70B | `meta-llama/llama-3-70b-instruct` |
| 4 | Mistral Large | `mistralai/mistral-large` |

Chairman (synthesizer): `openai/gpt-4o`

---

### Step 3 — Stage 1: First opinions

Query all 4 models in parallel. For each model, run:

```bash
curl -s https://openrouter.ai/api/v1/chat/completions \
  -H "Authorization: Bearer $OPENROUTER_API_KEY" \
  -H "Content-Type: application/json" \
  -d '{
    "model": "MODEL_ID_HERE",
    "messages": [{"role": "user", "content": "QUESTION_HERE"}],
    "max_tokens": 800
  }' | python3 -c "import sys,json; r=json.load(sys.stdin); print(r['choices'][0]['message']['content'])"
```

Replace `MODEL_ID_HERE` and `QUESTION_HERE` for each model. Run all 4 curl calls using Bash background jobs (`&`) then `wait` so they run in parallel.

Display results clearly:

```
╔══════════════════════════════════════╗
║  STAGE 1 — FIRST OPINIONS            ║
╚══════════════════════════════════════╝

🤖 GPT-4o:
[response]

🤖 Gemini Flash:
[response]

🤖 Llama 3 70B:
[response]

🤖 Mistral Large:
[response]
```

---

### Step 4 — Stage 2: Peer review

Now send all 4 responses back to each model and ask them to critique the others.

System prompt for each reviewer:
```
You are reviewing responses from other AI models to this question: "QUESTION"

Here are all responses (anonymized):
- Response A: [paste model A's answer]
- Response B: [paste model B's answer]
- Response C: [paste model C's answer]
- Response D: [paste model D's answer]

Your task: Rank these responses from best to worst and briefly explain why. 
Be critical. Identify factual errors, missing nuance, or weak reasoning.
Format: Rank 1st/2nd/3rd/4th with 1-2 sentence justification each.
```

Run all 4 review calls (each model reviews all 4 responses including its own, anonymized).

Display:

```
╔══════════════════════════════════════╗
║  STAGE 2 — PEER REVIEW               ║
╚══════════════════════════════════════╝

🔍 GPT-4o's rankings:
[review]

🔍 Gemini Flash's rankings:
[review]

🔍 Llama 3 70B's rankings:
[review]

🔍 Mistral Large's rankings:
[review]
```

---

### Step 5 — Stage 3: Chairman synthesis

Send everything to GPT-4o as Chairman with this prompt:

```
You are the Chairman of an AI council. The council was asked: "QUESTION"

Here are the 4 initial responses and each model's peer reviews:

[paste all stage 1 responses and stage 2 reviews]

Your job:
1. Identify where models agreed and disagreed
2. Note any factual errors flagged in peer review
3. Synthesize the BEST possible final answer that incorporates the strongest points
4. Note any important caveats or uncertainties

Be direct. Give the actual answer first, then the synthesis notes.
```

Display:

```
╔══════════════════════════════════════╗
║  STAGE 3 — CHAIRMAN'S VERDICT        ║
╚══════════════════════════════════════╝

⚖️  Final synthesized answer:

[chairman response]

━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━
Council complete. Models consulted: GPT-4o, Gemini Flash, Llama 3 70B, Mistral Large
```

---

### Error handling

- If any model call returns an error, show `[MODEL NAME: API error — skipping]` and continue with the remaining models
- If fewer than 2 models respond successfully in Stage 1, abort and tell the user to check their API key or OpenRouter balance
- If a response JSON has no `choices` key, print the raw response for debugging
- Timeout: if a curl call hangs over 30 seconds, kill it (`curl --max-time 30`)
