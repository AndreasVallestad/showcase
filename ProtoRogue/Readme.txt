Project consists of multiple gameplay systems for a traditional roguelike, made in the Unity Game Engine.

Some of the features included:

- Component based gameplay architecture, using separate behaviour-oriented scripts and interfaces.
- Gameplay loop that ticks relevant actors and gives the player full control of when turns happen.
- Generic level grid that can be populated and changed at runtime, and can override Unitys built in tilemap system to display visuals with better performance.
- Breadth-First-Search pathfinding system that reads from the level grid, allowing dynamic obstacles and changes to the level, without breaking.
- Character grid movement system, separated from the decision-making character controllers. May use BFS pathfinding, player input, or strictly defined rules.
- Attacking, damage, health, and death functionality.
- Optional attack-pattern system, that allows for certain actors or equipment to make attacks that cover specific areas of the levelgrid, relative to the actor.
- Generic Finite State Machine for organization of actor behaviours, or gameplay AI.
- Incomplete bodypart health system, that would allow simulation of damage to individual parts of each actors body.
