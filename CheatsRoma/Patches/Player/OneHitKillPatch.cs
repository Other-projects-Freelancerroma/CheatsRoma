using CheatsRoma.Cheats;
using HarmonyLib;
using System;
using TaleWorlds.MountAndBlade;
using TaleWorlds.MountAndBlade.ComponentInterfaces;

namespace CheatsRoma.Patches.Player
{
    [HarmonyPatch(typeof(AgentApplyDamageModel), "CalculateDamage")]
    public static class OneHitKillPatch
    {
        [HarmonyPostfix]
        public static void CalculateDamage(
            AttackInformation attackInformation,
            AttackCollisionData collisionData,
            ref float __result)
        {
            try
            {
                // Проверяем, включён ли чит
                if (!CheatConfig.OneHitKillEnabled)
                    return;

                // Атакующий должен быть главным героем, и это не урон по своим
                if (attackInformation.AttackerAgent == Agent.Main && !attackInformation.IsFriendlyFire)
                {
                    __result = 10000f;
                }
            }
            catch (Exception e)
            {
                SubModule.LogError(e, typeof(OneHitKillPatch));
            }
        }
    }
}