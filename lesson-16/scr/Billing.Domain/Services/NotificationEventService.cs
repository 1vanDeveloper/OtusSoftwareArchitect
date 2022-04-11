using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Billing.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace Billing.Domain.Services
{
    /// <inheritdoc cref="INotificationEventService"/> 
    internal class NotificationEventService : INotificationEventService
    {
        private readonly AppDbContext _dbContext;

        /// <summary>
        /// Constructor
        /// </summary>
        public NotificationEventService(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        
        public async Task CreateNotificationEventAsync(Guid operationId, long userId, string message,
            CancellationToken cancellationToken)
        {
            if (operationId == Guid.Empty)
            {
                throw new Exception("Операция не валидна");
            }
            
            if (string.IsNullOrWhiteSpace(message))
            {
                throw new Exception("Сообщение пусто");
            }

            await _dbContext.NotificationEvents.AddAsync(
                new NotificationEvent
                {
                    OperationId = operationId,
                    UserId = userId,
                    Message = message
                }, cancellationToken);
            
            await _dbContext.SaveChangesAsync(cancellationToken);
        }

        public Task<List<NotificationEvent>> GetNotificationEventAsync(CancellationToken cancellationToken)
        {
            return _dbContext.NotificationEvents.AsNoTracking().ToListAsync(cancellationToken);
        }

        public async Task RemoveNotificationEventAsync(Guid operationId, CancellationToken cancellationToken)
        {
            var record = await _dbContext.NotificationEvents.FirstOrDefaultAsync(e => e.OperationId == operationId, cancellationToken);
            if (record == null)
            {
                return;
            }
            _dbContext.NotificationEvents.Remove(record);
            await _dbContext.SaveChangesAsync(cancellationToken);
        }
    }
}