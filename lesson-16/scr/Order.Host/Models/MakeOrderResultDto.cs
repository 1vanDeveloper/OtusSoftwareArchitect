using System;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using Order.Domain.Models;

namespace Order.Host.Models
{
    /// <summary>
    /// Заказ
    /// </summary>
    [DataContract]
    public class MakeOrderResultDto
    {
        /// <summary>
        /// Идентификатор
        /// </summary>
        [DataMember, Required]
        public long Id { get; set; }
        
        /// <summary>
        /// Дата и время совершения операции
        /// </summary>
        [DataMember, Required]
        public DateTime Timestamp { get; set; }
        
        /// <summary>
        /// Состояние заказа
        /// </summary>
        [DataMember, Required]
        public OrderState State { get; set; }
        
        /// <summary>
        /// Сумма заказа 
        /// </summary>
        [DataMember, Required]
        public decimal Amount { get; set; }
        
        /// <summary>
        /// Описание заказа
        /// </summary>
        [DataMember, Required]
        public string Description { get; set; }
        
        /// <summary>
        /// Уникльный идентификатор операции
        /// </summary>
        [DataMember, Required]
        public Guid OperationId { get; set; }
    }
}