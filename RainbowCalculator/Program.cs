var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddSingleton<ITestProvider, TestProvider>();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.MapGet("/api/user/{userId}", (string userId, [FromServices] ITestProvider testProvider) => testProvider.Get(userId));
app.Run();