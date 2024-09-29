using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.DTOs.Requests;
using api.Models;
using api.Repositories;
using api.Transformers;
using api.Utilities;

namespace api.Services
{
    public class OrderService(OrderRepository orderRepository, ILogger<OrderService> logger)
    {
        private readonly ILogger<OrderService> _logger = logger;
        private readonly OrderRepository _orderRepository = orderRepository;

        public Order CreateOrder(CreateOrderRequestDto request)
        {
            _logger.LogInformation("Creating order");
            var order = request.ToModel();
            var savedOrder = _orderRepository.Add(order);
            _logger.LogInformation("Order created with id {id}", savedOrder.Id);
            return savedOrder;
        }

        public void DeleteOrder(string id)
        {
            _logger.LogInformation("Deleting order {id}", id);
            _orderRepository.Delete(id);
            _logger.LogInformation("Order deleted");
        }   

        public Order GetOrder(string id)
        {
            _logger.LogInformation("Getting order {id}", id);
            var order = _orderRepository.GetById(id) ?? throw new Exception("Order not found");
            _logger.LogInformation("Order found with id {id}", order.Id);
            return order;
        }

        public Page<Order> SearchOrders(PageRequest<Order> request)
        {
            _logger.LogInformation("Searching orders with page {page} and page size {pageSize}", request.Page, request.PageSize);
            var orders = _orderRepository.GetPage(request);
            _logger.LogInformation("Found {count} orders", orders.Data.Count());
            return orders;
        }

        public Order UpdateOrder(string id, CreateOrderRequestDto request)
        {
            _logger.LogInformation("Updating order {id}", id);
            var order = request.ToModel();
            var updatedOrder = _orderRepository.Update(order);
            _logger.LogInformation("Order updated with id {id}", updatedOrder.Id);
            return updatedOrder;
        }
    }
}