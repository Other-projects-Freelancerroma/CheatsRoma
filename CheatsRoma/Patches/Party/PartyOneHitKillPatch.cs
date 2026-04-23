using CheatsRoma.Cheats;
using CheatsRoma.Extensions;
using HarmonyLib;
using System;
using TaleWorlds.CampaignSystem;
using TaleWorlds.Core;
using TaleWorlds.MountAndBlade;
using TaleWorlds.MountAndBlade.ComponentInterfaces;

namespace CheatsRoma.Patches.Party
{
    [HarmonyPatch(typeof(AgentApplyDamageModel), "CalculateDamage")]
    public static class PartyOneHitKillPatch
    {
        [HarmonyPostfix]
        public static void CalculateDamage(
            AttackInformation attackInformation,
            AttackCollisionData collisionData,
            ref float __result)
        {
            try
            {
                if (!CheatConfig.PartyOneHitKillEnabled)
                    return;

                // Не обрабатываем игрока — для него есть отдельный OneHitKill
                if (attackInformation.AttackerAgent.IsPlayer())
                    return;

                // Убедимся, что атакующий — человек и НЕ герой
                if (!attackInformation.AttackerAgent.TryGetHuman(out CharacterObject character) || character.IsHero)
                    return;

                // Проверяем, что атакующий из партии игрока
                if (!attackInformation.AttackerAgent.Origin.TryGetParty(out var party) || !party.IsPlayerParty())
                    return;

                // Исключаем friendly fire (по желанию, как в UFO)
                if (attackInformation.IsFriendlyFire)
                    return;

                // Устанавливаем колоссальный урон
                __result = 10000f;
            }
            catch (Exception e)
            {
                SubModule.LogError(e, typeof(PartyOneHitKillPatch));
            }
        }
    }
}