using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using CustomerManagement.Core.Strategies;

namespace CustomerManagement.Core.Models
{
    /// <summary>
    /// Класс VIP-покупателя с премиальным обслуживанием
    /// Наследуется от базового класса Customer
    /// </summary>
    public class VIPCustomer : Customer
    {
        #region Дополнительные свойства

        /// <summary>
        /// Процент VIP скидки (25-30%)
        /// </summary>
        public decimal VIPDiscountPercent { get; private set; }

        /// <summary>
        /// Накопленные бонусные баллы
        /// </summary>
        public decimal BonusPoints { get; private set; }

        /// <summary>
        /// Имя персонального менеджера
        /// </summary>
        public string PersonalManager { get; private set; }

        /// <summary>
        /// Процент начисления бонусов от суммы покупки
        /// По умолчанию: 5%
        /// </summary>
        public decimal BonusAccrualRate { get; set; }

        #endregion

        #region Конструкторы

        /// <summary>
        /// Конструктор по умолчанию
        /// </summary>
        public VIPCustomer() : base()
        {
            VIPDiscountPercent = 25m;
            BonusPoints = 0m;
            PersonalManager = "Не назначен";
            BonusAccrualRate = 5m;
            Type = CustomerType.VIP;

            // Устанавливаем стратегию VIP скидок
            DiscountStrategy = new VIPDiscountStrategy();
        }

        /// <summary>
        /// Конструктор с параметрами
        /// </summary>
        /// <param name="fullName">ФИО</param>
        /// <param name="email">Email</param>
        /// <param name="phone">Телефон</param>
        /// <param name="personalManager">Имя персонального менеджера</param>
        public VIPCustomer(string fullName, string email, string phone, string personalManager = "Не назначен")
            : base(fullName, email, phone, CustomerType.VIP)
        {
            VIPDiscountPercent = 25m;
            BonusPoints = 0m;
            PersonalManager = personalManager;
            BonusAccrualRate = 5m;

            // Устанавливаем стратегию VIP скидок
            DiscountStrategy = new VIPDiscountStrategy();
        }

        #endregion

        #region Специализированные методы

        /// <summary>
        /// Получить размер VIP скидки для текущей суммы покупок
        /// </summary>
        /// <returns>Сумма скидки в BYN</returns>
        public decimal GetVIPDiscount()
        {
            if (DiscountStrategy == null)
                return 0m;

            return DiscountStrategy.CalculateDiscount(TotalAmount);
        }

        /// <summary>
        /// Начислить бонусные баллы от суммы покупки
        /// </summary>
        /// <param name="purchaseAmount">Сумма покупки</param>
        public void AddBonusPoints(decimal purchaseAmount)
        {
            if (purchaseAmount <= 0)
            {
                throw new ArgumentException("Сумма покупки должна быть больше нуля");
            }

            decimal bonusToAdd = purchaseAmount * (BonusAccrualRate / 100);
            BonusPoints += bonusToAdd;
        }

        /// <summary>
        /// Использовать бонусные баллы для оплаты
        /// </summary>
        /// <param name="amount">Сумма для списания</param>
        /// <returns>True если списание успешно</returns>
        public bool UseBonusPoints(decimal amount)
        {
            if (amount <= 0)
            {
                return false;
            }

            if (amount > BonusPoints)
            {
                return false; // Недостаточно бонусов
            }

            BonusPoints -= amount;
            return true;
        }

        /// <summary>
        /// Получить баланс бонусных баллов
        /// </summary>
        /// <returns>Количество бонусных баллов</returns>
        public decimal GetBonusBalance()
        {
            return BonusPoints;
        }

        /// <summary>
        /// Назначить персонального менеджера
        /// </summary>
        /// <param name="managerName">Имя менеджера</param>
        public void AssignPersonalManager(string managerName)
        {
            if (string.IsNullOrWhiteSpace(managerName))
            {
                throw new ArgumentException("Имя менеджера не может быть пустым");
            }

            PersonalManager = managerName;
        }

        /// <summary>
        /// Проверить возможность использования бонусов
        /// </summary>
        /// <param name="amount">Сумма для списания</param>
        /// <returns>True если хватает бонусов</returns>
        public bool CanUseBonusPoints(decimal amount)
        {
            return amount > 0 && amount <= BonusPoints;
        }

        #endregion

        #region Переопределенные методы

        /// <summary>
        /// Переопределенный метод применения скидки
        /// Использует VIP стратегию скидок
        /// </summary>
        /// <returns>Сумма скидки в BYN</returns>
        public override decimal ApplyDiscount()
        {
            decimal discount = base.ApplyDiscount();

            // Обновляем процент VIP скидки для отображения
            if (DiscountStrategy != null)
            {
                VIPDiscountPercent = DiscountStrategy.GetDiscountPercentage(TotalAmount);
            }

            return discount;
        }

        /// <summary>
        /// Переопределенное добавление покупки с начислением бонусов
        /// </summary>
        /// <param name="purchase">Покупка</param>
        public new void AddPurchase(Purchase purchase)
        {
            // Вызываем базовый метод
            base.AddPurchase(purchase);

            // Начисляем бонусы за покупку
            AddBonusPoints(purchase.TotalPrice);
        }

        /// <summary>
        /// Переопределенное строковое представление
        /// </summary>
        public override string ToString()
        {
            return $"[VIP] {FullName} | {Email} | Менеджер: {PersonalManager} | Бонусы: {BonusPoints:F2}";
        }

        #endregion
    }
}
