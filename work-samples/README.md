# Work Samples — Jonathan Smith

**How to read this pack (3 min):**
- Three short excerpts showing API design, reliability, and correctness.
- Each sample has a brief “why it’s good / tradeoffs / edges”.
- Links back to full context in the repo.

## 1) Endpoint: GET /flags/{key}
- **Shows:** validation, cancellation, clean result modeling.
- **Tradeoffs:** in-memory cache stub today; Redis later via `IFlagStore`.
- **Edges:** whitespace keys, missing flag.
- **Link:** https://github.com/Landers99/canary-platform/apps/control-plane/Controllers/FlagsController.cs
- **Code:** `sample-1.cs`

## 2) Resilience: Exponential backoff with jitter
- **Shows:** bounded retries, jitter to prevent herds, cancellation.
- **Edges:** zero attempts guard, overflow-safe backoff cap (future).
- **Link:** https://github.com/Landers99/canary-platform/sdks/dotnet/Resilience.cs
- **Code:** `sample-2.cs`

## 3) Unit test: Canary rollback rule
- **Shows:** domain-focused test, boundary assertion.
- **Edges:** p95 ties trigger rollback; error-rate rules in separate tests.
- **Link:** https://github.com/Landers99/canary-platform/apps/control-plane.tests/CanaryEvaluatorTests.cs
- **Code:** `sample-3.cs`

---

### Provenance & Safety
- Code authored by me on 08-11-2025. No secrets, no proprietary content.
- AI used for doc polish only; logic hand-written and tested.

### What to see next (links)
- Flagship repo (readme + demo): https://github.com/Landers99/canary-platform
- One-pager: <ONE-PAGER-LINK>
