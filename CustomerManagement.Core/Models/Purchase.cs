using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomerManagement.Core.Models
{
    /// <summary>
    /// Представляет покупку товара
    /// </summary>
    public class Purchase
    {
        /// <summary>
        /// Уникальный идентификатор покупки
        /// </summary>
        public string PurchaseId { get; set; }

        /// <summary>
        /// Название товара
        /// </summary>
        public string ProductName { get; set; }

        /// <summary>
        /// Количество единиц товара
        /// </summary>
        public int Quantity { get; set; }

        /// <summary>
        /// Цена за единицу товара
        /// </summary>
        public decimal Price { get; set; }

        /// <summary>
        /// Дата совершения покупки
        /// </summary>
        public DateTime PurchaseDate { get; set; }

        /// <summary>
        /// Общая стоимость покупки (вычисляемое свойство)
        /// </summary>
        public decimal TotalPrice
        {
            get { return Quantity * Price; }
        }

        /// <summary>
        /// Конструктор по умолчанию
        /// </summary>
        public Purchase()
        {
            PurchaseId = Guid.NewGuid().ToString();
            ProductName = string.Empty;
            Quantity = 0;
            Price = 0m;
            PurchaseDate = DateTime.Now;
        }

        /// <summary>
        /// Конструктор с параметрами
        /// </summary>
        /// <param name="productName">Название товара</param>
        /// <param name="quantity">Количество</param>
        /// <param name="price">Цена за единицу</param>
        public Purchase(string productName, int quantity, decimal price)
        {
            PurchaseId = Guid.NewGuid().ToString();
            ProductName = productName;
            Quantity = quantity;
            Price = price;
            PurchaseDate = DateTime.Now;
        }

        /// <summary>
        /// Проверить корректность данных покупки
        /// </summary>
        /// <returns>True если данные корректны</returns>
        public bool IsValid()
        {
            return !string.IsNullOrWhiteSpace(ProductName)
                   && Quantity > 0
                   && Price > 0;
        }

        /// <summary>
        /// Получить строковое представление покупки
        /// </summary>
        public override string ToString()
        {
            return $"{ProductName} x {Quantity} = {TotalPrice:C}";
        }
    }
}
