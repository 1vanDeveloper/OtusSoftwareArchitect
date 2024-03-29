using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Order.Domain.Models;

namespace Order.Domain.Services
{
    /// <inheritdoc cref="IOrderService"/> 
    public class OrderService: IOrderService
    {
        private readonly AppDbContext _dbContext;
        private readonly IBillingService _billingService;

        /// <summary>
        /// Constructor
        /// </summary>
        public OrderService(AppDbContext dbContext, IBillingService billingService)
        {
            _dbContext = dbContext;
            _billingService = billingService;
        }
        
        public async Task<Models.Order> MakeOrderAsync(Models.Order order, CancellationToken cancellationToken)
        {
            Models.Order createdOrder;
            if (order.Id == 0)
            {
                createdOrder = await CreateOrderAsync(order, cancellationToken);
            }
            else
            {
                createdOrder = await _dbContext.Orders.FirstOrDefaultAsync(o => o.Id == order.Id, cancellationToken);
                if (createdOrder.UserId != order.UserId)
                {
                    throw new Exception("Заказ принадлежит другому пользователю");
                }

                if (createdOrder.State != OrderState.Unpaid)
                {
                    throw new Exception("Заказ не может быть оплачен");
                }
            }
            
            order.Timestamp = DateTime.Now;

            var (result, message) = await _billingService.BuyAsync(createdOrder.UserId, createdOrder.Amount,
                createdOrder.Description, createdOrder.OperationId, cancellationToken);

            if (!result)
            {
                throw new Exception(message);
            }
            
            createdOrder.State = OrderState.Paid;
            await _dbContext.SaveChangesAsync(cancellationToken);
            return createdOrder;
        }

        public Task<List<Models.Order>> GetOrdersAsync(long userId, CancellationToken cancellationToken)
        {
            return _dbContext.Orders.Where(o => o.UserId == userId).ToListAsync(cancellationToken);
        }

        private async Task<Models.Order> CreateOrderAsync(Models.Order order, CancellationToken cancellationToken)
        {
            if (order.Id != 0)
            {
                throw new Exception("Заказ уже существует");
            }

            if (order.State != OrderState.Unpaid)
            {
                throw new Exception("Заказ в неверном статусе");
            }

            await _dbContext.Orders.AddAsync(order, cancellationToken);
            await _dbContext.SaveChangesAsync(cancellationToken);

            return await _dbContext.Orders.FirstOrDefaultAsync(o => o.OperationId == order.OperationId, cancellationToken);;
        }
    }
}