// Purpose: Retry with exponential backoff + full jitter for transient faults.
// Why it's good: (1) Jitter prevents synchronized retries, (2) Cancellation-aware, (3) Small, composable API.
// Usage: await Resilience.RetryAsync(() => client.SendAsync(req, ct), maxAttempts:5, baseDelay:TimeSpan.FromMilliseconds(100), ct);
public static class Resilience
{
    private static readonly Random _rng = new();

    public static async Task<T> RetryAsync<T>(
        Func<Task<T>> action,
        int maxAttempts,
        TimeSpan baseDelay,
        CancellationToken ct)
    {
        Exception? last = null;
        for (int attempt = 1; attempt <= maxAttempts; attempt++)
        {
            ct.ThrowIfCancellationRequested();
            try { return await action(); }
            catch (Exception ex) when (attempt < maxAttempts)
            {
                last = ex;
                var backoff = TimeSpan.FromMilliseconds(baseDelay.TotalMilliseconds * Math.Pow(2, attempt - 1));
                var jitter = TimeSpan.FromMilliseconds(_rng.Next(0, (int)backoff.TotalMilliseconds));
                await Task.Delay(jitter, ct);
            }
        }
        throw last ?? new InvalidOperationException("Retry failed without exception?");
    }
}
