using System;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace Order.Host.Models
{
    /// <summary>
    /// Параметры покупки
    /// </summary>
    [DataContract]
    public class BuyParamsDto
    {
        /// <summary>
        /// Идентификатор пользователя
        /// </summary>
        [DataMember, Required]
        public long UserId { get; set; }
        
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