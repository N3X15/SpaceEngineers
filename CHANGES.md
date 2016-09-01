# Changelog

## Executive Summary

NOTE: SECE will not connect to stock servers due to protocol differences and a different version scheme.

### Compiling
  * SECE uses a python script to prepare the CSPROJ files for compile. (Otherwise, it'll fail to find dependencies.)
  * Compiled with Visual Studio 2015 CE instead of VS2013 (shouldn't cause any issues)

### UI
  * When starting a local game, mod loading progress is displayed to the user.
  * When loading or starting a local game, the loading screen displays more details about what is going on in the background. (Introduced 8/13/2016)
  * SECE adds a _CE### suffix to the version, making it easier to keep track of SECE hotfixes.
  * `Edit Settings > Advanced` lets you change a save's max speed.  Works with any Skybox. -  8/18/2016
    * Skybox hacks to change speeds no longer work, but won't break anything.

### Modding
  * Mods are compiled with debugging information, which provides line numbers in stack traces, among other things. - 8/18/2016
  * Gravity Generators
    * Mods based on Gravity Generators can now define a default field size and strength.
    * Mods can now re-implement some gravity generator code, if needed.
    * Blocks using Gravity Generator behaviors are no longer forced to play the generator's droning sound.
    * See [Porting Gravity Generator Mods](docs/porting/gravity_generators.md)
  * <s>DX11 renderer now uses user's skybox preferences, and is (somewhat) backwards-compatible with DX9 skyboxes.</s>
    * This is a stock thing now.
  * Added MyCubeGrid.RemoveBlock(IMySlimBlock, [bool]) for ModAPI users.
    * Needed for [Nanite Control Factories](http://steamcommunity.com/sharedfiles/filedetails/?id=655922051) mod. Stock mod will not run on dev builds of SE, so use [this](https://gitlab.com/N3X15/SECE-NaniteControlFactories).

### Bugfixes
 * Fixed NullReferenceException in MyShipController - 7/17/2016
 * Fixed outdated physics code in Sandbox.Game that resulted in compile failures.  Required decompiling Steam-delivered game. Fixes KeenSoftwareHouse/SpaceEngineers#554 - 8/12/2016

## Requesting Changes

Make an issue or pull request [here](https://github.com/N3X15/SpaceEngineers-CE/issues).
