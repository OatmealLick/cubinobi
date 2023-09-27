# Cubinobi #

## Project Overview ##

There are couple of places which contain tweakable values.

### Settings ###

Term `Settings` refers to the tweakable properties, especially different kinds of timers or values.

In `Settings/` directory you can find `Settings` and stance settings like `BasicStanceSettings` or `FireStanceSettings`.
`Settings` is the master object which includes settings for each stance as well as overall settings that do not benefit from being changed per stance.
Stance settings contain values that can be tweaked per stance. 
For example, jump or attack properties can be changed per stance (each stance can have different values).

### Elemental Stance Resources ###

There is an additional Scriptable Object for static data, that should not change during gameplay.
These should contain things like which animations should be played for each stance, or icons to be displayed.
Currently it has the color properties for each stance that are being used in a rectangular overlay (stance indicator) on the player character.

## Keybindings ##

These should be consistent with GDD, making here a note for them.
Keyboard:
- WASD - W and D to move, WASD to indicate the direction of the attack
- SPACE - Jump (the longer you hold the higher you jump)
- RIGHT CLICK - Melee Attack
- CHANGE STANCE:
  - 000 Basic
  - 111 Earth
  - 222 Fire 
  - 333 Wind
  - 444 Water 
  - 555 Void

## Jump ##

Three kinds of gravities are used to create character jump.

![gravities-chart](jumpgravity.png)
