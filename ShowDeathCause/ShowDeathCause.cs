using BepInEx;
using RoR2;
using System.Collections.Generic;
using Zio;
using Zio.FileSystems;

namespace ShowDeathCause
{
    [BepInPlugin("dev.tsunami.ShowDeathCause", "ShowDeathCause", "2.0.2")]
    public class ShowDeathCause : BaseUnityPlugin
    {
        // These members are added to avoid trying to later access a GameObject that doesn't exist
        private static DamageReport _damageReport;
        private static string _damageTaken;
        private static string _attacker;

        // FileSystem for ZIO
        public static FileSystem FileSystem { get; private set; }

        public void Awake()
        {
            // This adds in support for multiple languages
            // R2API offers LanguageAPI but we want to remain compatible with vanilla, thus use ZIO
            PhysicalFileSystem physicalFileSystem = new PhysicalFileSystem();
            var assemblyDir = System.IO.Path.GetDirectoryName(Info.Location);
            FileSystem = new SubFileSystem(physicalFileSystem, physicalFileSystem.ConvertPathFromInternal(assemblyDir));

            if (FileSystem.DirectoryExists("/Language/"))
            {
                Language.collectLanguageRootFolders += delegate (List<DirectoryEntry> list)
                {
                    list.Add(FileSystem.GetDirectoryEntry("/Language/"));
                };
            }

            // Subscribe to the pre-existing event, we were being a bad boy and hooking onto the GlobalEventManager before
            GlobalEventManager.onCharacterDeathGlobal += (damageReport) =>
            {
                // This should never happen, but protect against it just in case
                if (damageReport == null) return;

                NetworkUser networkUser = damageReport.attackerMaster.playerCharacterMasterController.networkUser;
                if (!networkUser) return;

                _damageReport = damageReport;
                _damageTaken = $"{damageReport.damageInfo.damage:F2}";

                string reportToken;
                if (damageReport.isFallDamage)
                {
                    // Fall damage is fatal when HP <=1 or when Artifact of Frailty is active
                    reportToken = "SDC_PLAYER_DEATH_FALL_DAMAGE";
                }
                else if (damageReport.isFriendlyFire)
                {
                    // Friendly fire is possible through the Artifact of Chaos or other mods
                    _attacker = networkUser.userName;
                    reportToken = damageReport.damageInfo.crit ? $"SDC_PLAYER_DEATH_FRIENDLY_CRIT" : $"SDC_PLAYER_DEATH_FRIENDLY";
                }
                else
                {
                    // Standard code path, GetBestBodyName replaces the need for a check against damageReport.attackerBody
                    _attacker = Util.GetBestBodyName(damageReport.attackerBody.gameObject);
                    reportToken = "SDC_PLAYER_DEATH";
                }
                
                Chat.SendBroadcastChat(new Chat.SimpleChatMessage {
                    baseToken = reportToken,
                    paramTokens = new[] { networkUser.userName, _attacker, _damageTaken }
                });
            };

            // This upgrades the game end panel to show damage numbers and be more concise
            On.RoR2.UI.GameEndReportPanelController.SetPlayerInfo += (orig, self, playerInfo) =>
            {
                orig(self, playerInfo);

                // This should never happen, but leave original text in the case the report is null
                if (_damageReport == null) return;

                // Override the string for killerBodyLabel ("Killed By: <killer>" on the end game report panel)
                string labelToken;
                if (_damageReport.isFallDamage)
                {
                    labelToken = "FALL_DAMAGE_PREFIX_DEATH";
                }
                else if (_damageReport.isFriendlyFire)
                {
                    labelToken = $"GENERIC_PREFIX_DEATH";
                }
                else
                {
                    labelToken = $"GENERIC_PREFIX_DEATH";
                }

                self.killerBodyLabel.text = Language.GetStringFormatted(labelToken, _attacker, _damageTaken);
            };
        }
    }
}
