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
        // These strings are added to avoid trying to access a GameObject that doesn't exist
        private DamageReport _damageReport;
        private string _friendlyAttacker;
        private string _attacker;

        public void Awake()
        {
            // This function handles printing the death message in chat
            On.RoR2.GlobalEventManager.OnPlayerCharacterDeath += (orig, self, damageReport, networkUser) =>
            {
                // I wanted to remove this initially, but this would cause any mod added before ShowDeathCause
                // in the execution cycle that relied on OnPlayerCharacterDeath to not fire.
                orig(self, damageReport, networkUser);

                if (!networkUser) return;

                _damageReport = damageReport;

                string token;
                if (damageReport.isFallDamage)
                {
                    // Fall damage is fatal when HP <=1 or when Artifact of Frailty is active
                    token = $"<color=#00FF80>{networkUser.userName}</color> died to fall damage.";
                }
                else if (damageReport.isFriendlyFire)
                {
                    // Friendly fire is possible through the Artifact of Chaos or other mods
                    // Compatibility with other mods is untested, but shouldn't break
                    _friendlyAttacker = damageReport.attackerMaster.playerCharacterMasterController.networkUser
                        .userName;
                    token = damageReport.damageInfo.crit ? $"<color=#FF0000>CRITICAL HIT!</color> <color=#00FF80>{networkUser.userName}</color> killed by <color=#FF8000>{_friendlyAttacker}</color> ({damageReport.damageInfo.damage:F2} damage taken)." : $"<color=#00FF80>{networkUser.userName}</color> killed by <color=#FF8000>{_friendlyAttacker}</color> ({damageReport.damageInfo.damage:F2} damage taken).";
                }
                else
                {
                    // Standard code path, GetBestBodyName replaces the need for a check against damageReport.attackerBody
                    _attacker = Util.GetBestBodyName(damageReport.attackerBody.gameObject);
                    token = $"<color=#00FF80>{networkUser.userName}</color> killed by <color=#FF8000>{_attacker}</color> ({damageReport.damageInfo.damage:F2} damage taken).";
                }
                
                Chat.SendBroadcastChat(new Chat.SimpleChatMessage {baseToken = token});
            };

            // This upgrades the game end panel to show damage numbers and be more concise
            On.RoR2.UI.GameEndReportPanelController.SetPlayerInfo += (orig, self, playerInfo) =>
            {
                orig(self, playerInfo);

                // Do nothing if the damage report is unset
                if (_damageReport == null) return;

                // Override the string for killerBodyLabel ("Killed By: <killer>" on the end game panel)
                if (_damageReport.isFallDamage)
                {
                    self.killerBodyLabel.text = "<color=#FFFFFF>Killed By:</color> <color=#964B00>Fall Damage</color>";
                }
                else if (_damageReport.isFriendlyFire)
                {
                    self.killerBodyLabel.text = $"<color=#FFFFFF>Killed By:</color> <color=#FFFF80>{_friendlyAttacker}</color> <color=#FFFFFF>({_damageReport.damageInfo.damage:F2} damage)</color>";
                }
                else
                {
                    self.killerBodyLabel.text = $"<color=#FFFFFF>Killed By:</color> <color=#FFFF80>{_attacker}</color> <color=#FFFFFF>({_damageReport.damageInfo.damage:F2} damage)</color>";
                }
            };
        }
    }
}
