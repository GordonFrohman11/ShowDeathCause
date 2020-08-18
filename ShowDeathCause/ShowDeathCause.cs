using BepInEx;
using R2API.Utils;
using RoR2;

namespace ShowDeathCause
{
    [NetworkCompatibility(CompatibilityLevel.NoNeedForSync, VersionStrictness.DifferentModVersionsAreOk)]
    [BepInDependency("com.bepis.r2api")]
    [BepInPlugin("dev.tsunami.ShowDeathCause", "ShowDeathCause", "1.0.4")]
    public class ShowDeathCause : BaseUnityPlugin
    {
        public void Awake()
        {
            On.RoR2.GlobalEventManager.OnPlayerCharacterDeath += (orig, self, damageReport, networkUser) =>
            {
                orig(self, damageReport, networkUser);
                
                if (!networkUser) return;

                string token = damageReport.attackerBody ? $"<color=#00FF80>{networkUser.userName}</color> killed by <color=#FF8000>{damageReport.attackerBody.GetDisplayName()}</color> ({damageReport.damageInfo.damage:F2} damage taken)." : $"<color=#00FF80>{networkUser.userName}</color> was killed by unknown causes ({damageReport.damageInfo.damage:F2} damage taken)!";

                Chat.SendBroadcastChat(new Chat.SimpleChatMessage {baseToken = token});
            };
        }
    }
}
