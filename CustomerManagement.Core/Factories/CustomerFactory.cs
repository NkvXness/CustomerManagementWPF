using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using CustomerManagement.Core.Models;
using CustomerManagement.Core.Strategies;

namespace CustomerManagement.Core.Factories
{
    /// <summary>
    /// Фабрика для создания объектов покупателей
    /// Реализует паттерн Factory Method
    /// </summary>
    public class CustomerFactory
    {
        /// <summary>
        /// Создать покупателя указанного типа с минимальными данными
        /// </summary>
        /// <param name="type">Тип покупателя</param>
        /// <returns>Объект покупателя</returns>
        public static Customer CreateCustomer(CustomerType type)
        {
            return type switch
            {
                CustomerType.Regular => new Customer(),
                CustomerType.Wholesale => new WholesaleCustomer(),
                CustomerType.VIP => new VIPCustomer(),
                _ => throw new ArgumentException($"Неизвестный тип покупателя: {type}")
            };
        }

        /// <summary>
        /// Создать покупателя с полными данными
        /// </summary>
        /// <param name="type">Тип покупателя</param>
        /// <param name="fullName">ФИО</param>
        /// <param name="email">Email</param>
        /// <param name="phone">Телефон</param>
        /// <returns>Объект покупателя с заполненными данными</returns>
        public static Customer CreateCustomer(CustomerType type, string fullName, string email, string phone)
        {
            Customer customer = type switch
            {
                CustomerType.Regular => new Customer(fullName, email, phone),
                CustomerType.Wholesale => new WholesaleCustomer(fullName, email, phone),
                CustomerType.VIP => new VIPCustomer(fullName, email, phone),
                _ => throw new ArgumentException($"Неизвестный тип покупателя: {type}")
            };

            // Назначаем соответствующую стратегию скидок
            customer.DiscountStrategy = CreateDiscountStrategy(type);

            return customer;
        }

        /// <summary>
        /// Создать покупателя с полными данными и дополнительными параметрами
        /// </summary>
        /// <param name="type">Тип покупателя</param>
        /// <param name="fullName">ФИО</param>
        /// <param name="email">Email</param>
        /// <param name="phone">Телефон</param>
        /// <param name="address">Адрес</param>
        /// <returns>Объект покупателя</returns>
        public static Customer CreateCustomer(CustomerType type, string fullName, string email, string phone, string address)
        {
            Customer customer = CreateCustomer(type, fullName, email, phone);
            customer.Address = address;
            return customer;
        }

        /// <summary>
        /// Создать оптового покупателя с настройкой минимальной суммы заказа
        /// </summary>
        /// <param name="fullName">ФИО</param>
        /// <param name="email">Email</param>
        /// <param name="phone">Телефон</param>
        /// <param name="minimumOrderAmount">Минимальная сумма заказа</param>
        /// <returns>Объект оптового покупателя</returns>
        public static WholesaleCustomer CreateWholesaleCustomer(string fullName, string email, string phone, decimal minimumOrderAmount)
        {
            var customer = new WholesaleCustomer(fullName, email, phone, minimumOrderAmount);
            customer.DiscountStrategy = new WholesaleDiscountStrategy(minimumOrderAmount);
            return customer;
        }

        /// <summary>
        /// Создать VIP-покупателя с назначением персонального менеджера
        /// </summary>
        /// <param name="fullName">ФИО</param>
        /// <param name="email">Email</param>
        /// <param name="phone">Телефон</param>
        /// <param name="personalManager">Имя персонального менеджера</param>
        /// <returns>Объект VIP-покупателя</returns>
        public static VIPCustomer CreateVIPCustomer(string fullName, string email, string phone, string personalManager)
        {
            var customer = new VIPCustomer(fullName, email, phone, personalManager);
            customer.DiscountStrategy = new VIPDiscountStrategy();
            return customer;
        }

        /// <summary>
        /// Создать стратегию скидок для указанного типа покупателя
        /// </summary>
        /// <param name="type">Тип покупателя</param>
        /// <returns>Объект стратегии скидок</returns>
        public static IDiscountStrategy CreateDiscountStrategy(CustomerType type)
        {
            return type switch
            {
                CustomerType.Regular => new StandardDiscountStrategy(),
                CustomerType.Wholesale => new WholesaleDiscountStrategy(),
                CustomerType.VIP => new VIPDiscountStrategy(),
                _ => throw new ArgumentException($"Неизвестный тип покупателя: {type}")
            };
        }

        /// <summary>
        /// Проверить корректность данных для создания покупателя
        /// </summary>
        /// <param name="fullName">ФИО</param>
        /// <param name="email">Email</param>
        /// <param name="phone">Телефон</param>
        /// <returns>True если данные корректны</returns>
        public static bool ValidateCustomerData(string fullName, string email, string phone)
        {
            return !string.IsNullOrWhiteSpace(fullName)
                   && !string.IsNullOrWhiteSpace(email)
                   && !string.IsNullOrWhiteSpace(phone)
                   && email.Contains("@"); // Простая валидация email
        }

        /// <summary>
        /// Создать тестового покупателя для демонстрации
        /// </summary>
        /// <param name="type">Тип покупателя</param>
        /// <returns>Покупатель с тестовыми данными</returns>
        public static Customer CreateTestCustomer(CustomerType type)
        {
            return type switch
            {
                CustomerType.Regular => CreateCustomer(
                    CustomerType.Regular,
                    "Иванов Иван Иванович",
                    "ivanov@example.com",
                    "+375 (44) 123-45-67",
                    "Могилёв, ул. Ленина, д. 1"
                ),
                CustomerType.Wholesale => CreateWholesaleCustomer(
                    "ООО \"Оптторг\"",
                    "opttorg@example.com",
                    "+375 (29) 987-65-43",
                    15000m
                ),
                CustomerType.VIP => CreateVIPCustomer(
                    "Петров Петр Петрович",
                    "petrov@example.com",
                    "+7 (999) 555-55-55",
                    "Сидоров Сергей"
                ),
                _ => throw new ArgumentException($"Неизвестный тип покупателя: {type}")
            };
        }
    }
}
