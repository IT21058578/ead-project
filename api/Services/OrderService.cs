using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.DTOs.Requests;
using api.Models;
using api.Repositories;
using api.Transformers;

namespace api.Services
{
    public class OrderService(OrderRepository orderRepository)
    {
        private readonly OrderRepository _orderRepository = orderRepository;

        public Order CreateOrder(CreateOrderRequestDto request)
        {
            var order = request.ToModel();
            var savedOrder = _orderRepository.Add(order);
            return savedOrder;
        }
    }
}