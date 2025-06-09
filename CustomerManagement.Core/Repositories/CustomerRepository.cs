using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using CustomerManagement.Core.Models;

namespace CustomerManagement.Core.Repositories
{
    /// <summary>
    /// Реализация репозитория для работы с покупателями
    /// Хранит данные в памяти (List)
    /// </summary>
    public class CustomerRepository : ICustomerRepository
    {
        // Приватное поле для хранения покупателей
        private readonly List<Customer> _customers;

        // Объект для потокобезопасности (если потребуется)
        private readonly object _lock = new object();

        /// <summary>
        /// Конструктор
        /// </summary>
        public CustomerRepository()
        {
            _customers = new List<Customer>();
        }

        /// <summary>
        /// Добавить нового покупателя
        /// </summary>
        /// <param name="customer">Объект покупателя</param>
        public void Add(Customer customer)
        {
            if (customer == null)
            {
                throw new ArgumentNullException(nameof(customer), "Покупатель не может быть null");
            }

            if (Exists(customer.CustomerId))
            {
                throw new InvalidOperationException($"Покупатель с ID {customer.CustomerId} уже существует");
            }

            lock (_lock)
            {
                _customers.Add(customer);
            }
        }

        /// <summary>
        /// Удалить покупателя по ID
        /// </summary>
        /// <param name="customerId">ID покупателя</param>
        /// <returns>True если удаление успешно</returns>
        public bool Remove(string customerId)
        {
            if (string.IsNullOrWhiteSpace(customerId))
            {
                return false;
            }

            lock (_lock)
            {
                var customer = _customers.FirstOrDefault(c => c.CustomerId == customerId);
                if (customer != null)
                {
                    return _customers.Remove(customer);
                }
            }

            return false;
        }

        /// <summary>
        /// Получить покупателя по ID
        /// </summary>
        /// <param name="customerId">ID покупателя</param>
        /// <returns>Объект покупателя или null если не найден</returns>
        public Customer? GetById(string customerId)
        {
            if (string.IsNullOrWhiteSpace(customerId))
            {
                return null;
            }

            lock (_lock)
            {
                return _customers.FirstOrDefault(c => c.CustomerId == customerId);
            }
        }

        /// <summary>
        /// Получить всех покупателей
        /// </summary>
        /// <returns>Список всех покупателей</returns>
        public List<Customer> GetAll()
        {
            lock (_lock)
            {
                // Возвращаем копию списка для безопасности
                return new List<Customer>(_customers);
            }
        }

        /// <summary>
        /// Обновить данные покупателя
        /// </summary>
        /// <param name="customer">Обновленный объект покупателя</param>
        /// <returns>True если обновление успешно</returns>
        public bool Update(Customer customer)
        {
            if (customer == null)
            {
                return false;
            }

            lock (_lock)
            {
                var existingCustomer = _customers.FirstOrDefault(c => c.CustomerId == customer.CustomerId);
                if (existingCustomer != null)
                {
                    // Удаляем старый объект
                    _customers.Remove(existingCustomer);
                    // Добавляем обновленный
                    _customers.Add(customer);
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Проверить существование покупателя по ID
        /// </summary>
        /// <param name="customerId">ID покупателя</param>
        /// <returns>True если покупатель существует</returns>
        public bool Exists(string customerId)
        {
            if (string.IsNullOrWhiteSpace(customerId))
            {
                return false;
            }

            lock (_lock)
            {
                return _customers.Any(c => c.CustomerId == customerId);
            }
        }

        /// <summary>
        /// Получить количество покупателей
        /// </summary>
        /// <returns>Количество покупателей</returns>
        public int Count()
        {
            lock (_lock)
            {
                return _customers.Count;
            }
        }

        /// <summary>
        /// Получить покупателей по типу
        /// </summary>
        /// <param name="type">Тип покупателя</param>
        /// <returns>Список покупателей указанного типа</returns>
        public List<Customer> GetByType(CustomerType type)
        {
            lock (_lock)
            {
                return _customers.Where(c => c.Type == type).ToList();
            }
        }

        /// <summary>
        /// Получить покупателей с активными договорами
        /// </summary>
        /// <returns>Список покупателей с активными договорами</returns>
        public List<Customer> GetWithActiveContracts()
        {
            lock (_lock)
            {
                return _customers
                    .Where(c => c.Contract != null && c.Contract.IsActive)
                    .ToList();
            }
        }

        /// <summary>
        /// Очистить все данные
        /// </summary>
        public void Clear()
        {
            lock (_lock)
            {
                _customers.Clear();
            }
        }
    }
}
