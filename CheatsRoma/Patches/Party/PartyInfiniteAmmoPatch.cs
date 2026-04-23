using HarmonyLib;
using System;
using TaleWorlds.Core;
using TaleWorlds.Library;
using TaleWorlds.MountAndBlade;
using CheatsRoma.Cheats;
using CheatsRoma.Extensions;

namespace CheatsRoma.Patches.Party
{
    [HarmonyPatch(typeof(Mission), "OnAgentShootMissile")]
    public static class PartyInfiniteAmmoPatch
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
                // Чит выключен – выходим
                if (!CheatConfig.PartyInfiniteAmmoEnabled)
                    return;

                // Если стреляет сам игрок – пропускаем (для игрока есть отдельный чит InfiniteAmmo)
                if (shooterAgent.IsPlayer())
                    return;

                // Проверяем, что стрелок из партии игрока
                if (!shooterAgent.Origin.TryGetParty(out var party) || !party.IsPlayerParty())
                    return;

                // Восстанавливаем боеприпасы во всех слотах расходуемого оружия
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
                SubModule.LogError(e, typeof(PartyInfiniteAmmoPatch));
            }
        }
    }
}