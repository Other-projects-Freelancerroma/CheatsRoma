using CheatsRoma.Cheats;
using CheatsRoma.Extensions;
using HarmonyLib;
using SandBox.GameComponents;
using StoryMode.GameComponents;
using System;
using TaleWorlds.Core;
using TaleWorlds.MountAndBlade;

namespace CheatsRoma.Patches.Party
{
    internal static class CompanionsKnockoutOrKilledLogic
    {
        public static void Apply(Agent affectedAgent, DamageTypes damageType, float useSurgeryProbability, ref float __result)
        {
            try
            {
                if (CheatConfig.CompanionsKnockoutOrKilled == KnockoutOrKilled.Default)
                    return;

                // Только для компаньонов игрока (невключая самого игрока)
                if (!affectedAgent.IsPlayerCompanion())
                    return;

                if (CheatConfig.CompanionsKnockoutOrKilled == KnockoutOrKilled.Knockout)
                    __result = 0f;   // вероятность смерти 0
                else if (CheatConfig.CompanionsKnockoutOrKilled == KnockoutOrKilled.Killed)
                    __result = 1f;   // вероятность смерти 1
            }
            catch (Exception e)
            {
                SubModule.LogError(e, typeof(CompanionsKnockoutOrKilledPatch_Default));
            }
        }
    }

    [HarmonyPatch(typeof(DefaultAgentDecideKilledOrUnconsciousModel), "GetAgentStateProbability")]
    public static class CompanionsKnockoutOrKilledPatch_Default
    {
        [HarmonyPostfix]
        public static void Postfix(Agent affectorAgent, Agent effectedAgent, DamageTypes damageType, float useSurgeryProbability, ref float __result)
        {
            CompanionsKnockoutOrKilledLogic.Apply(effectedAgent, damageType, useSurgeryProbability, ref __result);
        }
    }

    [HarmonyPatch(typeof(SandboxAgentDecideKilledOrUnconsciousModel), "GetAgentStateProbability")]
    public static class CompanionsKnockoutOrKilledPatch_Sandbox
    {
        [HarmonyPostfix]
        public static void Postfix(Agent affectorAgent, Agent effectedAgent, DamageTypes damageType, float useSurgeryProbability, ref float __result)
        {
            CompanionsKnockoutOrKilledLogic.Apply(effectedAgent, damageType, useSurgeryProbability, ref __result);
        }
    }

    [HarmonyPatch(typeof(StoryModeAgentDecideKilledOrUnconsciousModel), "GetAgentStateProbability")]
    public static class CompanionsKnockoutOrKilledPatch_StoryMode
    {
        [HarmonyPostfix]
        public static void Postfix(Agent affectorAgent, Agent effectedAgent, DamageTypes damageType, float useSurgeryProbability, ref float __result)
        {
            CompanionsKnockoutOrKilledLogic.Apply(effectedAgent, damageType, useSurgeryProbability, ref __result);
        }
    }
}