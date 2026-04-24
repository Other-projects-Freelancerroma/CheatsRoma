using CheatsRoma.Cheats;
using CheatsRoma.Extensions;
using HarmonyLib;
using System;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.GameComponents;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.Library;

namespace CheatsRoma.Patches.Map
{
    // Точная копия UFO-патча для потребления еды в процентах
    [HarmonyPatch(typeof(DefaultMobilePartyFoodConsumptionModel), "CalculateDailyFoodConsumptionf")]
    public static class FoodConsumptionPatch
    {
        [HarmonyPostfix]
        public static void Postfix(MobileParty party, ExplainedNumber baseConsumption, ref ExplainedNumber __result)
        {
            try
            {
                if (party == null || !party.IsPlayerParty())
                    return;

                // Применяем процент из настроек (0 = полное отключение)
                if (CheatConfig.FoodConsumptionPercentage >= 0f)
                {
                    __result.AddPercentage(CheatConfig.FoodConsumptionPercentage);
                }
            }
            catch (Exception e)
            {
                SubModule.LogError(e, typeof(FoodConsumptionPatch));
            }
        }
    }
}