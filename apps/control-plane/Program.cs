using Microsoft.AspNetCore.Http;

var builder = WebApplication.CreateBuilder(args);

// (Day 1: no DB/services wired yet)

var app = builder.Build();

// Health check
app.MapGet("/healthz", () =>
    Results.Json(new { status = "ok" })
);

// Flag lookup stub (returns 501 for now)
app.MapGet("/flags/{key}", (string key) =>
    Results.StatusCode(StatusCodes.Status501NotImplemented)
);

app.Run();

// Allow WebApplicationFactory in tests to find Program
public partial class Program { }
