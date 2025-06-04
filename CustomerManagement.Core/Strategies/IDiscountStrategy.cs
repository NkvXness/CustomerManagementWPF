using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomerManagement.Core.Strategies
{
    /// <summary>
    /// Интерфейс для стратегии расчета скидок
    /// Паттерн Strategy позволяет динамически менять алгоритм расчета скидки
    /// </summary>
    public interface IDiscountStrategy
    {
        /// <summary>
        /// Рассчитать скидку на основе суммы покупки
        /// </summary>
        /// <param name="totalAmount">Общая сумма покупок</param>
        /// <returns>Сумма скидки в BYN</returns>
        decimal CalculateDiscount(decimal totalAmount);

        /// <summary>
        /// Получить процент скидки для данной суммы
        /// </summary>
        /// <param name="totalAmount">Общая сумма покупок</param>
        /// <returns>Процент скидки (от 0 до 100)</returns>
        decimal GetDiscountPercentage(decimal totalAmount);

        /// <summary>
        /// Название стратегии скидки
        /// </summary>
        string StrategyName { get; }
    }
}
