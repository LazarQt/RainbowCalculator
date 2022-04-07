var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSingleton<ITestProvider, TestProvider>();

var app = builder.Build();
app.MapGet("/api/user", ([FromServices] ITestProvider testProvider) => testProvider.Get());
app.Run();