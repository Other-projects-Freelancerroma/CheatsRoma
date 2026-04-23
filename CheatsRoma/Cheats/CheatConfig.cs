using static CheatsRoma.Cheats.KnockoutOrKilled;


namespace CheatsRoma.Cheats
{
    // Статический класс для хранения и управления состоянием читов.
    // Все читы по умолчанию включены (true) для удобства тестирования.
    public static class CheatConfig
    {
        // Бессмертие игрока и его лошади
        public static bool GodModeEnabled { get; set; } = true;

        // Бесконечные боеприпасы (стрелы, болты, метательное)
        public static bool InfiniteAmmoEnabled { get; set; } = true;

        // Убийство с одного удара (включая ломание щитов)
        public static bool OneHitKillEnabled { get; set; } = true;

        // Пробивание блока (игнорирует блок щитом или оружием)
        public static bool CrushThroughEnabled { get; set; } = true;

        // ГГ не умирает от старости
        public static bool NeverDieOfOldAgeEnabled { get; set; } = true;

        // Неуязвимость героев в отряде игрока (компаньоны, члены семьи)
        public static bool PartyHeroesInvincibleEnabled { get; set; } = true;

        // Бесконечные боеприпасы для всех членов отряда игрока
        public static bool PartyInfiniteAmmoEnabled { get; set; } = true;

        // Бессмертие для обычных солдат (не героев) в отряде игрока
        public static bool PartyInvincibleEnabled { get; set; } = false;

        // Солдаты в отряде игрока убивают с одного удара
        public static bool PartyOneHitKillEnabled { get; set; } = false;

        // Режим для обычных солдат: убивать или только нокаутировать
        public static KnockoutOrKilled PartyKnockoutOrKilled { get; set; } = Knockout;

        // Режим для компаньонов (героев в отряде игрока, кроме самого игрока)
        public static KnockoutOrKilled CompanionsKnockoutOrKilled { get; set; } = Knockout;

        // Всегда побеждать в симуляции боя (когда бой проходит без личного участия)
        public static bool AlwaysWinBattleSimulationEnabled { get; set; } = true;
    }
}