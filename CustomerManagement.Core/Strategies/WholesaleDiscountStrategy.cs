using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomerManagement.Core.Strategies
{
    /// <summary>
    /// Стратегия оптовых скидок для оптовых покупателей
    /// Скидка от 10% до 20% в зависимости от объема закупки
    /// </summary>
    public class WholesaleDiscountStrategy : IDiscountStrategy
    {
        /// <summary>
        /// Название стратегии
        /// </summary>
        public string StrategyName => "Оптовая скидка";

        /// <summary>
        /// Минимальная сумма заказа для оптового покупателя
        /// </summary>
        public decimal MinimumOrderAmount { get; set; }

        /// <summary>
        /// Конструктор по умолчанию
        /// Минимальный заказ: 10000 руб.
        /// </summary>
        public WholesaleDiscountStrategy()
        {
            MinimumOrderAmount = 10000m;
        }

        /// <summary>
        /// Конструктор с параметром
        /// </summary>
        /// <param name="minimumOrderAmount">Минимальная сумма заказа</param>
        public WholesaleDiscountStrategy(decimal minimumOrderAmount)
        {
            MinimumOrderAmount = minimumOrderAmount;
        }

        /// <summary>
        /// Рассчитать скидку на основе суммы покупки
        /// </summary>
        /// <param name="totalAmount">Общая сумма покупок</param>
        /// <returns>Сумма скидки в BYN</returns>
        public decimal CalculateDiscount(decimal totalAmount)
        {
            // Если сумма меньше минимальной, скидка не применяется
            if (totalAmount < MinimumOrderAmount)
                return 0m;

            decimal percentage = GetDiscountPercentage(totalAmount);
            return totalAmount * (percentage / 100);
        }

        /// <summary>
        /// Получить процент скидки для данной суммы
        /// Логика оптовой скидки:
        /// - Меньше минимума: 0% (не применяется)
        /// - 10000-30000 BYN: 10%
        /// - 30000-50000 BYN: 12%
        /// - 50000-100000 BYN: 15%
        /// - 100000-200000 BYN: 18%
        /// - Свыше 200000 BYN: 20%
        /// </summary>
        /// <param name="totalAmount">Общая сумма покупок</param>
        /// <returns>Процент скидки</returns>
        public decimal GetDiscountPercentage(decimal totalAmount)
        {
            if (totalAmount < MinimumOrderAmount)
                return 0m;
            else if (totalAmount < 30000)
                return 10m;
            else if (totalAmount < 50000)
                return 12m;
            else if (totalAmount < 100000)
                return 15m;
            else if (totalAmount < 200000)
                return 18m;
            else
                return 20m;
        }

        /// <summary>
        /// Проверить, соответствует ли сумма заказа минимальным требованиям
        /// </summary>
        /// <param name="orderAmount">Сумма заказа</param>
        /// <returns>True если заказ соответствует минимуму</returns>
        public bool ValidateMinimumOrder(decimal orderAmount)
        {
            return orderAmount >= MinimumOrderAmount;
        }
    }
}
