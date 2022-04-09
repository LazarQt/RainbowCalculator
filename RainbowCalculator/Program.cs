using RestSharp;
using RainbowCalculator.Model;
using Newtonsoft.Json;

//var lands = await GetLands("https://api.scryfall.com/cards/search?q=t%3Aland+is%3Afirstprinting&unique=cards&as=grid&order=released&dir=asc");
//foreach (var land in lands)
//{
//    File.AppendAllText("lands.csv", $"{land.Name};{string.Join(string.Empty, land.ColorIdentity)};{string.Join(string.Empty, land.ProducedMana ?? new List<string>() { })};{land.Set}{Environment.NewLine}");
//}

//var cards = await GetCards("https://api.scryfall.com/cards/search?q=+-is%3Adigital+f%3Aedh+++((fo%3Acost+fo%3A\"less+to+cast\")++or+++fo%3Aaffinity+or+fo%3Adelve+or+m>%3Dx++or++(o%3Aconvoke+fo%3A\"can+help+cast+this+spell\")+++)++-(+fo%3A\"that+target+~\"+fo%3Acost+o%3Aless)+++-(o%3A\"this+spell+costs+{1}+less+to+cast+if\"+or+o%3A\"this+spell+costs+{2}+less+to+cast+if\")+++-o%3A\"spells+you+cast+cost+{1}+less+to+cast\"+++-o%3A\"spell+you+cast+costs+{1}+less+to+cast\"+++-(o%3A\"spells+you+cast+cost\"+o%3A\"less+to+cast\")+++-(o%3A\"spells+with+the+chosen+name+cost\"+o%3A\"less+to+cast\")++-(o%3A\"spells+with+the+chosen+name+you+cast+cost\"+o%3A\"less+to+cast\")++-(o%3A\"spells+you+cast+of+the+chosen+type+cost\"+o%3A\"less+to+cast\")++-(o%3A\"spells+with\"+o%3A\"you+cast+cost\"+o%3A\"less+to+cast\")");
//foreach (var c in cards)
//{
//    File.AppendAllText("exclude.txt", $"{c.Name}{Environment.NewLine}");
//}



var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSingleton<IManabaseProvider, ManabaseProvider>();

var app = builder.Build();

app.MapGet("/api/test", () => new { id = 1, name = "John Doe" });

app.MapGet("/api/manabase/{deckString}", (string deckString, [FromServices] IManabaseProvider manabaseProvider) => manabaseProvider.Retrieve(deckString));
app.Run();

static async Task<List<ScryfallCard>> GetCards(string request, List<ScryfallCard> lands = null)
{
    if (lands == null) lands = new List<ScryfallCard>();

    var response = await new RestClient().GetAsync(new RestRequest(request));
    if (response.Content == null) throw new Exception("card search error");

    var content = JsonConvert.DeserializeObject<CardsSearch>(response.Content);
    if (content == null) throw new Exception("no content");
    lands.AddRange(content.Data);

    if (content.HasMore)
    {
        return await GetCards(content.NextPage, lands);
    }
    else
    {
        return lands;
    }
}