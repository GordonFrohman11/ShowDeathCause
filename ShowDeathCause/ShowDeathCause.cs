using BepInEx;
using RoR2;

namespace ShowDeathCause
{
    [BepInDependency("com.bepis.r2api")]
    [BepInPlugin("dev.tsunami.ShowDeathCause", "ShowDeathCause", "1.0.2")]
    public class ShowDeathCause : BaseUnityPlugin
    {
        public void Awake()
        {
            On.RoR2.GlobalEventManager.OnPlayerCharacterDeath += (orig, self, damageReport, networkUser) =>
            {
                orig(self, damageReport, networkUser);
                
                if (!networkUser) return;

                Chat.SendBroadcastChat(new Chat.SimpleChatMessage
                {
                    baseToken = $"<color=#00FF80>{networkUser.userName}</color> killed by <color=#FF8000>{damageReport.attackerBody.GetDisplayName()}</color> ({damageReport.damageInfo.damage:F2} damage taken)."
                });
            };
        }
    }
}
