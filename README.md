# Rainbow Calculator 🌈➗

Tha goal of this library is to calculate the mana source requirements (lands and mana generating artifacts) of a regular EDH/Commander deck in the trading card game Magic: the Gathering.

## Goal 

Only mana producing lands are taken into account. Suggesting the perfect lands is impossible due to the various goals a deck is trying to achieve. But it should give a player a solid starting point where they can safely assume that:

- The land ratio is mathematically optimal
- The amount of mana producting artifacts is perfect for the given mana curve
- The amount of colored sources produced ensures that spells can be cast on curve

# Technical overview

This section adresses how the tool is setup from a technologial point of view. If you're interested in the methodology, skip to [Here](#Methodology). 

## API Setup

It's a simple API written in .NET 6 with only two endpoints, one of which is only used for testing purposes. This way any client can call the API and implement it the way they see fit. I chose to do it with a [Discord bot](https://discord.com/developers/docs/intro). See [this repository](https://github.com/LazarQt/RainbowFrog) on how it works and try it out yourself!

## API Call

**Request**

`POST: host:port/`

Parameters:

`Decklist` string array of the entire deck
`Ignorelands` string array of lands that should be ignored (in case user does not own this particular land)


DELETE


Endpoint for making a simple get request and ensuring the server is running correctly:

**Request**

`GET /api/ping/`

    curl -i -H 'Accept: application/json' http://host:port/api/ping/

**Response**

    HTTP/1.1 200 OK
    Status: 200 OK
    Content-Type: application/json

    {
        id: 1,
        name: "Ping successful"
    }

Endpoint to request mana base suggestion:

**Request**

`POST /api/manabase/`

    curl -i -H 'Accept: application/json' -d 'decklist=["Card 1","Card 2"]&ignorelands=["Badlands"]' http://host:port/api/manabase/

**Response**

    HTTP/1.1 201 Created
    Status: 200 Created
    Connection: close
    Content-Type: application/json

    {
        "data": 
        {
            "error": null,
            "averageManaValue": "2.345",
            "relevantCardList": [
                "Card A",
                "Card B",
                "Card C"
            ],
            "cardsNotFound": [],
            "excludedCards": [
                "Card X",
                "Card Y",
                "Card Z"
            ],
            "removedLands": [
                "Land 1",
                "Land 2"
            ],
            "sources": [
                "1 Land A",
                "2 Land B"
            ],
            "sourcesCount": 2,
            "colorRequirementsErrors": [
                "Ignoring hybrid mana cost of this card",
            ],
            "colorRequirements": [
                {
                    "color": "b",
                    "amount": 33,
                    "amountFulfilled": 33,
                    "isFulfilled": true
                },
                {
                    "color": "g",
                    "amount": 29,
                    "amountFulfilled": 33,
                    "isFulfilled": true
                }
            ],
            "manarockRatio": {
                "landsWithoutRocks": 38,
                "minMv": 2.08,
                "maxMv": 2.4,
                "manaRocks": 6,
                "lands": 35
            }
        }
    }

`error` If calculation throws an error, this is where it is shown

`averageManaValue` Average mana value of deck

`relevantCardList` All the cards that are "relevant" to the calculation (some cards are excluded, based on methodology)

`cardsNotFound` Cards that are not found (typo?)

`excludedCards` Cards taht are excluded because they are not required for calculation

`removedLands` Lands that were removed prior to calculation in case user forgot to take them out

`sources` Suggested sources

`sourcesCount` How many sources there are

`colorRequirementsErrors` If a card is NOT taken into account when it comes to calculations, this is where it is shown why

`colorRequirements` Color requirements for each color

`manarockRatio` How many lands and mana rocks are needed

# Methodology

If you're interested how the tool works without going through the code, here is how it works. 

If you disagree with any of this or have suggestions, feel free to open an issue or drop me a message.

## Gathering data for calculation

Luckily for a game that's almost 30 years old, there are many resources out there from people that have done some calculations themselves.

### Land count

First calculate how many lands are needed based on the average mana value of the deck, this is done by looking at Frank Karsten's calculations [here](https://strategy.channelfireball.com/all-strategy/mtg/channelmagic-articles/how-many-lands-do-you-need-to-consistently-hit-your-land-drops/).

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

