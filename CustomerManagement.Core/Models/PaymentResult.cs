using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomerManagement.Core.Models
{
    /// <summary>
    /// Результат операции оплаты
    /// </summary>
    public class PaymentResult
    {
        /// <summary>
        /// Успешна ли операция оплаты
        /// </summary>
        public bool IsSuccess { get; set; }

        /// <summary>
        /// Сообщение о результате операции
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// Оплаченная сумма
        /// </summary>
        public decimal PaidAmount { get; set; }

        /// <summary>
        /// Остаток к оплате (если оплата частичная)
        /// </summary>
        public decimal RemainingAmount { get; set; }

        /// <summary>
        /// Дата и время операции
        /// </summary>
        public DateTime TransactionDate { get; set; }

        /// <summary>
        /// Конструктор по умолчанию
        /// </summary>
        public PaymentResult()
        {
            IsSuccess = false;
            Message = string.Empty;
            PaidAmount = 0m;
            RemainingAmount = 0m;
            TransactionDate = DateTime.Now;
        }

        /// <summary>
        /// Создать успешный результат оплаты
        /// </summary>
        public static PaymentResult Success(decimal paidAmount, decimal remainingAmount)
        {
            return new PaymentResult
            {
                IsSuccess = true,
                Message = "Оплата прошла успешно",
                PaidAmount = paidAmount,
                RemainingAmount = remainingAmount,
                TransactionDate = DateTime.Now
            };
        }

        /// <summary>
        /// Создать неуспешный результат оплаты
        /// </summary>
        public static PaymentResult Failure(string errorMessage)
        {
            return new PaymentResult
            {
                IsSuccess = false,
                Message = errorMessage,
                PaidAmount = 0m,
                RemainingAmount = 0m,
                TransactionDate = DateTime.Now
            };
        }

        /// <summary>
        /// Получить строковое представление результата
        /// </summary>
        public override string ToString()
        {
            if (IsSuccess)
            {
                return $"✓ Оплата {PaidAmount:N2} BYN успешно обработана. Остаток: {RemainingAmount:N2} BYN";
            }
            else
            {
                return $"✗ Ошибка оплаты: {Message}";
            }
        }
    }
}
