using BepInEx;
using R2API.Utils;
using RoR2;

namespace ShowDeathCause
{
    [NetworkCompatibility(CompatibilityLevel.NoNeedForSync, VersionStrictness.DifferentModVersionsAreOk)]
    [BepInDependency("com.bepis.r2api")]
    [BepInPlugin("dev.tsunami.ShowDeathCause", "ShowDeathCause", "2.0.2")]
    public class ShowDeathCause : BaseUnityPlugin
    {
        // These members are added to avoid trying to later access a GameObject that doesn't exist
        private static DamageReport _damageReport;
        private static string _friendlyAttacker;
        private static string _attacker;

        public void Awake()
        {
            // Subscribe to the pre-existing event, we were being a bad boy and hooking onto the GlobalEventManager before
            GlobalEventManager.onCharacterDeathGlobal += (damageReport) =>
            {
                if (damageReport == null) return;
            
                string victimUserName = damageReport.victimMaster.playerCharacterMasterController.networkUser.userName;
                string reportToken;
                _damageReport = damageReport;
            
                if (damageReport.isFallDamage)
                {
                    // Fall damage is fatal when HP <=1 or when Artifact of Frailty is active
                    reportToken = $"<color=#00FF80>{victimUserName}</color> died to fall damage.";
                }
                else if (damageReport.isFriendlyFire)
                {
                    // Friendly fire is possible through the Artifact of Chaos or other mods
                    // Compatibility with other mods is untested, but shouldn't break
                    _friendlyAttacker = damageReport.attackerMaster.playerCharacterMasterController.networkUser
                        .userName;
                    reportToken = damageReport.damageInfo.crit 
                        ? $"<color=#FF0000>CRITICAL HIT!</color> <color=#00FF80>{victimUserName}</color> killed by <color=#FF8000>{_friendlyAttacker}</color> ({damageReport.damageInfo.damage:F2} damage taken)."
                        : $"<color=#00FF80>{victimUserName}</color> killed by <color=#FF8000>{_friendlyAttacker}</color> ({damageReport.damageInfo.damage:F2} damage taken).";
                }
                else
                {
                    // Standard code path, GetBestBodyName replaces the need for a check against damageReport.attackerBody
                    _attacker = Util.GetBestBodyName(damageReport.attackerBody.gameObject);
                    reportToken = $"<color=#00FF80>{victimUserName}</color> killed by <color=#FF8000>{_attacker}</color> ({damageReport.damageInfo.damage:F2} damage taken).";
                }
                
                Chat.SendBroadcastChat(new Chat.SimpleChatMessage {baseToken = reportToken});
            };

            // This upgrades the game end panel to show damage numbers and be more concise
            On.RoR2.UI.GameEndReportPanelController.SetPlayerInfo += (orig, self, playerInfo) =>
            {
                orig(self, playerInfo);

                // Do nothing if the damage report is unset
                if (_damageReport == null) return;

                // Override the string for killerBodyLabel ("Killed By: <killer>" on the end game panel)
                string labelToken;
                if (_damageReport.isFallDamage)
                {
                    labelToken = "<color=#FFFFFF>Killed By:</color> <color=#964B00>Fall Damage</color>";
                }
                else if (_damageReport.isFriendlyFire)
                {
                    labelToken = $"<color=#FFFFFF>Killed By:</color> <color=#FFFF80>{_friendlyAttacker}</color> <color=#FFFFFF>({_damageReport.damageInfo.damage:F2} damage)</color>";
                }
                else
                {
                    labelToken = $"<color=#FFFFFF>Killed By:</color> <color=#FFFF80>{_attacker}</color> <color=#FFFFFF>({_damageReport.damageInfo.damage:F2} damage)</color>";
                }

                self.killerBodyLabel.text = labelToken;
            };
        }
    }
}
