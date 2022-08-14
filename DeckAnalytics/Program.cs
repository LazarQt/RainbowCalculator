// See https://aka.ms/new-console-template for more information

using RainbowCore;

Console.WriteLine("Hello, World!");

var p = new CardsParser();

foreach (string file in Directory.EnumerateFiles("Files", "*.txt"))
{
    var lines = File.ReadAllLines(file);
    if (lines.Length != 100) continue;
    lines = lines.Select(l => l.Remove(0, 1)).ToArray();
    var cards = p.GetCards(lines, out var missing);
    var cardsWithoutLands = cards.Where(c => !c.TypeLine.ToLower().Contains("land")).ToList();
    var manarocks = cardsWithoutLands.Where(c => c.TypeLine.ToLower().Contains("artifact") && c.ProducedMana != null).ToList();

    var cardsWithoutLandsWithoutManarocks = cardsWithoutLands.Except(manarocks).ToList();

    var mv = cardsWithoutLandsWithoutManarocks.Average(x => x.Cmc);

    Console.WriteLine($"MV: {mv}");
    Console.WriteLine($"Cards: {cardsWithoutLandsWithoutManarocks.Count}");
    Console.WriteLine($"Rocks: {manarocks.Count}");
    Console.WriteLine($"Lands: {cards.Count - cardsWithoutLands.Count}");
    Console.WriteLine($"---------------");

    //if (manarocks.Any())
    //{
    //    foreach (var m in manarocks)
    //    {
    //        Console.WriteLine(m.Name);
    //    }
    //    var x = "";
    //}
}

Console.Write("finished");