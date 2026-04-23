using HarmonyLib;
using TaleWorlds.MountAndBlade;
using CheatsRoma.Cheats;
using CheatsRoma.Extensions; // <-- Вот эта строка всё исправляет

namespace CheatsRoma.Patches.Player
{
    [HarmonyPatch(typeof(Agent), "CurrentMortalityState", MethodType.Getter)]
    public static class GodModePatch
    {
        [HarmonyPostfix]
        public static void SetInvulnerable(Agent __instance, ref Agent.MortalityState __result)
        {
            if (!CheatConfig.GodModeEnabled)
                return;

            if (__instance.IsPlayer())
            {
                __result = Agent.MortalityState.Invulnerable;
                return;
            }

            if (__instance.IsMount && __instance.RiderAgent?.IsPlayer() == true)
            {
                __result = Agent.MortalityState.Invulnerable;
            }
        }
    }
}