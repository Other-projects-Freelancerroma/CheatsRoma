using CheatsRoma.Cheats;
using CheatsRoma.Extensions;
using HarmonyLib;
using SandBox.GameComponents;
using System;
using TaleWorlds.CampaignSystem;
using TaleWorlds.Core;
using TaleWorlds.MountAndBlade;
using StoryMode.GameComponents;

namespace CheatsRoma.Patches.Party
{
    // Общая логика для всех моделей
    internal static class PartyKnockoutOrKilledLogic
    {
        public static void Apply(Agent affectedAgent, DamageTypes damageType, float useSurgeryProbability, ref float __result)
        {
            try
            {
                if (CheatConfig.PartyKnockoutOrKilled == KnockoutOrKilled.Default)
                    return;

                // Только для агентов-людей, не игрок, не герой (т.е. простые солдаты)
                if (!affectedAgent.TryGetHuman(out CharacterObject character) ||
                    affectedAgent.IsPlayer() ||
                    character.IsHero)
                    return;

                // Проверяем, что агент из партии игрока
                if (!affectedAgent.Origin.TryGetParty(out var party) || !party.IsPlayerParty())
                    return;

                // Применяем выбранный режим
                if (CheatConfig.PartyKnockoutOrKilled == KnockoutOrKilled.Knockout)
                    __result = 0f;   // вероятность смерти 0
                else if (CheatConfig.PartyKnockoutOrKilled == KnockoutOrKilled.Killed)
                    __result = 1f;   // вероятность смерти 1
            }
            catch (Exception e)
            {
                SubModule.LogError(e, typeof(PartyKnockoutOrKilledPatch_Default));
            }
        }
    }

    // Патчи для трёх возможных моделей (аналогично UFO)

    [HarmonyPatch(typeof(DefaultAgentDecideKilledOrUnconsciousModel), "GetAgentStateProbability")]
    public static class PartyKnockoutOrKilledPatch_Default
    {
        [HarmonyPostfix]
        public static void Postfix(Agent affectorAgent, Agent effectedAgent, DamageTypes damageType, float useSurgeryProbability, ref float __result)
        {
            PartyKnockoutOrKilledLogic.Apply(effectedAgent, damageType, useSurgeryProbability, ref __result);
        }
    }

    [HarmonyPatch(typeof(SandboxAgentDecideKilledOrUnconsciousModel), "GetAgentStateProbability")]
    public static class PartyKnockoutOrKilledPatch_Sandbox
    {
        [HarmonyPostfix]
        public static void Postfix(Agent affectorAgent, Agent effectedAgent, DamageTypes damageType, float useSurgeryProbability, ref float __result)
        {
            PartyKnockoutOrKilledLogic.Apply(effectedAgent, damageType, useSurgeryProbability, ref __result);
        }
    }

    [HarmonyPatch(typeof(StoryModeAgentDecideKilledOrUnconsciousModel), "GetAgentStateProbability")]
    public static class PartyKnockoutOrKilledPatch_StoryMode
    {
        [HarmonyPostfix]
        public static void Postfix(Agent affectorAgent, Agent effectedAgent, DamageTypes damageType, float useSurgeryProbability, ref float __result)
        {
            PartyKnockoutOrKilledLogic.Apply(effectedAgent, damageType, useSurgeryProbability, ref __result);
        }
    }
}