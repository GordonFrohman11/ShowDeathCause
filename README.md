# ShowDeathCause
This mod lets you know how your and your friends miserable lives ended. This is based on the original [ShowDeathCause](https://github.com/MOV-MB/ShowDeathCause) with several improvements and updated for the latest game version.

## Features
The death notice lets everyone know what killed you and how. The end game report screen is improved too! This fork provides the following improvements over the old version:
- Works with the latest game version as of the first DLC update
- Handles several more scenarios (friendly fire, critical hits, fall damage, void damage)
- Prints the name of the dead player along with damage taken
- Updates the end game report screen to include the enhanced information
- **(NEW)** Language support (Currently only English is supported, but pull requests are always welcome! See the guide below for contributing.)

| Chat Message (Before) | Chat Message (After) |
| ----- | ----- |
| ![Before](https://raw.githubusercontent.com/NotTsunami/ShowDeathCause/master/Images/ExampleChatBefore.jpg) | ![After](https://raw.githubusercontent.com/NotTsunami/ShowDeathCause/master/Images/ExampleChatAfter.jpg) |

| Game End Report (Before) | Game End Report (After) |
| ------ | ------ |
| ![Before](https://raw.githubusercontent.com/NotTsunami/ShowDeathCause/master/Images/ExampleBefore.jpg) | ![After](https://raw.githubusercontent.com/NotTsunami/ShowDeathCause/master/Images/ExampleAfter.jpg) |

_* Screenshots are taken on ShowDeathCause 2.0.0_

## Installation
It is highly recommended to use [r2modman](https://thunderstore.io/package/ebkr/r2modman/) to install ShowDeathCause because it will set up everything for you! If you are installing manually, you will need to make a folder in `Risk of Rain 2\BepInEx\plugins` called `ShowDeathCause` and drop the contents of the zip into it.

## Changelog
### Version 3.0.0
Version 3.0.0 is a MAJOR release, with a lot of changes under the hood. A very large shoutout goes to [DestroyedClone](https://github.com/DestroyedClone) for the support of additional languages along with the newly added death messages, and [Bubbet](https://github.com/Bubbet) for helping me learn Harmony. I recommend showing your support for these two by checking out their mods!
- R2API dependency has been removed, making ShowDeathCause vanilla-compatible!
- Language support has been added
- New death messages have been added for the DLC update (void fog and void instant kill abilities)
- Under the hood changes, mostly some future-proofing and hooking correctly

### Version 2.0.2
- Fixed NullReferenceException when the killer's body no longer exists in between the death message and end game screen
    - This happened most commonly when being killed by a Jellyfish

### Version 2.0.1
- Reverted change to remove original death message as this may break other mods that depend on `OnPlayerCharacterDeath`

### Version 2.0.0 - Happy almost 100k downloads!
- The biggest change in this update is that the end game report screen now includes the same information as the death notice
- Elite/Umbra prefixes are now shown (Thanks WolfgangIsBestWolf for the suggestion!)
- Fall damage is now labeled as such (Can occur when max HP <= 1 or with artifact)
- Friendly fire kills are now attributed to the display name of the player who killed them
- The original red death message is no longer shown, only the messsage from ShowDeathCause is printed in chat now

### Version 1.0.5
- Switched to local stripped libs instead of relying on game's installation
- Updated for anniversary update

### Version 1.0.4
- Updated for Risk of Rain 2 version 1.0

### Version 1.0.3
- Catch case where there are no attackers

### Version 1.0.2
- Artifacts update!
- Removed enemy skill and capped float value to 2 decimal points

## Contributing a Translation

Thank you for your interest in contributing a translation! You can contribute a translation by following the steps below:

1. Locate this code block in `ShowDeathCause.cs` (I have omitted several of the `list.Add()` calls as there are a lot of tokens, but the [...] block refers to all of the tokens):
```csharp
Language.onCurrentLanguageChanged += () =>
{
    var list = new List<KeyValuePair<string, string>>();
    if (Language.currentLanguageName == "en")
    {
            list.Add(new KeyValuePair<string, string>("SDC_KILLER_FALLDAMAGE", "<color=#964B00>Fall Damage</color>"));
            [...]
            list.Add(new KeyValuePair<string, string>("SDC_PLAYER_DEATH_VOID", "<color=#621e7d>JAILED!</color> <color=#00FF80>{0}</color> killed by <color=#FF8000>{1}</color>."));
    }
    Language.currentLanguage.SetStringsByTokens(list);
}
```
2. You will want to add another branch to the if statement that checks against your language's [IETF language tag](https://en.wikipedia.org/wiki/IETF_language_tag). The code should now look like the following if I were translating for French:
```csharp
Language.onCurrentLanguageChanged += () =>
{
    var list = new List<KeyValuePair<string, string>>();
    if (Language.currentLanguageName == "en")
    {
            list.Add(new KeyValuePair<string, string>("SDC_KILLER_FALLDAMAGE", "<color=#964B00>Fall Damage</color>"));
            [...]
            list.Add(new KeyValuePair<string, string>("SDC_PLAYER_DEATH_VOID", "<color=#621e7d>JAILED!</color> <color=#00FF80>{0}</color> killed by <color=#FF8000>{1}</color>."));
    }
    else if (Language.currentLanguageName == "fr")
    {
    
    }
    Language.currentLanguage.SetStringsByTokens(list);
}
```
3. Copy all of the `list.add();` calls from the `en` section to your branch, and then translate them as per the suggestion below. The final code should look like:
```csharp
Language.onCurrentLanguageChanged += () =>
{
    var list = new List<KeyValuePair<string, string>>();
    if (Language.currentLanguageName == "en")
    {
            list.Add(new KeyValuePair<string, string>("SDC_KILLER_FALLDAMAGE", "<color=#964B00>Fall Damage</color>"));
            [...]
            list.Add(new KeyValuePair<string, string>("SDC_PLAYER_DEATH_VOID", "<color=#621e7d>JAILED!</color> <color=#00FF80>{0}</color> killed by <color=#FF8000>{1}</color>."));
    }
    else if (Language.currentLanguageName == "fr")
    {
		    list.Add(new KeyValuePair<string, string>("SDC_KILLER_FALLDAMAGE", "<color=#964B00>Fall Damage</color>"));
            [...]
            list.Add(new KeyValuePair<string, string>("SDC_PLAYER_DEATH_VOID", "<color=#621e7d>JAILED!</color> <color=#00FF80>{0}</color> killed by <color=#FF8000>{1}</color>."));
    }
    Language.currentLanguage.SetStringsByTokens(list);
}
```
4. Send in a pull request through GitHub!

Anything in brackets ({}) or wrapped in <> does not need to be translated. For example, if you were translating the `SDC_PLAYER_DEATH` token, which reads `<color=#00FF80>{0}</color> killed by <color=#FF8000>{1}</color> ({2} damage taken).` as of version 3.0.0, you would only translate "killed by" and "damage taken".

## Credits
### Original Authors
- [MOV-MB](https://github.com/MOV-MB)
- [MagnusMagnuson](https://thunderstore.io/package/MagnusMagnuson/)

### Other Credits
As noted in the changelog, the 3.0.0 release would not have been possible without the help of the following:
- [DestroyedClone](https://github.com/DestroyedClone) (https://github.com/NotTsunami/ShowDeathCause/pull/4) 
- [Bubbet](https://github.com/Bubbet)
- Everyone in the Risk of Rain 2 Modding Discord that has helped and supported me!
- [Skull icon](https://icons8.com/icons/set/skull) by [Icons8](https://icons8.com)
