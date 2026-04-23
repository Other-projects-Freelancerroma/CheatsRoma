using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.AgentOrigins;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.Core;
using TaleWorlds.MountAndBlade;

namespace CheatsRoma.Extensions
{
    public static class AgentExtensions
    {
        /// <summary>
        /// Проверяет, является ли агент игроком.
        /// </summary>
        public static bool IsPlayer(this Agent agent)
        {
            return agent == Agent.Main;
        }

        /// <summary>
        /// Пытается получить CharacterObject для агента-человека.
        /// </summary>
        public static bool TryGetHuman(this Agent agent, out CharacterObject character)
        {
            character = agent?.Character as CharacterObject;
            return character != null;
        }

        /// <summary>
        /// Проверяет, является ли агент героем (не простым солдатом).
        /// </summary>
        public static bool IsHero(this Agent agent)
        {
            return agent?.IsHero ?? false;
        }

        /// <summary>
        /// Пытается получить PartyBase из IAgentOriginBase.
        /// Аналог метода TryGetParty из UFO.
        /// </summary>
        public static bool TryGetParty(this IAgentOriginBase origin, out PartyBase party)
        {
            party = null;
            if (origin == null) return false;

            if (origin is PartyAgentOrigin partyOrigin)
            {
                party = partyOrigin.Party;
                return party != null;
            }
            if (origin is PartyGroupAgentOrigin partyGroupOrigin)
            {
                party = partyGroupOrigin.Party;
                return party != null;
            }
            if (origin is SimpleAgentOrigin simpleOrigin)
            {
                party = simpleOrigin.Party;
                return party != null;
            }
            return false;
        }

        /// <summary>
        /// Проверяет, является ли агент компаньоном игрока.
        /// </summary>
        public static bool IsPlayerCompanion(this Agent agent)
        {
            return !agent.IsPlayer()
                && agent.IsHero()
                && agent.Origin.TryGetParty(out PartyBase party)
                && party.IsPlayerParty();
        }
    }
}