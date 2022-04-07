var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSingleton<ITestProvider, TestProvider>();

var app = builder.Build();

app.MapGet("/api/user/{userId}", (string userId, [FromServices] ITestProvider testProvider) => testProvider.Get(userId));
app.Run();