var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSingleton<IManabaseProvider, ManabaseProvider>();

var app = builder.Build();

app.MapGet("/api/ping", () => new { id = 1, name = "Ping Successful" });

app.MapGet("/api/manabase/{deckString}", (string deckString, [FromServices] IManabaseProvider manabaseProvider) => manabaseProvider.Retrieve(deckString));
app.Run();