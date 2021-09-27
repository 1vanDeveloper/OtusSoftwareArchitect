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
    public class CashTransactionService: ICashTransactionService
    {
        private readonly AppDbContext _dbContext;
        
        /// <summary>
        /// Constructor
        /// </summary>
        public CashTransactionService(AppDbContext dbContext)
        {
            _dbContext = dbContext;
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
            await tran.CommitAsync(cancellationToken);

            return result;
        }

        public Task<List<CashTransaction>> GetCashTransactionAsync(long userId, CancellationToken cancellationToken)
        {
            return _dbContext.CashTransactions.AsNoTracking().Where(ct => ct.UserId == userId).ToListAsync(cancellationToken);
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
                throw new Exception("Операция уже существует");
            }
            
            if (cashTransaction.UserId == 0)
            {
                throw new Exception("Пользователь не указан");
            }

            if (cashTransaction.Type != CashTransactionType.Credit)
            {
                throw new Exception("Операция не является списанием");
            }

            var debitSum = await _dbContext.CashTransactions
                .Where(ct =>
                    ct.UserId == cashTransaction.UserId && ct.Type == CashTransactionType.Debit && !ct.IsCanceled)
                .SumAsync(ct => ct.Amount, cancellationToken);
            
            var creditSum = await _dbContext.CashTransactions
                .Where(ct =>
                    ct.UserId == cashTransaction.UserId && ct.Type == CashTransactionType.Credit && !ct.IsCanceled)
                .SumAsync(ct => ct.Amount, cancellationToken);

            if (debitSum < creditSum + cashTransaction.Amount)
            {
                throw new Exception("Недостаточно средств на счете");
            }
            
            var createdCashTransaction = (await _dbContext.CashTransactions.AddAsync(cashTransaction, cancellationToken)).Entity;
            await _dbContext.SaveChangesAsync(cancellationToken);

            return createdCashTransaction;
        }
    }
}