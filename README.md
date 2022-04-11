# Rainbow Calculator
## Calculate the perfect mana base for your EDH deck

Epic.

# Methodology 

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
32 | 0.80 | 1.12  | 0 | 30
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

# Dillinger
## _The Last Markdown Editor, Ever_

[![N|Solid](https://cldup.com/dTxpPi9lDf.thumb.png)](https://nodesource.com/products/nsolid)

[![Build Status](https://travis-ci.org/joemccann/dillinger.svg?branch=master)](https://travis-ci.org/joemccann/dillinger)

Dillinger is a cloud-enabled, mobile-ready, offline-storage compatible,
AngularJS-powered HTML5 Markdown editor.

- Type some Markdown on the left
- See HTML in the right
- ✨Magic ✨

## Features

- Import a HTML file and watch it magically convert to Markdown
- Drag and drop images (requires your Dropbox account be linked)
- Import and save files from GitHub, Dropbox, Google Drive and One Drive
- Drag and drop markdown and HTML files into Dillinger
- Export documents as Markdown, HTML and PDF

As [John Gruber] writes on the [Markdown site][df1]

> The overriding design goal for Markdown's
> formatting syntax is to make it as readable
## Tech

Dillinger uses a number of open source projects to work properly:

- [AngularJS] - HTML enhanced for web apps!
- [node.js] - evented I/O for the backend
- [Express] - fast node.js network app framework [@tjholowaychuk]
- [Gulp] - the streaming build system
- [Breakdance](https://breakdance.github.io/breakdance/) - HTML
to Markdown converter
- [jQuery] - duh

And of course Dillinger itself is open source with a [public repository][dill]
 on GitHub.

## Installation

Dillinger requires [Node.js](https://nodejs.org/) v10+ to run.



| Plugin | README |
| ------ | ------ |
| Dropbox | [plugins/dropbox/README.md][PlDb] |
| GitHub | [plugins/github/README.md][PlGh] |
| Google Drive | [plugins/googledrive/README.md][PlGd] |
| OneDrive | [plugins/onedrive/README.md][PlOd] |
| Medium | [plugins/medium/README.md][PlMe] |
| Google Analytics | [plugins/googleanalytics/README.md][PlGa] |

## Development

Want to contribute? Great!

Dillinger uses Gulp + Webpack for fast developing.
Make a change in your file and instantaneously see your updates!

Open your favorite Terminal and run these commands.

First Tab:

```sh
node app
```

Second Tab:

```sh
gulp watch
```

(optional) Third:

```sh
karma test
```

#### Building for source

For production release:

```sh
gulp build --prod
```

Generating pre-built zip archives for distribution:

```sh
gulp build dist --prod
```

## Docker

Dillinger is very easy to install and deploy in a Docker container.

By default, the Docker will expose port 8080, so change this within the
Dockerfile if necessary. When ready, simply use the Dockerfile to
build the image.

```sh
cd dillinger
docker build -t <youruser>/dillinger:${package.json.version} .
```

This will create the dillinger image and pull in the necessary dependencies.
Be sure to swap out `${package.json.version}` with the actual
version of Dillinger.
