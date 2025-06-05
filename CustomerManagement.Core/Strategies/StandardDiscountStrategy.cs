using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomerManagement.Core.Strategies
{
    /// <summary>
    /// Стандартная стратегия скидок для обычных покупателей
    /// Скидка зависит от суммы покупки: 0-5%
    /// </summary>
    public class StandardDiscountStrategy : IDiscountStrategy
    {
        /// <summary>
        /// Название стратегии
        /// </summary>
        public string StrategyName => "Стандартная скидка";

        /// <summary>
        /// Рассчитать скидку на основе суммы покупки
        /// </summary>
        /// <param name="totalAmount">Общая сумма покупок</param>
        /// <returns>Сумма скидки в рублях</returns>
        public decimal CalculateDiscount(decimal totalAmount)
        {
            decimal percentage = GetDiscountPercentage(totalAmount);
            return totalAmount * (percentage / 100);
        }

        /// <summary>
        /// Получить процент скидки для данной суммы
        /// Логика скидки:
        /// - До 1000 BYN: 0% (без скидки)
        /// - 1000-5000 BYN: 2%
        /// - 5000-10000 BYN: 3%
        /// - 10000-20000 BYN: 4%
        /// - Свыше 20000 BYN: 5%
        /// </summary>
        /// <param name="totalAmount">Общая сумма покупок</param>
        /// <returns>Процент скидки</returns>
        public decimal GetDiscountPercentage(decimal totalAmount)
        {
            if (totalAmount < 1000)
                return 0m;
            else if (totalAmount < 5000)
                return 2m;
            else if (totalAmount < 10000)
                return 3m;
            else if (totalAmount < 20000)
                return 4m;
            else
                return 5m;
        }
    }
}
