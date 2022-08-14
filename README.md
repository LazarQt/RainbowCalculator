# Rainbow Calculator
## Calculate the perfect mana base for your EDH deck

Epic.

# Methodology 

The goal is to find a set of lands that have enough sources for each color, taking curve and hard to cast cards into account. The following steps explains how the program tries to find this specific set of cards.

Note: Utility lands, man lands and other lands that do NOT produce colored mana, are completely ignored. Feel free to include them by replacing a land with same produced mana.

## Gathering data for calculation

First calculate how many lands are needed, this is done by looking at Frank Karsten's calculations here: 
https://strategy.channelfireball.com/all-strategy/mtg/channelmagic-articles/how-many-lands-do-you-need-to-consistently-hit-your-land-drops/

Lands | Avg min MV | Avg max MV 
--- | --- | --- |
30 | 0 | 0.80 
32 | 0.80 | 1.12 
33 | 1.12 | 1.44 	
35 | 1.44 | 1.76 	
37 | 1.76 | 2.08 	
38 | 2.08 | 2.40 	
40 | 2.40 | 2.72
42 | 2.72 | 3.04 	
43 | 3.04 | 3.36 	
45 | 3.36 | 9

By using mana rocks we can cut down on lands to include some ramp. Going by general census, we can cut one land for 2 other mana sources. (Technically Scry effects and the like account for 0.XX mana sources or so but that's going too far)

The higher the required land count (and therefore average mana value) the more mana rocks are needed.

Lands | Avg min MV | Avg max MV | Mana Rocks | New Lands
--- | --- | --- | --- | --- |
30 | 0 | 0.80 | 0 | 30
32 | 0.80 | 1.12  | 0 | 32
33 | 1.12 | 1.44  | 4 | 31
35 | 1.44 | 1.76  | 4 | 33
37 | 1.76 | 2.08  | 6 | 34	
38 | 2.08 | 2.40  | 6 | 35	
40 | 2.40 | 2.72 | 8 | 36
42 | 2.72 | 3.04  | 8 | 38	
43 | 3.04 | 3.36  | 10 | 38	
45 | 3.36 | 9 | 10 | 40

Some cards have reduced costs and will in most cases not be cast for their full cost. We need to exclude these cards so they don't mess up the math. This Scryfall query finds most of them:

```
 -is:digital f:edh 
 ((fo:cost fo:"less to cast")
 or 
 fo:affinity or fo:delve or m>=x
 or
 (o:convoke fo:"can help cast this spell") 
 )
 -( fo:"that target ~" fo:cost o:less) 
 -(o:"this spell costs {1} less to cast if" or o:"this spell costs {2} less to cast if") 
 -o:"spells you cast cost {1} less to cast" 
 -o:"spell you cast costs {1} less to cast" 
 -(o:"spells you cast cost" o:"less to cast") 
 -(o:"spells with the chosen name cost" o:"less to cast")
 -(o:"spells with the chosen name you cast cost" o:"less to cast")
 -(o:"spells you cast of the chosen type cost" o:"less to cast")
 -(o:"spells with" o:"you cast cost" o:"less to cast")
```
Manually excluded (query will get too long otherwise) are:

```
Animar, Soul of Elements
Arcane Melee
Delver of Secrets
Conduit of Ruin
Extus, Oriq Overlord
Goblin Anarchomancer
God-Eternal Kefnet
Goreclaw, Terror of Qal Sisma
Gravebreaker Lamia
Hardened Berserker
Helm of Awakening
Invasion of the Giants
Jungle Delver
Kadena, Slinking Sorcerer
Kaza, Roil Chaser
Killian, Ink Duelist
Krosan Drover
Maelstrom Muse
Mascot Interception
Mistform Warchief
Monk Class
Morophon, the Boundless
Myth Unbound
Obsidian Charmaw
Patrician Geist
Peerless Samurai
Phyrexian Delver
Sage of the Beyond
Saheeli, the Gifted
Semblance Anvil
Spectacle Mage
Tezzeret, Master of the Bridge
Thryx, the Sudden Storm
Urza's Filter
Urza's Incubator
Uvilda, Dean of Perfection
Vine Gecko
Visions of Dominance
Visions of Dread
Visions of Duplicity
Visions of Glory
Visions of Ruin
Will Kenrith
```

To be very precise a lot of those cards should not simply be excluded but rather adjusted. A card with X in its cost can vary a lot but a card with "Undaunted" or "Party" can only go down in cost by a certain fixed amount. This could be an exercise if I have a lot of time and nothing else to do, for this this will suffice.

If you're creaing a deck and want to exclude certain cards because you ramp into them anyway or cheat them into play without ever casting it, there will be a command argument to exclude those cards manually. 
// todo: exclude
// todo: discord bot to exclude // omg

By looking at Karsten's charts we can also deduct the following mana source requirements needed for different mana values and color pips:

Cost | Pips | Sources
--- | --- | --- |
1 | 1 | 23
2 | 1 | 21
2 | 2 | 33
3 | 1 | 19
3 | 2 | 29
3 | 3 | 37
4 | 1 | 17
4 | 2 | 26
4 | 3 | 33
4 | 4 | 38
5 | 1 | 15
5 | 2 | 23
5 | 3 | 30
5 | 4 | 36
5 | 5 | 41
6 | 1 | 14
6 | 2 | 22
6 | 3 | 28
6 | 4 | 35
6 | 5 | 39
6 | 6 | 43

If there are errors feel free to point them out.
Also, if a card has a completely different cost (or higher cost) it gets ignored.

## Add mana rocks to deck

First step is to include the mana rocks using previous calculations. The tool only considers all the dual colored 2-mana cost artifacts (Signets, Talismans) and WUBRG producing rocks like Arcane signet. If you see "Generic Rock X" in your results, this means you can include any 3-mana rock (there are too many of them, pick the one that fits your deck best).

## Add a basic land for each color

As the title suggets, one of each basic land is added. This ensures that there's always a fetch-able source for cards like Evolving Wilds. (The inclusion of basic lands is up for debate)

## Add lands

This is the most crucial step. The program now knows exactly how many sources of a given color is needed. By going through the "Best" lands (there is a separate list for 2, 3, 4 and 5-colored decks) and adding them to the deck, the amount of sources needed is being lowered. If there is no need for a specific color anymore, all the lands that produce this color are ignored (This is up for debate).

## Fill deck with basic lands

At the end, deck is filled with basic lands if there's space left.