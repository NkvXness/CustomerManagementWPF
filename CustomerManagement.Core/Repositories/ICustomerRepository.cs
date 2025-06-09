using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using CustomerManagement.Core.Models;

namespace CustomerManagement.Core.Repositories
{
    /// <summary>
    /// Интерфейс репозитория для работы с покупателями
    /// Реализует паттерн Repository для абстракции доступа к данным
    /// </summary>
    public interface ICustomerRepository
    {
        /// <summary>
        /// Добавить нового покупателя
        /// </summary>
        /// <param name="customer">Объект покупателя</param>
        void Add(Customer customer);

        /// <summary>
        /// Удалить покупателя по ID
        /// </summary>
        /// <param name="customerId">ID покупателя</param>
        /// <returns>True если удаление успешно</returns>
        bool Remove(string customerId);

        /// <summary>
        /// Получить покупателя по ID
        /// </summary>
        /// <param name="customerId">ID покупателя</param>
        /// <returns>Объект покупателя или null если не найден</returns>
        Customer? GetById(string customerId);

        /// <summary>
        /// Получить всех покупателей
        /// </summary>
        /// <returns>Список всех покупателей</returns>
        List<Customer> GetAll();

        /// <summary>
        /// Обновить данные покупателя
        /// </summary>
        /// <param name="customer">Обновленный объект покупателя</param>
        /// <returns>True если обновление успешно</returns>
        bool Update(Customer customer);

        /// <summary>
        /// Проверить существование покупателя по ID
        /// </summary>
        /// <param name="customerId">ID покупателя</param>
        /// <returns>True если покупатель существует</returns>
        bool Exists(string customerId);

        /// <summary>
        /// Получить количество покупателей
        /// </summary>
        /// <returns>Количество покупателей</returns>
        int Count();

        /// <summary>
        /// Получить покупателей по типу
        /// </summary>
        /// <param name="type">Тип покупателя</param>
        /// <returns>Список покупателей указанного типа</returns>
        List<Customer> GetByType(CustomerType type);

        /// <summary>
        /// Получить покупателей с активными договорами
        /// </summary>
        /// <returns>Список покупателей с активными договорами</returns>
        List<Customer> GetWithActiveContracts();

        /// <summary>
        /// Очистить все данные
        /// </summary>
        void Clear();
    }
}
