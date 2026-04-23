using CheatsRoma.Cheats;
using CheatsRoma.Extensions;
using HarmonyLib;
using System;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.GameComponents;
using TaleWorlds.CampaignSystem.MapEvents;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.CampaignSystem.Siege;
using TaleWorlds.Core;
using TaleWorlds.Library;

namespace CheatsRoma.Patches.Combat
{
    // Патч 1: прорыв в осаждённое поселение
    [HarmonyPatch(typeof(DefaultTroopSacrificeModel), "GetLostTroopCountForBreakingInBesiegedSettlement")]
    public static class NoTroopSacrificeBreakInPatch
    {
        [HarmonyPostfix]
        public static void Postfix(MobileParty party, SiegeEvent siegeEvent, ref ExplainedNumber __result)
        {
            try
            {
                if (CheatConfig.NoTroopSacrificeEnabled && party.IsPlayerParty())
                    __result = new ExplainedNumber(0);
            }
            catch (Exception e)
            {
                SubModule.LogError(e, typeof(NoTroopSacrificeBreakInPatch));
            }
        }
    }

    // Патч 2: выход из осаждённого поселения
    [HarmonyPatch(typeof(DefaultTroopSacrificeModel), "GetLostTroopCountForBreakingOutOfBesiegedSettlement")]
    public static class NoTroopSacrificeBreakOutPatch
    {
        [HarmonyPostfix]
        public static void Postfix(MobileParty party, SiegeEvent siegeEvent, ref ExplainedNumber __result)
        {
            try
            {
                if (CheatConfig.NoTroopSacrificeEnabled && party.IsPlayerParty())
                    __result = new ExplainedNumber(0);
            }
            catch (Exception e)
            {
                SubModule.LogError(e, typeof(NoTroopSacrificeBreakOutPatch));
            }
        }
    }

    // Патч 3: попытка бегства
    [HarmonyPatch(typeof(DefaultTroopSacrificeModel), nameof(DefaultTroopSacrificeModel.GetNumberOfTroopsSacrificedForTryingToGetAway))]
    [HarmonyPatch(new Type[] { typeof(BattleSideEnum), typeof(MapEvent) })]
    public static class NoTroopSacrificeRunawayPatch
    {
        [HarmonyPostfix]
        public static void Postfix(BattleSideEnum playerBattleSide, MapEvent mapEvent, ref int __result)
        {
            try
            {
                if (CheatConfig.NoTroopSacrificeEnabled &&
                    mapEvent.IsPlayerMapEvent &&
                    playerBattleSide == mapEvent.PlayerSide)
                {
                    __result = 0;
                }
            }
            catch (Exception e)
            {
                SubModule.LogError(e, typeof(NoTroopSacrificeRunawayPatch));
            }
        }
    }
}