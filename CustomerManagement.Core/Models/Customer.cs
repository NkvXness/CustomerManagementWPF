using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using CustomerManagement.Core.Strategies;

namespace CustomerManagement.Core.Models
{
    /// <summary>
    /// Базовый класс для всех типов покупателей
    /// Содержит общую функциональность для работы с покупателями
    /// </summary>
    public class Customer
    {
        #region Свойства

        /// <summary>
        /// Уникальный идентификатор покупателя
        /// </summary>
        public string CustomerId { get; set; }

        /// <summary>
        /// Полное имя покупателя (ФИО)
        /// </summary>
        public string FullName { get; set; }

        /// <summary>
        /// Адрес электронной почты
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// Контактный телефон
        /// </summary>
        public string Phone { get; set; }

        /// <summary>
        /// Почтовый адрес
        /// </summary>
        public string Address { get; set; }

        /// <summary>
        /// Договор с покупателем
        /// </summary>
        public Contract? Contract { get; set; }

        /// <summary>
        /// Список всех покупок покупателя
        /// </summary>
        public List<Purchase> Purchases { get; set; }

        /// <summary>
        /// Общая сумма всех покупок (вычисляемое свойство)
        /// </summary>
        public decimal TotalAmount
        {
            get
            {
                return Purchases.Sum(p => p.TotalPrice);
            }
        }

        /// <summary>
        /// Стратегия расчета скидки для данного покупателя
        /// </summary>
        public IDiscountStrategy? DiscountStrategy { get; set; }

        /// <summary>
        /// Тип покупателя
        /// </summary>
        public CustomerType Type { get; set; }

        #endregion

        #region Конструкторы

        /// <summary>
        /// Конструктор по умолчанию
        /// </summary>
        public Customer()
        {
            CustomerId = Guid.NewGuid().ToString();
            FullName = string.Empty;
            Email = string.Empty;
            Phone = string.Empty;
            Address = string.Empty;
            Contract = null;
            Purchases = new List<Purchase>();
            DiscountStrategy = null;
            Type = CustomerType.Regular;
        }

        /// <summary>
        /// Конструктор с параметрами
        /// </summary>
        public Customer(string fullName, string email, string phone, CustomerType type = CustomerType.Regular)
        {
            CustomerId = Guid.NewGuid().ToString();
            FullName = fullName;
            Email = email;
            Phone = phone;
            Address = string.Empty;
            Contract = null;
            Purchases = new List<Purchase>();
            DiscountStrategy = null;
            Type = type;
        }

        #endregion

        #region Методы работы с договором

        /// <summary>
        /// Оформить договор с покупателем
        /// </summary>
        /// <param name="contractNumber">Номер договора</param>
        /// <param name="signDate">Дата подписания</param>
        public void SignContract(string contractNumber, DateTime signDate)
        {
            if (string.IsNullOrWhiteSpace(contractNumber))
            {
                throw new ArgumentException("Номер договора не может быть пустым", nameof(contractNumber));
            }

            if (Contract != null && Contract.IsActive)
            {
                throw new InvalidOperationException("У покупателя уже есть активный договор");
            }

            Contract = new Contract(contractNumber, signDate);
        }

        /// <summary>
        /// Расторгнуть договор
        /// </summary>
        public void TerminateContract()
        {
            if (Contract == null)
            {
                throw new InvalidOperationException("Договор не был оформлен");
            }

            if (!Contract.IsActive)
            {
                throw new InvalidOperationException("Договор уже расторгнут");
            }

            Contract.Terminate();
        }

        /// <summary>
        /// Возобновить расторгнутый договор
        /// </summary>
        /// <param name="newSignDate">Новая дата подписания</param>
        public void RenewContract(DateTime newSignDate)
        {
            if (Contract == null)
            {
                throw new InvalidOperationException("Договор не был оформлен");
            }

            if (Contract.IsActive)
            {
                throw new InvalidOperationException("Договор уже активен");
            }

            Contract.Renew(newSignDate);
        }

