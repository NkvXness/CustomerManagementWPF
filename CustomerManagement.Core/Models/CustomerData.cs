using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomerManagement.Core.Models
{
    /// <summary>
    /// DTO для передачи персональных данных покупателя
    /// </summary>
    public class CustomerData
    {
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
        /// Конструктор по умолчанию
        /// </summary>
        public CustomerData()
        {
            FullName = string.Empty;
            Email = string.Empty;
            Phone = string.Empty;
            Address = string.Empty;
        }

        /// <summary>
        /// Конструктор с параметрами
        /// </summary>
        public CustomerData(string fullName, string email, string phone, string address)
        {
            FullName = fullName;
            Email = email;
            Phone = phone;
            Address = address;
        }

        /// <summary>
        /// Создать копию объекта
        /// </summary>
        /// <returns>Новый объект с теми же данными</returns>
        public CustomerData Clone()
        {
            return new CustomerData(FullName, Email, Phone, Address);
        }

        /// <summary>
        /// Проверить, что все обязательные поля заполнены
        /// </summary>
        public bool IsValid()
        {
            return !string.IsNullOrWhiteSpace(FullName)
                   && !string.IsNullOrWhiteSpace(Email)
                   && !string.IsNullOrWhiteSpace(Phone);
        }

        /// <summary>
        /// Получить строковое представление данных
        /// </summary>
        public override string ToString()
        {
            return $"{FullName} | {Email} | {Phone}";
        }
    }
}
