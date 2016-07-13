# Changelog

## Executive Summary

### UI
  * When starting a local game, mod loading progress is displayed to the user.
    * Currently broken - 7/12/2016

### Modding
  * Mods based on Gravity Generators can now define a default field size and strength.
  * Mods can now re-implement some gravity generator code, if needed.
  * Blocks using Gravity Generator behaviors are no longer forced to play the generator's droning sound.
  * See [Porting Gravity Generator Mods](docs/porting/gravity_generators.md)
  * <s>DX11 renderer now uses user's skybox preferences, and is (somewhat) backwards-compatible with DX9 skyboxes.</s>
    * This is a stock thing now.
