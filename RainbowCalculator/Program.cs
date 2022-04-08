using RestSharp;
using RainbowCalculator.Model;
using Newtonsoft.Json;

//var lands = await GetLands("https://api.scryfall.com/cards/search?q=t%3Aland+is%3Afirstprinting&unique=cards&as=grid&order=released&dir=asc");
//foreach (var land in lands)
//{
//    File.AppendAllText("lands.csv", $"{land.Name};{string.Join(string.Empty, land.ColorIdentity)};{string.Join(string.Empty, land.ProducedMana ?? new List<string>() { })};{land.Set}{Environment.NewLine}");
//}



var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSingleton<IManabaseProvider, ManabaseProvider>();

var app = builder.Build();

app.MapGet("/api/test", () => new { id = 1, name = "John Doe" });

app.MapGet("/api/manabase/{deckString}", (string deckString, [FromServices] IManabaseProvider manabaseProvider) => manabaseProvider.Retrieve(deckString));
app.Run();

//static async Task<List<ScryfallCard>> GetLands(string request, List<ScryfallCard> lands = null)
//{
//    if (lands == null) lands = new List<ScryfallCard>();

//    var response = await new RestClient().GetAsync(new RestRequest(request));
//    if (response.Content == null) throw new Exception("card search error");

//    var content = JsonConvert.DeserializeObject<CardsSearch>(response.Content);
//    if (content == null) throw new Exception("no content");
//    lands.AddRange(content.Data);

//    if (content.HasMore)
//    {
//        return await GetLands(content.NextPage, lands);
//    }
//    else
//    {
//        return lands;
//    }
//}