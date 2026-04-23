using CheatsRoma.Cheats;
using HarmonyLib;
using System;
using TaleWorlds.Core;
using TaleWorlds.MountAndBlade;

namespace CheatsRoma.Patches.Player
{
    // Вспомогательный класс с логикой
    public static class CrushThroughLogic
    {
        public static bool ShouldCrushThrough(Agent attackerAgent)
        {
            // Только для игрока и только если чит включен
            return CheatConfig.CrushThroughEnabled && attackerAgent == Agent.Main;
        }
    }

    // Патч для Sandbox (кампания)
    [HarmonyPatch(typeof(SandBox.GameComponents.SandboxAgentApplyDamageModel), "DecideCrushedThrough")]
    public static class CrushThroughPatch_Sandbox
    {
        [HarmonyPostfix]
        public static void Postfix(
            ref bool __result,
            Agent attackerAgent,
            Agent defenderAgent,
            float totalAttackEnergy,
            Agent.UsageDirection attackDirection,
            StrikeType strikeType,
            WeaponComponentData defendItem,
            bool isPassiveUsage)
        {
            try
            {
                if (CrushThroughLogic.ShouldCrushThrough(attackerAgent))
                {
                    __result = true;
                }
            }
            catch (Exception e)
            {
                SubModule.LogError(e, typeof(CrushThroughPatch_Sandbox));
            }
        }
    }

    // Патч для CustomBattle (пользовательские битвы)
    [HarmonyPatch(typeof(CustomAgentApplyDamageModel), "DecideCrushedThrough")]
    public static class CrushThroughPatch_Custom
    {
        [HarmonyPostfix]
        public static void Postfix(
            ref bool __result,
            Agent attackerAgent,
            Agent defenderAgent,
            float totalAttackEnergy,
            Agent.UsageDirection attackDirection,
            StrikeType strikeType,
            WeaponComponentData defendItem,
            bool isPassiveUsage)
        {
            try
            {
                if (CrushThroughLogic.ShouldCrushThrough(attackerAgent))
                {
                    __result = true;
                }
            }
            catch (Exception e)
            {
                SubModule.LogError(e, typeof(CrushThroughPatch_Custom));
            }
        }
    }
}