// Purpose: Minimal, testable endpoint for reading a feature flag by key.
// Why it's good: (1) Explicit validation, (2) Cancellation-friendly, (3) No exceptions for control flow.
// Tradeoffs: In-memory cache stub today; swaps to Redis later behind IFlagStore.
// Edge cases handled: whitespace key, missing flag, canceled requests.
// Result shape: 200 { key, enabled, variants } | 404 | 400
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("flags")]
public class FlagsController : ControllerBase
{
    private readonly IFlagStore _store;
    public FlagsController(IFlagStore store) => _store = store;

    [HttpGet("{key}")]
    public async Task<IActionResult> Get(string key, CancellationToken ct)
    {
        if (string.IsNullOrWhiteSpace(key)) return BadRequest("key required");
        var flag = await _store.GetAsync(key.Trim(), ct);
        if (flag is null) return NotFound();
        return Ok(flag);
    }
}
