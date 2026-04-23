using HarmonyLib;
using System;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.Core;
using TaleWorlds.MountAndBlade;
using CheatsRoma.Cheats;
using CheatsRoma.Extensions;

namespace CheatsRoma.Patches.Party
{
    [HarmonyPatch(typeof(Agent), "CurrentMortalityState", MethodType.Getter)]
    public static class PartyInvinciblePatch
    {
        [HarmonyPostfix]
        public static void SetInvulnerable(Agent __instance, ref Agent.MortalityState __result)
        {
            try
            {
                if (!CheatConfig.PartyInvincibleEnabled)
                    return;

                // Агент должен быть человеком
                if (!__instance.TryGetHuman(out CharacterObject character))
                    return;

                // Пропускаем героев (компаньонов, членов семьи) — для них есть отдельный чит
                if (character.IsHero)
                    return;

                // Получаем партию через Agent.Origin (работает благодаря нашему методу расширения TryGetParty)
                if (!__instance.Origin.TryGetParty(out PartyBase party) || !party.IsPlayerParty())
                    return;

                // Делаем обычного солдата бессмертным
                __result = Agent.MortalityState.Invulnerable;
            }
            catch (Exception e)
            {
                SubModule.LogError(e, typeof(PartyInvinciblePatch));
            }
        }
    }
}