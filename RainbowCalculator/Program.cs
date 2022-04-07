var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();
app.MapGet("/api/user", () => new { id = 1, name = "John Doe" });
app.Run();