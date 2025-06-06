using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomerManagement.Core.Strategies
{
    /// <summary>
    /// Премиальная стратегия скидок для VIP-покупателей
    /// Максимальные скидки от 25% до 30%
    /// </summary>
    public class VIPDiscountStrategy : IDiscountStrategy
    {
        /// <summary>
        /// Название стратегии
        /// </summary>
        public string StrategyName => "VIP скидка";

        /// <summary>
        /// Базовый процент VIP скидки (применяется всегда)
        /// </summary>
        public decimal BaseVIPDiscountPercentage { get; set; }

        /// <summary>
        /// Конструктор по умолчанию
        /// Базовая VIP скидка: 25%
        /// </summary>
        public VIPDiscountStrategy()
        {
            BaseVIPDiscountPercentage = 25m;
        }

        /// <summary>
        /// Конструктор с параметром
        /// </summary>
        /// <param name="baseDiscountPercentage">Базовый процент VIP скидки</param>
        public VIPDiscountStrategy(decimal baseDiscountPercentage)
        {
            // Ограничиваем значение в диапазоне 20-35%
            if (baseDiscountPercentage < 20m)
                BaseVIPDiscountPercentage = 20m;
            else if (baseDiscountPercentage > 35m)
                BaseVIPDiscountPercentage = 35m;
            else
                BaseVIPDiscountPercentage = baseDiscountPercentage;
        }

        /// <summary>
        /// Рассчитать скидку на основе суммы покупки
        /// </summary>
        /// <param name="totalAmount">Общая сумма покупок</param>
        /// <returns>Сумма скидки в BYN</returns>
        public decimal CalculateDiscount(decimal totalAmount)
        {
            decimal percentage = GetDiscountPercentage(totalAmount);
            return totalAmount * (percentage / 100);
        }

        /// <summary>
        /// Получить процент скидки для данной суммы
        /// Логика VIP скидки:
        /// - До 5000 BYN: 25% (базовая VIP скидка)
        /// - 5000-20000 BYN: 26%
        /// - 20000-50000 BYN: 27%
        /// - 50000-100000 BYN: 28%
        /// - 100000-200000 BYN: 29%
        /// - Свыше 200000 BYN: 30%
        /// </summary>
        /// <param name="totalAmount">Общая сумма покупок</param>
        /// <returns>Процент скидки</returns>
        public decimal GetDiscountPercentage(decimal totalAmount)
        {
            // VIP клиенты всегда получают минимум базовую скидку
            if (totalAmount < 5000)
                return BaseVIPDiscountPercentage;
            else if (totalAmount < 20000)
                return 26m;
            else if (totalAmount < 50000)
                return 27m;
            else if (totalAmount < 100000)
                return 28m;
            else if (totalAmount < 200000)
                return 29m;
            else
                return 30m;
        }

        /// <summary>
        /// Получить дополнительную скидку сверх базовой
        /// </summary>
        /// <param name="totalAmount">Сумма покупки</param>
        /// <returns>Дополнительный процент скидки</returns>
        public decimal GetBonusDiscountPercentage(decimal totalAmount)
        {
            decimal totalDiscount = GetDiscountPercentage(totalAmount);
            return totalDiscount - BaseVIPDiscountPercentage;
        }
    }
}