        #endregion

        #region Методы работы с персональными данными

        /// <summary>
        /// Получить персональные данные покупателя
        /// </summary>
        /// <returns>Объект с персональными данными</returns>
        public CustomerData GetPersonalData()
        {
            return new CustomerData(FullName, Email, Phone, Address);
        }

        /// <summary>
        /// Обновить персональные данные покупателя
        /// </summary>
        /// <param name="data">Новые данные</param>
        public void UpdatePersonalData(CustomerData data)
        {
            if (data == null)
            {
                throw new ArgumentNullException(nameof(data), "Данные не могут быть null");
            }

            if (!data.IsValid())
            {
                throw new ArgumentException("Переданы некорректные данные покупателя");
            }

            FullName = data.FullName;
            Email = data.Email;
            Phone = data.Phone;
            Address = data.Address;
        }

        #endregion

        #region Методы работы с покупками

        /// <summary>
        /// Добавить покупку
        /// </summary>
        /// <param name="purchase">Объект покупки</param>
        public void AddPurchase(Purchase purchase)
        {
            if (purchase == null)
            {
                throw new ArgumentNullException(nameof(purchase), "Покупка не может быть null");
            }

            if (!purchase.IsValid())
            {
                throw new ArgumentException("Покупка содержит некорректные данные");
            }

            if (Contract == null || !Contract.IsActive)
            {
                throw new InvalidOperationException("Невозможно добавить покупку без активного договора");
            }

            Purchases.Add(purchase);
        }

        /// <summary>
        /// Получить все покупки покупателя
        /// </summary>
        /// <returns>Список покупок</returns>
        public List<Purchase> GetPurchases()
        {
            return new List<Purchase>(Purchases); // Возвращаем копию списка
        }

        /// <summary>
        /// Очистить все покупки
        /// </summary>
        public void ClearPurchases()
        {
            Purchases.Clear();
        }

        #endregion

        #region Методы работы со скидками и оплатой

        /// <summary>
        /// Применить скидку к общей сумме покупок
        /// Виртуальный метод - может быть переопределен в наследниках
        /// </summary>
        /// <returns>Сумма скидки в BYN</returns>
        public virtual decimal ApplyDiscount()
        {
            if (DiscountStrategy == null)
            {
                return 0m; // Нет скидки
            }

            return DiscountStrategy.CalculateDiscount(TotalAmount);
        }

        /// <summary>
        /// Получить итоговую сумму с учетом скидки
        /// </summary>
        /// <returns>Сумма к оплате</returns>
        public decimal GetTotalWithDiscount()
        {
            decimal discount = ApplyDiscount();
            return TotalAmount - discount;
        }

        /// <summary>
        /// Обработать оплату
        /// </summary>
        /// <param name="amount">Сумма оплаты</param>
        /// <returns>Результат операции оплаты</returns>
        public PaymentResult ProcessPayment(decimal amount)
        {
            if (amount <= 0)
            {
                return PaymentResult.Failure("Сумма оплаты должна быть больше нуля");
            }

            if (Contract == null || !Contract.IsActive)
            {
                return PaymentResult.Failure("Невозможно принять оплату без активного договора");
            }

            decimal totalWithDiscount = GetTotalWithDiscount();

            if (amount > totalWithDiscount)
            {
                return PaymentResult.Failure($"Сумма оплаты ({amount:C}) превышает сумму к оплате ({totalWithDiscount:C})");
            }

            decimal remaining = totalWithDiscount - amount;
            return PaymentResult.Success(amount, remaining);
        }

        #endregion

        #region Переопределенные методы

        /// <summary>
        /// Получить строковое представление покупателя
        /// </summary>
        public override string ToString()
        {
            string contractStatus = Contract?.IsActive == true ? "Договор активен" : "Нет договора";
            return $"[{Type}] {FullName} | {Email} | Покупок: {Purchases.Count} | {contractStatus}";
        }

        #endregion
    }
}