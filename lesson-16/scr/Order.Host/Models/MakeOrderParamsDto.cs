using System;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace Order.Host.Models
{
    /// <summary>
    /// Параметры заказа
    /// </summary>
    [DataContract]
    public class MakeOrderParamsDto
    {
        /// <summary>
        /// Сумма операции 
        /// </summary>
        [DataMember, Required]
        public decimal Amount { get; set; }
        
        /// <summary>
        /// Описание операции
        /// </summary>
        public string Description { get; set; }
        
        /// <summary>
        /// Уникльный идентификатор операции
        /// </summary>
        [DataMember, Required]
        public Guid OperationId { get; set; }
    }
}