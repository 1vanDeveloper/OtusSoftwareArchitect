using System;
using Billing.Domain.Models;
using Billing.Host.Models;

namespace Billing.Host.Extensions
{
    /// <summary>
    /// Dto helper functions
    /// </summary>
    public static class DtoExtensions
    {
        /// <summary>
        /// Convert to domain model
        /// </summary>
        public static CashTransaction ToCashTransaction(this BuyParamsDto buyParams)
        {
            return new CashTransaction
            {
                Timestamp = DateTime.Now,
                Type = CashTransactionType.Credit,
                OperationId = buyParams.OperationId,
                UserId = buyParams.UserId,
                Description = buyParams.Description,
                Amount = buyParams.Amount
            };
        }
        
        /// <summary>
        /// Convert to domain model
        /// </summary>
        public static CashTransaction ToCashTransaction(this PutMoneyParamsDto putMoneyParamsDto, long userId)
        {
            return new CashTransaction
            {
                Timestamp = DateTime.Now,
                Type = CashTransactionType.Debit,
                OperationId = putMoneyParamsDto.OperationId,
                UserId = userId,
                Description = "Пополнение счета пользователем",
                Amount = putMoneyParamsDto.Amount
            };
        }
    }
}