using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Testing;
using Xunit;

namespace ControlPlane.Tests;

public class HealthzTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly WebApplicationFactory<Program> _factory;

    public HealthzTests(WebApplicationFactory<Program> factory) => _factory = factory;

    [Fact]
    public async Task Healthz_ReturnsOkWithExpectedJson()
    {
        var client = _factory.CreateClient();
        var res = await client.GetAsync("/healthz");
        res.EnsureSuccessStatusCode();

        var json = await res.Content.ReadAsStringAsync();
        using var doc = JsonDocument.Parse(json);
        Assert.True(doc.RootElement.TryGetProperty("status", out var status));
        Assert.Equal("ok", status.GetString());
    }
}