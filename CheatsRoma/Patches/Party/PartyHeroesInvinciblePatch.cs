using HarmonyLib;
using System;
using TaleWorlds.MountAndBlade;
using CheatsRoma.Cheats;
using CheatsRoma.Extensions;

namespace CheatsRoma.Patches.Party
{
    [HarmonyPatch(typeof(Agent), "CurrentMortalityState", MethodType.Getter)]
    public static class PartyHeroesInvinciblePatch
    {
        [HarmonyPostfix]
        public static void SetInvulnerable(Agent __instance, ref Agent.MortalityState __result)
        {
            try
            {
                if (!CheatConfig.PartyHeroesInvincibleEnabled)
                    return;

                // Используем готовый метод расширения
                if (__instance.IsPlayerCompanion())
                {
                    __result = Agent.MortalityState.Invulnerable;
                }
            }
            catch (Exception e)
            {
                SubModule.LogError(e, typeof(PartyHeroesInvinciblePatch));
            }
        }
    }
}