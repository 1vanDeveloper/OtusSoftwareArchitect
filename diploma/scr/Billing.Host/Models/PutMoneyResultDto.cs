using System;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace Billing.Host.Models
{
    /// <summary>
    /// Результат пополнения счета
    /// </summary>
    [DataContract]
    public class MoneyResultDto
    {
        /// <summary>
        /// Итого на счете 
        /// </summary>
        [DataMember]
        public decimal TotalAmount { get; set; }
        
        /// <summary>
        /// Уникльный идентификатор операции
        /// </summary>
        [DataMember]
        public Guid OperationId { get; set; }
    }
}