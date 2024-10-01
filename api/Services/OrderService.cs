using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.DTOs.Requests;
using api.Models;
using api.Repositories;
using api.Transformers;
using api.Utilities;
using MongoDB.Bson;

namespace api.Services
{
    public class OrderService(OrderRepository orderRepository, ILogger<OrderService> logger, UserService userService, ProductService productService)
    {
        private readonly ILogger<OrderService> _logger = logger;
        private readonly OrderRepository _orderRepository = orderRepository;
        private readonly UserService _userService = userService;
        private readonly ProductService _productService = productService;

        public Order CreateOrder(CreateOrderRequestDto request)
        {
            _logger.LogInformation("Attempting to create an order");

            // Create order
            var order = request.ToModel();
            ValidateOrderAndThrowIfInvalid(order);

            // Substract stock from products
            foreach (var product in order.Products)
            {
                _productService.DecreaseProductStock(product.ProductId.ToString(), product.Quantity);
            }
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

            var oldOrder = GetOrder(id);

            var order = request.ToModel();
            order.Id = new ObjectId(id);
            ValidateOrderAndThrowIfInvalid(order);

            // Add products allocated to old order back
            foreach (var product in oldOrder.Products)
            {
                _productService.IncreaseProductStock(product.ProductId.ToString(), product.Quantity);
            }

            // Remove products allocated to new order 
            foreach (var product in order.Products)
            {
                _productService.DecreaseProductStock(product.ProductId.ToString(), product.Quantity);
            }

            var updatedOrder = _orderRepository.Update(order);
            _logger.LogInformation("Order updated with id {id}", updatedOrder.Id);
            return updatedOrder;
        }



        public void ValidateOrderAndThrowIfInvalid(Order order)
        {
            // Check whether all id references are valid
            if (!_userService.IsUserValid(order.UserId.ToString()))
            {
                throw new Exception("User not found");
            }

            foreach (var vendorId in order.VendorIds)
            {
                if (!_userService.IsUserValid(vendorId.ToString()))
                {
                    throw new Exception("Vendor not found");
                }
            }

            // Check whether enough stock exists for all products
            foreach (var product in order.Products)
            {
                if (!_productService.IsProductValid(product.ProductId.ToString()))
                {
                    throw new Exception($"Product {product.ProductId} not found");
                }
                if (!_productService.IsProductStocked(product.ProductId.ToString(), product.Quantity))
                {
                    throw new Exception($"Product {product.ProductId} does not have ${product.Quantity} items in stock");
                }
            }
        }
    }
}