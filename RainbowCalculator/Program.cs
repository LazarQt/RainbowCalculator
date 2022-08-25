var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSingleton<IManabaseProvider, ManabaseProvider>();

var app = builder.Build();

app.MapGet("/api/ping", () => new { id = 1, name = "Ping Successful" });

app.MapPost("/api/manabase", ([FromBody] Input jsonInput, [FromServices] IManabaseProvider manabaseProvider)
    => new { data = manabaseProvider.Retrieve(jsonInput.Decklist, jsonInput.Ignorelands, jsonInput.Ignoreaveragemv) });

app.Run();