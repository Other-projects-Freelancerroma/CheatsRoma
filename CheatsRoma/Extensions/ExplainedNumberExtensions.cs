using TaleWorlds.CampaignSystem;
using TaleWorlds.Library;

namespace CheatsRoma.Extensions
{
    public static class ExplainedNumberExtensions
    {
        /// <summary>
        /// Добавляет процентное изменение к ExplainedNumber (аналог UFO).
        /// </summary>
        /// <param name="percentage">Положительное значение увеличивает, отрицательное уменьшает. 0 = без изменений.</param>
        public static void AddPercentage(this ref ExplainedNumber explainedNumber, float percentage)
        {
            float value = (1f - percentage / 100f) * -1f;
            explainedNumber.AddFactor(value);
        }
    }
}