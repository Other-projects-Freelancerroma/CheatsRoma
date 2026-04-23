using CheatsRoma.Cheats;
using CheatsRoma.Extensions;
using HarmonyLib;
using System;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.GameComponents;
using TaleWorlds.CampaignSystem.MapEvents;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.Core;
using TaleWorlds.Library;

namespace CheatsRoma.Patches.Combat
{
    [HarmonyPatch(typeof(DefaultCombatSimulationModel), "SimulateHit")]
    [HarmonyPatch(new Type[] {
        typeof(CharacterObject),
        typeof(CharacterObject),
        typeof(PartyBase),
        typeof(PartyBase),
        typeof(float),
        typeof(MapEvent),
        typeof(float),
        typeof(float)
    })]
    public static class AlwaysWinBattleSimulationPatch
    {
        [HarmonyPostfix]
        public static void SimulateHit(
            CharacterObject strikerTroop,
            CharacterObject struckTroop,
            PartyBase strikerParty,
            PartyBase struckParty,
            float strikerAdvantage,
            MapEvent battle,
            float strikerSideMorale,
            float struckSideMorale,
            ref ExplainedNumber __result)
        {
            try
            {
                if (!CheatConfig.AlwaysWinBattleSimulationEnabled)
                    return;

                // Если атакуют партию игрока — обнуляем урон
                if (struckParty.IsPlayerParty())
                {
                    __result = new ExplainedNumber(0f);
                }
            }
            catch (Exception e)
            {
                SubModule.LogError(e, typeof(AlwaysWinBattleSimulationPatch));
            }
        }
    }
}