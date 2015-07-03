# Changelog

## Executive Summary

### UI
  * When starting a local game, mod loading progress is displayed to the user.

### Modding
  * Mods based on Gravity Generators can now define a default field size and strength.
  * Mods can now re-implement some gravity generator code, if needed.
  * Blocks using Gravity Generator behaviors are no longer forced to play the generator's droning sound.
  * See [Porting Gravity Generator Mods](docs/porting/gravity_generators.md)
  * DX11 renderer now uses user's skybox preferences, and is (somewhat) backwards-compatible with DX9 skyboxes.
    * Still needs some work done to import colors and shaders.