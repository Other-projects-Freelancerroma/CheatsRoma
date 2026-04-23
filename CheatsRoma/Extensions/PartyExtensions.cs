using System;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.Party;

namespace CheatsRoma.Extensions
{
    public static class PartyExtensions
    {
        /// <summary>
        /// Проверяет, принадлежит ли PartyBase игроку.
        /// </summary>
        public static bool IsPlayerParty(this PartyBase party)
        {
            try
            {
                if (party == null)
                    return false;

                // Исключаем бандитские отряды без владельца
                if (party.MobileParty?.BanditPartyComponent != null && party.MobileParty.BanditPartyComponent.PartyOwner == null)
                    return false;

                // Караваны не считаются партией игрока в контексте боя
                if (party.MobileParty?.IsCaravan == true)
                    return false;

                return party.Owner?.IsHumanPlayerCharacter ?? false;
            }
            catch (NullReferenceException)
            {
                return false;
            }
        }

        /// <summary>
        /// Проверяет, принадлежит ли MobileParty игроку.
        /// </summary>
        public static bool IsPlayerParty(this MobileParty party)
        {
            return party?.Party?.IsPlayerParty() == true;
        }
    }
}