using HarmonyLib;
using System;
using TaleWorlds.Core;
using TaleWorlds.Library;
using TaleWorlds.MountAndBlade;
using CheatsRoma.Cheats;

namespace CheatsRoma.Patches.Player
{
    [HarmonyPatch(typeof(Mission), "OnAgentShootMissile")]
    public static class InfiniteAmmoPatch
    {
        [HarmonyPostfix]
        public static void OnAgentShootMissile(
            Agent shooterAgent,
            EquipmentIndex weaponIndex,
            Vec3 position,
            Vec3 velocity,
            Mat3 orientation,
            bool hasRigidBody,
            bool isPrimaryWeaponShot,
            int forcedMissileIndex)
        {
            try
            {
                // Проверяем, включён ли чит и стреляет ли игрок
                if (!CheatConfig.InfiniteAmmoEnabled || shooterAgent != Agent.Main)
                    return;

                // Восстанавливаем все расходуемые предметы до максимума
                for (EquipmentIndex eqIndex = EquipmentIndex.WeaponItemBeginSlot;
                     eqIndex < EquipmentIndex.NumAllWeaponSlots;
                     eqIndex++)
                {
                    MissionWeapon weapon = shooterAgent.Equipment[eqIndex];
                    if (weapon.IsAnyConsumable() && weapon.Amount < weapon.ModifiedMaxAmount)
                    {
                        shooterAgent.SetWeaponAmountInSlot(eqIndex, weapon.ModifiedMaxAmount, true);
                    }
                }
            }
            catch (Exception e)
            {
                SubModule.LogError(e, typeof(InfiniteAmmoPatch));
            }
        }
    }
}