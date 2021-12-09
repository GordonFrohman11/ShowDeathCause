# ShowDeathCause
This mod lets you know how your and your friends miserable lives ended. This is based on the original [ShowDeathCause](https://github.com/MOV-MB/ShowDeathCause) with some improvements.

## Features
The death notice lets everyone know what killed you and how. The end game report screen is improved now too! This fork provides the following improvements over the old version:
- Works with the latest game version as of the anniversary update
- Handles a few more cases
- Prints the name of the dead player along with damage taken
- Updates the end game report screen to include the enhanced information

| Chat Message (Before) | Chat Message (After) |
| ----- | ----- |
| ![Before](https://raw.githubusercontent.com/NotTsunami/ShowDeathCause/master/ExampleChatBefore.jpg) | ![After](https://raw.githubusercontent.com/NotTsunami/ShowDeathCause/master/ExampleChatAfter.jpg) |

| Game End Report (Before) | Game End Report (After) |
| ------ | ------ |
| ![Before](https://raw.githubusercontent.com/NotTsunami/ShowDeathCause/master/ExampleBefore.jpg) | ![After](https://raw.githubusercontent.com/NotTsunami/ShowDeathCause/master/ExampleAfter.jpg) |

_* The player is killed by the same type of monster, a Glacial Beetle, in both cases_

_** Screenshots are taken on ShowDeathCause 2.0.0 with the anniversary update_

## Installation
1. Install [BepInEx](https://thunderstore.io/package/bbepis/BepInExPack/) and [R2API](https://thunderstore.io/package/tristanmcpherson/R2API/).
2. Copy the included `ShowDeathCause.dll` into the resulting `C:\Program Files (x86)\Steam\steamapps\common\Risk of Rain 2\BepInEx\plugins` folder.
3. Launch the game and enjoy! To remove you simply need to delete the `ShowDeathCause.dll` file.

## Changelog
### Version 2.0.2
- Fixed NullReferenceException when the killer's body no longer exists in between the death message and end game screen
    - This was most noticeable with Jellyfish

### Version 2.0.1
- Reverted change to remove original death message as this may break other mods that depend on `OnPlayerCharacterDeath`

### Version 2.0.0 - Happy almost 100k downloads!
- The biggest change in this update is that the end game report screen now includes the same information as the death notice
- Elite/Umbra prefixes are now shown (Thanks WolfgangIsBestWolf for the suggestion!)
- Fall damage is now labeled as such (Can occur when max HP <= 1)
- Friendly fire kills are now attributed to the display name of the player who killed them
- The original red death message is no longer shown, only the messsage from ShowDeathCause is printed in chat now

### Version 1.0.5
- Switched to local stripped libs instead of relying on game's installation
    - No longer rely on a preexisting install of Risk of Rain 2
- Updated for anniversary update

### Version 1.0.4
- Updated for Risk of Rain 2 version 1.0

### Version 1.0.3
- Catch case where there are no attackers

### Version 1.0.2
- Artifacts update!
- Removed enemy skill and capped float value to 2 decimal points

## Credits to the Original Authors
[MOV-MB](https://github.com/MOV-MB)
[MagnusMagnuson](https://thunderstore.io/package/MagnusMagnuson/)

[Skull icon](https://icons8.com/icons/set/skull) by [Icons8](https://icons8.com).
