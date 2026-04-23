using HarmonyLib;
using System;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.Actions;
using CheatsRoma.Cheats;
using CheatsRoma.Extensions; // если понадобится IsPlayer, но здесь сравниваем с Hero.MainHero

namespace CheatsRoma.Patches.Player
{
    // Патч предотвращает смерть главного героя от старости
    [HarmonyPatch(typeof(KillCharacterAction), nameof(KillCharacterAction.ApplyByOldAge))]
    public static class NeverDieOfOldAgePatch
    {
        [HarmonyPrefix]
        public static bool Prefix(Hero victim, ref bool showNotification)
        {
            try
            {
                // Если чит включен и умирающий — главный герой игрока
                if (CheatConfig.NeverDieOfOldAgeEnabled && victim == Hero.MainHero)
                {
                    return false; // Блокируем выполнение оригинального метода — смерть не происходит
                }
                return true; // Иначе пусть всё идёт своим чередом
            }
            catch (Exception e)
            {
                SubModule.LogError(e, typeof(NeverDieOfOldAgePatch));
                return true; // В случае ошибки лучше не ломать игру
            }
        }
    }
}