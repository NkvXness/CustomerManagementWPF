using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using CustomerManagement.Core.Strategies;

namespace CustomerManagement.Core.Models
{
    /// <summary>
    /// Класс оптового покупателя с расширенными возможностями
    /// Наследуется от базового класса Customer
    /// </summary>
    public class WholesaleCustomer : Customer
    {
        #region Дополнительные свойства

        /// <summary>
        /// Минимальная сумма заказа для оптового покупателя
        /// По умолчанию: 10000 BYN
        /// </summary>
        public decimal MinimumOrderAmount { get; set; }

        /// <summary>
        /// Количество дней отсрочки платежа
        /// </summary>
        public int PaymentDeferralDays { get; set; }

        /// <summary>
        /// Процент оптовой скидки
        /// </summary>
        public decimal WholesaleDiscountPercent { get; private set; }

        #endregion

        #region Конструкторы

        /// <summary>
        /// Конструктор по умолчанию
        /// </summary>
        public WholesaleCustomer() : base()
        {
            MinimumOrderAmount = 10000m;
            PaymentDeferralDays = 0;
            WholesaleDiscountPercent = 10m;
            Type = CustomerType.Wholesale;

            // Устанавливаем стратегию оптовых скидок
            DiscountStrategy = new WholesaleDiscountStrategy(MinimumOrderAmount);
        }

        /// <summary>
        /// Конструктор с параметрами
        /// </summary>
        /// <param name="fullName">ФИО</param>
        /// <param name="email">Email</param>
        /// <param name="phone">Телефон</param>
        /// <param name="minimumOrderAmount">Минимальная сумма заказа</param>
        public WholesaleCustomer(string fullName, string email, string phone, decimal minimumOrderAmount = 10000m)
            : base(fullName, email, phone, CustomerType.Wholesale)
        {
            MinimumOrderAmount = minimumOrderAmount;
            PaymentDeferralDays = 0;
            WholesaleDiscountPercent = 10m;

            // Устанавливаем стратегию оптовых скидок
            DiscountStrategy = new WholesaleDiscountStrategy(MinimumOrderAmount);
        }

        #endregion

        #region Специализированные методы

        /// <summary>
        /// Установить минимальную сумму заказа
        /// </summary>
        /// <param name="amount">Новая минимальная сумма</param>
        public void SetMinimumOrderAmount(decimal amount)
        {
            if (amount < 0)
            {
                throw new ArgumentException("Минимальная сумма заказа не может быть отрицательной");
            }

            MinimumOrderAmount = amount;

            // Обновляем стратегию скидок с новым минимумом
            if (DiscountStrategy is WholesaleDiscountStrategy wholesaleStrategy)
            {
                wholesaleStrategy.MinimumOrderAmount = amount;
            }
        }

        /// <summary>
        /// Получить оптовую скидку для конкретной суммы заказа
        /// </summary>
        /// <param name="orderAmount">Сумма заказа</param>
        /// <returns>Сумма скидки в BYN</returns>
        public decimal GetWholesaleDiscount(decimal orderAmount)
        {
            if (DiscountStrategy == null)
                return 0m;

            return DiscountStrategy.CalculateDiscount(orderAmount);
        }

        /// <summary>
        /// Запросить отсрочку платежа
        /// </summary>
        /// <param name="days">Количество дней отсрочки</param>
        /// <returns>True если отсрочка одобрена</returns>
        public bool RequestPaymentDeferral(int days)
        {
            // Максимальная отсрочка - 30 дней
            if (days <= 0 || days > 30)
            {
                return false;
            }

            // Проверяем наличие активного договора
            if (Contract == null || !Contract.IsActive)
            {
                return false;
            }

            PaymentDeferralDays = days;
            return true;
        }

        /// <summary>
        /// Проверить, соответствует ли текущая сумма покупок минимальному заказу
        /// </summary>
        /// <returns>True если сумма соответствует минимуму</returns>
        public bool ValidateMinimumOrder()
        {
            return TotalAmount >= MinimumOrderAmount;
        }

        #endregion

        #region Переопределенные методы

        /// <summary>
        /// Переопределенный метод применения скидки
        /// Использует оптовую стратегию скидок
        /// </summary>
        /// <returns>Сумма скидки в BYN</returns>
        public override decimal ApplyDiscount()
        {
            decimal discount = base.ApplyDiscount();

            // Обновляем процент скидки для отображения
            if (DiscountStrategy != null)
            {
                WholesaleDiscountPercent = DiscountStrategy.GetDiscountPercentage(TotalAmount);
            }

            return discount;
        }

        /// <summary>
        /// Переопределенное строковое представление
        /// </summary>
        public override string ToString()
        {
            string deferralInfo = PaymentDeferralDays > 0
                ? $"Отсрочка: {PaymentDeferralDays} дн."
                : "Без отсрочки";

            return $"[ОПТОВЫЙ] {FullName} | {Email} | Мин.заказ: {MinimumOrderAmount:C} | {deferralInfo}";
        }

        #endregion
    }
}
