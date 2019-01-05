# Main scene structure

This document describe the current state of the Main scene(Main.tscn). This document should be clear enough so that anyone can understand the structure of the project. The Main scene is the scene that is run when the game is launched. GodView engine has 2 Important concept that are independent of each other. There is the currentLevel node, this node is the level that would be read by the game. The CurrentLevel node should be stand-alone and can be run by the game without the other part. At the same level in the hierarchy. there is the EditorHandler. This node is where the level editing happens. It is where it holds the reference to the CurrentLevel itself. Here is a screenshot of the hierarchy of the project so far.

> NOTE: this is subject to any changes during developpement.

> As you can see The Scene Contains 3 distinct node. "CurrentLevel", Editor Handler and the "GameHandler". The "GameHandler "sole purpose is to handle the transition between the game view and the editor view. It is meant to keep in memory the player position, and objects states for seamless transitions.

## Defining a Level

A Level always follow the same structure, It has a defined Size starting from the upper-left corner. The level size is used to define the limit of the cameras. It has a type that is either a World, a Room or a dungeon. It has a minimum of 1 spawn point if the level can kill the player. 

### Layers

A level can have an unlimited amount of layers, but the minimum is 3("MainGround", "Entities", "Environment"). The MainGround is the walkable surface, Entities holds any entities of the level, the Environment is the Collider layers(For example fences, rocks or walls). Each layer contains at least 1 tilemap(Tilemaps are a grid of tiles that can be draw unto. See Godot Docs for more info.). In the editor view, there is the ability to add any amount of layers, set them to be hidden or not. Layers are useful for ordering object on the Z-Axis but are mainly useful to organize your levels. You can rename them however you want.

### Nodes

> Don't confuse with Nodes from Godot. We will probably rename them in the future.

Nodes are non-visible in the game that serves as Holder for data. A Good example would be a Sun Node that holds settings that sets the mood of the level. Its a non-visible entity in the game that is represented as a Node In the editor. We don't have a specific list of nodes at the moment, we will create them as we development the engine. We chose that approach because we don't know yet what node will be useful.
