using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.DTOs;
using Core.Entities.OrderAggregate;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace API.Extensions
{
    public static class OrderMappingExtensions
    {

        public static OrderDto  ToDto(this Order order)
        {
            return new OrderDto
            {
                Id = order.Id,
                BuyerEmail = order.BuyerEmail,
                OrderDate = order.OrderDate,
                ShippingAddress = order.ShippingAddress,
                PaymentSummary = order.PaymentSummary,
                DeliveryMethod = order.DeliveryMethod.Description,
                ShippingPrice = order.DeliveryMethod.Price,
                OrderItems = order.OrderItems.Select(x => x.ToDto()).ToList(),
                Subtotal = order.Subtotal,
                Total =  order.GetTotal(),
                Status = order.Status.ToString(),
                PaymentIntentId = order.PaymentIntentId,



            }; 
        }


        public static OrderItemDto ToDto(this OrderItem orderItem)
        {
            return new OrderItemDto
            {
              DeviceId =  orderItem.ItemOrdered.DeviceId,
              DeviceName = orderItem.ItemOrdered.DeviceName,
              ImageUrl =  orderItem.ItemOrdered.ImageUrl,
              Price = orderItem.Price,
              Quantity = orderItem.Quantity 
            };
        }
        
    }
}