This project is a small Unity prototype demonstrating a character-based gameplay system built with a focus on clean architecture, separation of concerns, and reusability.
The prototype features a controllable tank unit, AI-controlled bots, team switching mechanics, and basic combat on a simple arena.
The same character model and behavior system is shared between the player and AI.
Target platform: Mobile
Engine: Unity
Input: Unity Input System

?? Core Features
* Shared Character Model for player and bots
* Modular systems:
o Movement
o Attack
o Health
o State / Behavior handling
* Player-controlled unit with mobile-friendly input
* AI bots using the same logic and states as the player
* Team switching mechanic:
o When a unit reaches 0 HP, it changes team instead of dying
o HP is restored and the unit continues to act for the new team
* Directional vision & attack logic (tank-style movement)
* Simple arena-based gameplay

?? Architecture Overview
The project follows a Model–View–Controller–like approach, adapted for Unity.
Core Layers
Model (pure logic)
 ?? CharacterModel
 ?? HealthComponent
 ?? AttackComponent
 ?? MovementComponent

View (MonoBehaviour)
 ?? CharacterView
 ?? HealthBarView

Controllers (non-MonoBehaviour)
 ?? PlayerController
 ?? BotController
Key Principles
* Models contain no Unity-specific logic
* Views only represent data and forward events
* Controllers handle decision-making and behavior
* AI and Player use the same movement and attack systems

?? Character System
Each character has:
* Team identifier
* Race (extensible)
* Health system
* Attack system
* Movement controller
* State / behavior logic
Team Switching
When HP reaches zero:
1. Character changes its TeamId
2. HP is reset to a base value
3. Character continues gameplay as part of the new team
This mechanic is handled by game rules, not by the character itself.

?? AI Behavior
Bots use the same character logic as the player.
Bot behavior flow:
* Roam between random points
* Detect enemies in front using directional checks (dot product)
* Move toward enemy targets
* Attack when the target is within attack conditions
* Convert defeated enemies to their team
Optional extensions:
* Group movement for same-team units
* Priority targeting
* Obstacle-aware navigation

?? Controls
* Movement: Virtual joystick / touch input
* Attack: Dedicated attack button
* Character rotates to face movement or attack direction
* Designed for mobile input, but works in Editor with mouse

?? How to Run
1. Open the project in Unity
2. Change platform for Android and choose simulator
3. Load the “Bootstrap” scene 
4. Press play
5. Use touch/mouse controls to move and attack

?? Possible Improvements
* Using Layer Mask
* Using Addresables
* Make SpriteAtlasses For every screen/popup
* Add new FieldView, IMovementController (with stamina), IAttackExecutor (with range/radius)
* Improve UI logic
* Save/load system
Author
Developed by Glib (Unity Developer)
Feel free to reach out for questions or discussions about the architecture.

