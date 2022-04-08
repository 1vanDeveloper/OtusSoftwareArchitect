using System;
using System.Collections.Generic;
using System.Linq;
using Order.Host.Models;

namespace Order.Host.Extensions
{
    public static class DtoExtensions
    {
        public static Domain.Models.Order ToOrder(this MakeOrderParamsDto orderParams, long userId)
        {
            return new Domain.Models.Order
            {
                Timestamp = DateTime.Now,
                OperationId = orderParams.OperationId,
                UserId = userId,
                Description = orderParams.Description,
                Amount = orderParams.Amount
            };
        }
        
        public static MakeOrderResultDto ToMakeOrderResult(this Domain.Models.Order order)
        {
            return new MakeOrderResultDto
            {
                Id = order.Id,
                Timestamp = order.Timestamp,
                State = order.State,
                Amount = order.Amount,
                Description = order.Description,
                OperationId = order.OperationId
            };
        }
        
        public static GetOrdersResultDto ToGetOrdersResultDto(this IList<Domain.Models.Order> orders)
        {
            return new GetOrdersResultDto
            {
                Orders = orders.Select(ToOrderResultDto).ToList()
            };
        }

        private static GetOrderResultDto ToOrderResultDto(Domain.Models.Order order)
        {
            return new GetOrderResultDto
            {
                Id = order.Id,
                Timestamp = order.Timestamp,
                State = order.State,
                Amount = order.Amount,
                Description = order.Description,
                OperationId = order.OperationId
            };
        }
    }
}