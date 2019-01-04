![](/docs/GodViewBanner.png)

----

This is a engine made using the Godot engine. It is meant to be used for top down games. It is also currently under development by *@antopilo* and *@emerycp*. It is fairly new still, but a lot more is planned. We won't support this engine, so if you plan to use this, you've been warned.

## Planned features

At this stage there is a LOT to still to implement. but here is what is planned so far. Note : This can change during development.
TODOS:
- [ ] Node system(Level meta modification).
- [ ] Layer system.
- [ ] TileMap Editor.
- [ ] Level saving(JSON).
- [ ] Warping system(linking levels).
- [ ] Loading levels from file.

## Editor
We are planning to do an in-game editor that supports tiled based editing, and node based editing. We also want to be able to Save the levels in a human readable format like JSON. We will also have per-level environment settings, for example Lighting, Mood, Theme, etc.
We want to have a WYSIWYG-style editor with Nodes representing non-visible objects. The editor should also be able to be toggled on and off on run time for easy testing. The editor should be modular. It uses components and components should work without any other dependencies to interact in the game. Developping new components is very easy if you are following those simple rules. Good components should be hassle free to implement in the editor.

## Player

We have a simple Player Controller. Since player movement is always unique per game. We don't want to make a full fledged player controller, but still have some basic elements, like spawning, changing levels and what not(will change).

TODOS:
- [X] Basic movement
- [ ] Art
- [ ] Projectiles
- [ ] Advanced movement
- [ ] Progression system
- [ ] Character customization

## Levels

Levels will support multiple layers that can customized, Godview support levels as big as you want! You can either make small rooms or big open worlds that are inter-connected however you want them.

----

more will come in the future :) stay tuned
