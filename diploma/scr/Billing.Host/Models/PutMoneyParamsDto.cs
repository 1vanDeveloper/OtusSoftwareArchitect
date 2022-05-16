using System;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace Billing.Host.Models
{
    /// <summary>
    /// Параметры пополнения счета
    /// </summary>
    [DataContract]
    public class PutMoneyParamsDto
    {
        /// <summary>
        /// Сумма операции 
        /// </summary>
        [DataMember, Required]
        public decimal Amount { get; set; }
        
        /// <summary>
        /// Уникльный идентификатор операции
        /// </summary>
        [DataMember, Required]
        public Guid OperationId { get; set; }
    }
}