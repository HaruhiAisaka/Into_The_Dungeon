# Design Doc for Dungeon Game

## Overview

### Description
This 2D video game creates procedurally generated dungeons where the player is tasked to find a chalice. The dungeon is filled with items, traps, weapons, and enemies for the player to interact with. The dungeons are procedurally generated so that every single dungeon is a new experience for the player.

The original Legend of Zelda (NES) is what inspired this idea. Just think Zelda but procedurally generated. [Example: Legend of Zelda first dungeon.](https://www.youtube.com/watch?v=U4wptz77hKg)

### Intended Audience
The intended audience will be fans of retro action-adventure games or roguelikes who want a short burst of entertainment. The expected audience will most likely be adult men between the ages of 20-45 years. Many of these people liked playing video games as children (in the age of the NES and SNES) but don’t have the time to commit themselves to a long action-adventure game.

### Scope
The game will at first only generate procedural dungeons. However, if we successfully make this version of the game, it can be expanded so that we can include a whole overworld to explore. The game can start small but is scalable.

### Intended Hardware
The game is intended for PCs or any game console which uses a basic controller. It can be converted for mobile devices but the game’s controls will favor regular controllers.


## UI

### UI During Gameplay
The screen will display a 2D overhead view of a singular room of the dungeon where the player is. The room will cover the entire screen with small UI elements on the top left and right for health and item’s equipped.
![UI_During_Gameplay](Sketches_Images/ui_gameplay.PNG)

### UI During Pause
A pause menu will also be accessible where the player can equip items and view a map of the dungeon. This map will consist of all rooms visited by the player (if the map item is discovered then the entire dungeon map will be revealed). The player will be free to add pins on to the map to document possible secrets or things of interest.
![UI_During_Pause](Sketches_Images/ui_pause.PNG)


## Gameplay

### Player Actions:
The player will be able to move in the 4 cardinal directions and have two action buttons. Movement will be WASD keys on the keyboard while the two action buttons will be J and K (or the two mouse buttons we can decide later).

The first action button is the main action button which controls the player’s sword. The sword can be swung in front of the player.

The second action button is the item action button. The action depends on the item selected by the player. For example, if the player equips a bow and arrow the player will shoot arrows. If the player equips bombs he will pull out a bomb over his head and the player will be able to throw it.

The unlocking of doors or the pushing of blocks will be done if the player moves into a door or a block. For example, if the player wants to push a movable block, all he has to do is run up into it as if he’s pushing it.

### Room Layout/Visual Secrets
All rooms will have the same rectangular dimension. The items, blocks, enemies, stairways, traps, etc. will be randomized. Visual indicators will be used to notify the player of secret passageways that can be accessed given the right item or condition. For example, a cracked wall may indicate that a bomb may be placed there or an oddly placed block may indicate that it’s movable. Secret passageways may or may not be necessary for progression given the difficulty setting that the player selected.

### Dungeon Layout
The layout of dungeons will also be randomized but confined to a 2D array. In other words, there are spaces within a rectangular 2D grid where rooms can or can not exist. Whether these rooms exist in the space and how each room is connected to one another will be procedurally generated.

### How Rooms are Connected
Rooms can be connected via a door if they are adjacent to each other. Rooms may also be connected via stairways. If the player enters a stairway, they will re-emerge at the other room at the end of the stairway. There is no transition scene or room between the two.

### Keys:
Keys will be used to limit the rooms that are accessible to the player. The game will not use the original Zelda “one key opens one locked door” method. Instead, the dungeon will be divided up into sections where a room is only accessible given that the player has the right colored key. For example, the red section of a dungeon will only be accessible if the player has a red key. Locked doors will be strategically placed in order to ensure this outcome.

### Enemies
A room may consist of enemies that the player may fight. The enemies may drop items randomly such as arrows or bombs. In other cases, the player may be ambushed where all escape routes are locked and the player will be forced to defeat all enemies in the room to free himself.

A list of enemies can be decided upon later. The base game should be designed first. Right now a simple enemy can be added (one that moves aimlessly and the player is damaged if it comes in contact with it).

### Health
The player has a set amount of health that can be regenerated by hearts dropped by enemies. Health is decreased if the player comes in contact with a damage-dealing object (such as an enemy or a projectile).

### Items
The player can collect items to help in exploring the dungeon. Items may include non-equipable items (non-equipable meaning can’t be assigned to the item action button) like keys, shields, armor, and equipable items like bombs, arrows, etc.

Currently, these should be the only items required to make a basic version of the game:

| Item  | Uses | Availability |
| ----- | ---- | ------------ |
| Basic Sword | To attack enemies | Given to the player at the beginning of the game|
| Bombs | Can be thrown by the player, short fuse starts once the bomb is thrown. **Limmited Amunition** Visual indication used to show when the bomb will blow. Used to kill enemies and blow up cracked walls to reveal secret rooms. | Random placement in dungeon. Dropped by enemies once the item is picked up. |
| Keys | Used to unlock new areas of a dungeon. See key section above. | Random placement in dungeon. |
| Hearts | Used to regain health. | Dropped by enemies. |

### Chalice
The goal of the game is to find the chalice that is located at the “end” of the dungeon. This chalice should only be accessible once the player has found all keys to the dungeon. 

Once the player touches the chalice, the game ends in victory.

In the future, the chalice should be guarded by a final boss of sorts. However, this shouldn’t be developed until we have the base game in order.


## Procedural Generation

### Generation of Dungeon
What should be generated first is an algorithm that connects the dungeon. The dungeon should be generated on a 2D grid (the size of the grid may differ depending on difficulty). 

**Rules for Dungeon Generation**
*Key*
![Dungeon Generation Key](Sketches_Images\generation_dungeon_key.PNG)
1. All rooms must be orderly and within the confines of a 2D square grid.
![Ordered Dungeon](Sketches_Images/generation_dungeon_1_good.PNG)
![UnOrdered Dungeon](Sketches_Images/generation_dungeon_1_bad.PNG)
2. The number of rooms will be between a small random range depending on the difficulty selected.
3. A group of rooms all connected to eachother by doors is considered a cluster. No more than 4 clusters can exist in a dungeon.
![Bad Clusters](Sketches_Images\generation_dungeon_3_bad.PNG)
![Good Clusters](Sketches_Images\generation_dungeon_3_good.PNG)
4. A starting room must be selected.
5. A chalice room must be selected. The chalice room must only have once enterance.
6. The room for the final boss must be adjacent to the chalice room. The final boss room must have only two doors, one that connects to the rest of the dungeon. Once that connects the room to the chalice room. The final boss room must not be accessable from the start room.
![Follows Rules 4-6](Sketches_Images\generation_dungeon_6_good.PNG)
![Invalid Rules 4-6](Sketches_Images\generation_dungeon_6_bad.PNG)
7. All rooms must be connected either by doors or staricases from the starting room. *The definition of "door" will be used for regular, unlocked doors, locked doors, or chacked walls that can be opened with a bomb. In otherwords, a door is simply a connection from once adjacent room to another.*
![Rooms Connected](Sketches_Images\generation_dungeon_7_bad.PNG)
![Rooms Unconnected](Sketches_Images\generation_dungeon_7_good.PNG)
8. There will be a limit on the number of staircases. Staircases do not nessecaraly have to be a replacement to doors. A room accessable by both doors and staircases is possible.
![Staircase and Door](Sketches_Images\generation_dungeon_8_good.PNG)
9. The dungeon must be divided into colored sections which are unlocked via the same colored key. For example, the red section of a dungeon can be accessed once the red key is colected. All doors between colored sections will be locked.
![Colored Sections Seperate](Sketches_Images\generation_dungeon_9_seperate.PNG)
![Colored Sections Overlap](Sketches_Images\generation_dungeon_9_overlap.PNG)
10. Keys must be placed where they can be accessed. In otherwords, a red key can not be located in the red section of a dungeon. Atleast one key must be accessable without the use of other keys.
![Accessable Seperate](Sketches_Images\generation_dungeon_10_accessable_seperate.PNG)
![Accessable Overlap](Sketches_Images\generation_dungeon_10_accessable_overlap.PNG)
11. All other items must be placed and accessable without useing that item. For example, bombs can not be located in a room that can only be accessed by bombing a room.
![Bomb Accessable](Sketches_Images\generation_dungeon_11_good.PNG)
![Bomb Unaccessable](Sketches_Images\generation_dungeon_11_bad.PNG)
12. Strength of enemies will increase depending on their distance from the starting room (distance meaning the shortest route between the room and the starting room.)

### Generation of Rooms
Once the dungeon is generated, individual rooms will be generated also. Each room will have data that records what items the player can have access too. This data will determine the placement of enemies or secrets.

**Rules for Room Generation**
1. All enterances and exits must be accessable by the player.
2. All walkable tiles of a room must be accessable.
3. There will be a limit on the number of enemies per room so that the player will not be overwhelmed by projectiles or enemies.
4. All movable blocks must be movable.