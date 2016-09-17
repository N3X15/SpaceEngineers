# Changelog

This page will record changes made to SECE.

## 9/16/2016 - 01_152_002CE4
* UPDATED to use SE DEV 01_152_002.

### Changes
* **Added:** A hidden SE feature that allows you to set the speed limit on autopilots has been unhidden. (MyFakes.ENABLE_VR_REMOTE_BLOCK_AUTOPILOT_SPEED_LIMIT)
* **Fixed:** Some verbose startup messages were made a bit more detailed, and some new ones were added.

## 9/11/2016 - 01_148_002CE3
* Based on SE DEV 01_148_002

### Installer
* Installer will no longer finish silently.  A popup will detail file changes, and provide game start procedure.

### Changes
* **Added:** New versioning scheme (_CE# suffix indicating SECE release).  Breaks compatibility with all SE servers.
* **Fixed:** Air Vents were not saving actions properly, leading to failed saves.
* **Fixed:** Gravity generator was not symmetrically converting to Gs and back.

## 01_148_001-o-01_149_002
* Translation: 01_148_001, overwrites 01_149_002

### Changes
* **Added:** Game compiles mods with debugging information.  This means mod crashes will now include line numbers.
* **Added:** `Edit Settings > Advanced` now includes large ship and small ship max speed sliders.  No more hacking skyboxes.

### Packaged Mods
* **Added:** [Darky](http://forum.keenswh.com/threads/darky-shader-mod-make-darkness-great-again.7385983/), by plaYer2k

### Installer
* **Added:** Installer now includes a framework for installing mods requiring modification of the installed files.
* Fixed a problem with Steam library recognition failing to recognize directories due to case sensitivity.

## 8/13/2016 - 01_148_001
### Changes
* **Added:** Loading GUI verbosity - The loading screen will show background processes, for the curious.
* **Added:** Patcher now supports the dedicated server.

## 8/12/2016 - 01_146_006
### Notes
* Initial binary release.
* Accidentally built against the "stable" Keen release, but still works.

### Changes
* Uses decompiled code to fix compile issues. (Keen committed code incompatible with all shipped versions of HavokWrapper.)
