// Purpose: Verifies canary evaluator triggers rollback when p95 exceeds threshold.
// Why it's good: (1) Captures domain rule, (2) Table-driven cases, (3) No mocks where unnecessary.
using Xunit;

public class CanaryEvaluatorTests
{
    [Theory]
    [InlineData(120, 100, true)]
    [InlineData(90, 100, false)]
    [InlineData(100, 100, true)] // boundary condition == triggers rollback
    public void TriggersRollback_OnP95Threshold(int p95Ms, int thresholdMs, bool expected)
    {
        var eval = new CanaryEvaluator(thresholdMs);
        var result = eval.ShouldRollback(p95Ms, errorRate: 0.0);
        Assert.Equal(expected, result);
    }
}
