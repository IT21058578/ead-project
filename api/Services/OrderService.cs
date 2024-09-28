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

        public void DeleteOrder(string id)
        {
            _orderRepository.Delete(id);
        }

        public Order GetOrder(string id)
        {
            var order = _orderRepository.GetById(id) ?? throw new Exception("Order not found");
            return order;
        }

        public IEnumerable<Order> SearchOrders()
        {
            var orders = _orderRepository.GetAll();
            return orders;
        }

        public Order UpdateOrder(string id, CreateOrderRequestDto request)
        {
            var order = request.ToModel();
            var updatedOrder = _orderRepository.Update(order);
            return updatedOrder;
        }
    }
}