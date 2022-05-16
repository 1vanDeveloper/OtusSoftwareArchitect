using System;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Order.Domain.Models
{
    /// <summary>
    /// Заказ
    /// </summary>
    [Index(nameof(OperationId), IsUnique = true)]
    public class Order
    {
        /// <summary>
        /// Идентификатор
        /// </summary>
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }
        
        /// <summary>
        /// Идентификатор пользователя
        /// </summary>
        public long UserId { get; set; }
        
        /// <summary>
        /// Дата и время совершения операции
        /// </summary>
        public DateTime Timestamp { get; set; }
        
        /// <summary>
        /// Состояние заказа
        /// </summary>
        public OrderState State { get; set; }
        
        /// <summary>
        /// Сумма заказа 
        /// </summary>
        public decimal Amount { get; set; }
        
        /// <summary>
        /// Описание заказа
        /// </summary>
        public string Description { get; set; }
        
        /// <summary>
        /// Уникльный идентификатор операции
        /// </summary>
        public Guid OperationId { get; set; }
    }
}