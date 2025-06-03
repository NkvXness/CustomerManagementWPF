using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomerManagement.Core.Models
{
    /// <summary>
    /// Перечисление типов покупателей
    /// </summary>
    public enum CustomerType
    {
        /// <summary>
        /// Обычный покупатель (стандартная скидка 0-5%)
        /// </summary>
        Regular = 0,

        /// <summary>
        /// Оптовый покупатель (скидка 10-20%, минимальный заказ от 10000 BYN.)
        /// </summary>
        Wholesale = 1,

        /// <summary>
        /// VIP покупатель (скидка 25-30%, бонусная программа)
        /// </summary>
        VIP = 2
    }
}
