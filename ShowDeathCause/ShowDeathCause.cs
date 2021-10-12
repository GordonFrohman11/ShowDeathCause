using BepInEx;
using R2API.Utils;
using RoR2;

namespace ShowDeathCause
{
    [NetworkCompatibility(CompatibilityLevel.NoNeedForSync, VersionStrictness.DifferentModVersionsAreOk)]
    [BepInDependency("com.bepis.r2api")]
    [BepInPlugin("dev.tsunami.ShowDeathCause", "ShowDeathCause", "2.0.1")]
    public class ShowDeathCause : BaseUnityPlugin
    {
        private DamageReport _damageReport;

        public void Awake()
        {
            // This function handles printing the death message in chat
            On.RoR2.GlobalEventManager.OnPlayerCharacterDeath += (orig, self, damageReport, networkUser) =>
            {
                orig(self, damageReport, networkUser);

                if (!networkUser) return;

                _damageReport = damageReport;

                string token;
                if (damageReport.isFallDamage)
                {
                    // Fall damage is fatal when HP <=1
                    token = $"<color=#00FF80>{networkUser.userName}</color> died to fall damage.";
                }
                else if (damageReport.isFriendlyFire)
                {
                    // Friendly fire is possible through the Artifact of Chaos
                    token = damageReport.damageInfo.crit ? $"<color=#FF0000>CRITICAL HIT!</color> <color=#00FF80>{networkUser.userName}</color> killed by <color=#FF8000>{damageReport.attackerMaster.playerCharacterMasterController.networkUser.userName}</color> ({damageReport.damageInfo.damage:F2} damage taken)." : $"<color=#00FF80>{networkUser.userName}</color> killed by <color=#FF8000>{damageReport.attackerMaster.name}</color> ({damageReport.damageInfo.damage:F2} damage taken).";
                }
                else
                {
                    // Standard code path, GetBestBodyName replaces the need for a check against damageReport.attackerBody
                    token = $"<color=#00FF80>{networkUser.userName}</color> killed by <color=#FF8000>{Util.GetBestBodyName(damageReport.attackerBody.gameObject)}</color> ({damageReport.damageInfo.damage:F2} damage taken).";
                }
                
                Chat.SendBroadcastChat(new Chat.SimpleChatMessage {baseToken = token});
            };

            // This upgrades the game end panel to show damage numbers and be more concise
            On.RoR2.UI.GameEndReportPanelController.SetPlayerInfo += (orig, self, playerInfo) =>
            {
                orig(self, playerInfo);

                // Do nothing if the damage report is (somehow) unset
                if (_damageReport == null) return;

                // Override the string for killerBodyLabel ("Killed By: <killer>" on the end game panel)
                if (_damageReport.isFallDamage)
                {
                    self.killerBodyLabel.text = "<color=#FFFFFF>Killed By:</color> <color=#964B00>Fall Damage</color>";
                }
                else if (_damageReport.isFriendlyFire)
                {
                    self.killerBodyLabel.text = $"<color=#FFFFFF>Killed By:</color> <color=#FFFF80>{_damageReport.attackerMaster.playerCharacterMasterController.networkUser.userName}</color> <color=#FFFFFF>({_damageReport.damageInfo.damage:F2} damage)</color>";
                }
                else
                {
                    self.killerBodyLabel.text = $"<color=#FFFFFF>Killed By:</color> <color=#FFFF80>{Util.GetBestBodyName(_damageReport.attackerBody.gameObject)}</color> <color=#FFFFFF>({_damageReport.damageInfo.damage:F2} damage)</color>";
                }
            };
        }
    }
}
