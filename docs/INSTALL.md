# Installing SECE


## Prerequisites

* .NET 4.6.1 (included with SE)
* Steam
* Legally-owned Space Engineers on Steam
* 7-zip

## Installer

SECE can only be installed with the SECE installer, which does its best to ensure
that you actually have a legal copy of the game.  Because SE is picky about
install location, it also checks to ensure SECE is being installed to a Steam
Library folder (otherwise, SECE will exit and launch the SE instead).

1. Download the SECE .7z to somewhere accessible.
2. Extract the contents to a convenient location.
3. Enter the directory in Explorer and launch Installer.exe.
4. Accept the UAC prompt, if it presents itself.
5. Choose your Space Engineers install directory in the top textbox, if it is not already. (`...\SteamApps\common\Space Engineers`)
6. Select an install directory for SECE.  Sece must be installed in a directory under `...\SteamApps\common\`, and inside a Steam Library folder!
  * For example, I use `F:\SteamLibrary\SteamApps\common\SECE`, which is on an external hard drive and under my `F:\SteamLibrary` Steam Library.
7. Press "Validate" to run an integrity check of SE.
  * If this fails, your SE install cannot be patched, for technical reasons.  This can happen if SE has a release that we haven't patched yet, or if your install is corrupt.
8. Once the integrity check passes, you will be able to press "OK" to begin the installation process.  **This can take a long time and will be slow.** Be patient.
9. Once SECE is installed, browse to the installation folder and run `RunGame.bat` to start the game. Do not launch SpaceEngineers.exe directly or it'll restart as the unpatched Steam version.

## What's Included?

You're probably wondering what all those .bin files are.  Those are the files
that differ from the stock game, plus new files that the stock game does not
include.  SECE is packaged this way in order to comply with Space Engineers'
code usage policy and work around distribution restrictions.  The way we
package SECE ensures that the only way to make the patch work is if you have
a unedited copy of the game in Steam.

However, we acknowledge that determined individuals can work around this system,
and we don't desire to get into an arms race with them.  We do not condone
unauthorized distribution of this patch, and ask that you do not distribute
SECE without a similar system in place. This is the best we can come up with,
given limited time and budgets.

## Why is the install so slow?

Every file in the SE installation directory is currently checked against a list
of signatures in the SECE manifest.  This requires running a cryptographic
function against all files in SE, which adds significant time to the install.

## Can I distribute/mirror SECE?

You may distribute SECE, but only if it is packaged using the SECE Installer.
We forbid distribution of the unpackaged binary form of SECE, for legal reasons.
You may distribute the source code version of SECE as specified in the
Space Engineers Source Code EULA.

## WHY DID YOU USE C#/WPF/etc?
Because the other option would have been pycompiled Python + Qt, which would have
pissed even MORE people off and would have bloated up the archive.

## Installer Source Code
N3X15 is still debating whether to release it, as he may use it in other
projects that would require even more restrictions on distribution. It's also
still under heavy development.

## Support/Discussion
* [GitHub](https://github.com/N3X15/SpaceEngineers-CE)
* [irc.rizon.net #sece](irc://irc.rizon.net/sece)
  * Installer - N3X15
  * Devs - Ops/Halfops
* 4chan /vg/ - /egg/ (Engineering Games General)
