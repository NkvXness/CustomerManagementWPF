using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomerManagement.Core.Models
{
    /// <summary>
    /// Представляет договор с покупателем
    /// </summary>
    public class Contract
    {
        /// <summary>
        /// Номер договора (уникальный идентификатор)
        /// </summary>
        public string ContractNumber { get; set; }

        /// <summary>
        /// Дата подписания договора
        /// </summary>
        public DateTime SignDate { get; set; }

        /// <summary>
        /// Активен ли договор в данный момент
        /// </summary>
        public bool IsActive { get; set; }

        /// <summary>
        /// Дата расторжения договора (null если договор не расторгнут)
        /// </summary>
        public DateTime? TerminationDate { get; set; }

        /// <summary>
        /// Конструктор по умолчанию
        /// </summary>
        public Contract()
        {
            ContractNumber = string.Empty;
            SignDate = DateTime.Now;
            IsActive = false;
            TerminationDate = null;
        }

        /// <summary>
        /// Конструктор с параметрами для создания нового договора
        /// </summary>
        /// <param name="contractNumber">Номер договора</param>
        /// <param name="signDate">Дата подписания</param>
        public Contract(string contractNumber, DateTime signDate)
        {
            ContractNumber = contractNumber;
            SignDate = signDate;
            IsActive = true;
            TerminationDate = null;
        }

        /// <summary>
        /// Расторгнуть договор
        /// </summary>
        public void Terminate()
        {
            IsActive = false;
            TerminationDate = DateTime.Now;
        }

        /// <summary>
        /// Возобновить расторгнутый договор
        /// </summary>
        /// <param name="newSignDate">Новая дата подписания</param>
        public void Renew(DateTime newSignDate)
        {
            IsActive = true;
            SignDate = newSignDate;
            TerminationDate = null;
        }

        /// <summary>
        /// Получить строковое представление договора
        /// </summary>
        public override string ToString()
        {
            string status = IsActive ? "Активен" : "Расторгнут";
            return $"Договор №{ContractNumber} от {SignDate:dd.MM.yyyy} - {status}";
        }
    }
}