*Example: For a deck with an average mana value of 2 there should be 37 lands.*

### Mana generating artifacts (Mana rocks)

By using mana rocks we can cut down on lands to include some ramp.

> Ramp - A card which accelerates your mana, giving you an additional, reusable mana source beyond the usual one land per turn.

I was not able to find a definitive mathematical answer on how many mana rocks to include but the general sentiment seems to be: For every 2 mana rocks, a land can be cut from the deck. This is the calculation used here.

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

*Example: As established before, 37 lands are needed for a deck with average mana value of 2. However, by including 6 mana rocks, this number can be reduced to 34 which means there are a total of 40 mana generating sources.*

### Cards excluded from calculation

Some cards have reduced costs and will in most cases not be cast for their full cost. Those cards need to be excluded or they will mess up the math. This [Scryfall](https://scryfall.com/) query finds most of them:

```
  -is:digital f:edh 
 (
	 keyword:affinity 
	 or
	 keyword:delve
	 or m>=x
	 or keyword:convoke
	 or keyword:improvise
	 or keyword:undaunted
	 or (fo:"this spell costs {" and fo:"less to cast")
 )
```

To be very precise a lot of those cards should not simply be excluded but rather adjusted. A card with X in its cost can vary a lot but a card with "Undaunted" or "Party" can only go down in cost by a certain fixed amount. For now this will suffice.

*Note: Some decks have cards that are almost never going to be cast the "normal" way but rather by cheating them into play in some way. If this scenario comes up a lot, one parameter could be added to manually exclude a list of cards from the calculation.*

### Pip calculation

> Pip - A mana symbol in a card's casting cost, when counting them. Not to be confused with converted mana cost. For example, [Gray Merchant of Asphodel](https://scryfall.com/card/thb/99/gray-merchant-of-asphodel) has 2 pips and mana value of 5.

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

## Creating mana base based on gathered data

There are a few simple steps now to create the mana base. It's going to cover all the discussed bases and be a solid starting point or even the final version. You can fill your deck with exactly those suggested lands and would have a fully functional mana base. However, depending on the deck strategy, a few substitutions could be made.

*For example: If the deck strategy revolves around sacrificing creatures, a land could be replaced with [High Market](https://scryfall.com/card/afc/246/high-market) which DOES NOT produce colored mana but serves as an important synergy piece.*

### Add mana rocks to deck

First step is to include the mana rocks using previous calculations. The tool only considers all the dual colored 2-mana cost artifacts (Signets, Talismans) and WUBRG producing rocks like Arcane signet. If you see "Generic Rock X" in your results, this means you can include any 3-mana rock (or any other cheaper or more expensive accelerator for that matter, there are too many of them, pick the one that fits your deck best).

Here are the common options, ranked by EDH viability: [EDH Mana rocks](https://scryfall.com/search?q=c%3Ac+t%3Aartifact+o%3Aadd+f%3Aedh+-t%3Acreature+-t%3Aland&unique=cards&as=grid&order=edhrec)

### Add a basic land for each color

There are many situations where having a basic land comes in handy:

- Fetch-able with fetch lands
- It's a land that is not affected by cards that turn off generation of mana from non basic sources
- It's something to search for when instructed by opponent's cards that make you get a basic land in addition to its other effects

The inclusion of basic lands is up for debate, personally I like it.

### Add lands

This is the most crucial step. The program now knows exactly how many sources of a given color is needed. By going through the "Best" lands (there is a separate list for 2, 3, 4 and 5-colored decks) and adding them to the deck, the amount of sources needed is being lowered. If there is no need for a specific color anymore, all the lands that produce this color are ignored (This is also up for debate).

This step stops as soon as there are enough sources if the rest of the needed lands can be filled up with sources that produce a single type of color.

### Fill deck with basic lands

At the end, deck is filled with basic lands.

### Criteria not met

The program will loop back to the start and try again if it requires more lands than projected to fulfill the mana source requirements. In that case it will take "worse" lands first that produce more colors.

If this also doesn't help, you get an error!