using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Billing.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace Billing.Domain.Services
{
    /// <inheritdoc cref="ICashTransactionService"/> 
    internal class CashTransactionService: ICashTransactionService
    {
        private readonly AppDbContext _dbContext;
        private readonly INotificationEventService _notificationEventService;

        /// <summary>
        /// Constructor
        /// </summary>
        public CashTransactionService(AppDbContext dbContext, INotificationEventService notificationEventService)
        {
            _dbContext = dbContext;
            _notificationEventService = notificationEventService;
        }

        public async Task<CashTransaction> CreateCashTransactionAsync(CashTransaction cashTransaction, CancellationToken cancellationToken)
        {
            await using var tran = await _dbContext.Database.BeginTransactionAsync(cancellationToken);
            var result = cashTransaction.Type switch
            {
                CashTransactionType.Debit => await DepositAsync(cashTransaction, cancellationToken),
                CashTransactionType.Credit => await PayAsync(cashTransaction, cancellationToken),
                _ => throw new ArgumentOutOfRangeException()
            };
            await _notificationEventService.CreateNotificationEventAsync(result.OperationId, result.UserId, result.Description,
                cancellationToken);
            
            await tran.CommitAsync(cancellationToken);

            return result;
        }

        public Task<List<CashTransaction>> GetCashTransactionAsync(long userId, CancellationToken cancellationToken)
        {
            return _dbContext.CashTransactions.AsNoTracking().Where(ct => ct.UserId == userId).ToListAsync(cancellationToken);
        }

        public async Task<decimal> GetTotalAmountAsync(long userId, CancellationToken cancellationToken)
        {
            var debitSum = await _dbContext.CashTransactions
                .Where(ct =>
                    ct.UserId == userId && ct.Type == CashTransactionType.Debit && !ct.IsCanceled)
                .SumAsync(ct => ct.Amount, cancellationToken);
            
            var creditSum = await _dbContext.CashTransactions
                .Where(ct =>
                    ct.UserId == userId && ct.Type == CashTransactionType.Credit && !ct.IsCanceled)
                .SumAsync(ct => ct.Amount, cancellationToken);

            return debitSum - creditSum;
        }

        /// <summary>
        /// Положить на счет
        /// </summary>
        private async Task<CashTransaction> DepositAsync(CashTransaction cashTransaction, CancellationToken cancellationToken)
        {
            if (cashTransaction.Id != 0)
            {
                throw new Exception("Операция уже существует");
            }

            if (cashTransaction.Type != CashTransactionType.Debit)
            {
                throw new Exception("Операция не является пополнением");
            }

            var createdCashTransaction = (await _dbContext.CashTransactions.AddAsync(cashTransaction, cancellationToken)).Entity;
            await _dbContext.SaveChangesAsync(cancellationToken);

            return createdCashTransaction;
        }

        /// <summary>
        /// Положить на счет
        /// </summary>
        private async Task<CashTransaction> PayAsync(CashTransaction cashTransaction,
            CancellationToken cancellationToken)
        {
            if (cashTransaction.Id != 0)
            {
                throw new ArgumentException("Операция уже существует");
            }
            
            if (cashTransaction.UserId == 0)
            {
                throw new ArgumentException("Пользователь не указан");
            }

            if (cashTransaction.Type != CashTransactionType.Credit)
            {
                throw new ArgumentException("Операция не является списанием");
            }

            var totalAmount = await GetTotalAmountAsync(cashTransaction.UserId, cancellationToken);

            if (totalAmount <  cashTransaction.Amount)
            {
                throw new ArgumentException("Недостаточно средств на счете");
            }
            
            var createdCashTransaction = (await _dbContext.CashTransactions.AddAsync(cashTransaction, cancellationToken)).Entity;
            await _dbContext.SaveChangesAsync(cancellationToken);

            return createdCashTransaction;
        }
    }
}