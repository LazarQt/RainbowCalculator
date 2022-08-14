var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSingleton<IManabaseProvider, ManabaseProvider>();

var app = builder.Build();

app.MapGet("/api/ping", () => new { id = 1, name = "Ping Successful" });

app.MapPost("/api/manabase", ([FromBody] string jsonstring, [FromServices] IManabaseProvider manabaseProvider) => new { id = 1, name = manabaseProvider.Retrieve(jsonstring, "") } );

app.Run();