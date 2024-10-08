
using api.DTOs.Requests;
using api.Exceptions;
using api.Models;
using api.Repositories;
using api.Transformers;
using api.Utilities;
using FluentEmail.Core;
using MongoDB.Bson;

namespace api.Services
{
    /// <summary>
    /// The OrderService class provides methods for managing orders.
    /// </summary>
    /// 
    /// <remarks>
    /// The OrderService class is responsible for creating, deleting, updating, and retrieving orders.
    /// It interacts with the OrderRepository, UserService, ProductService, and NotificationService classes.
    /// The class contains methods for creating an order, deleting an order, getting an order, searching orders,
    /// and updating an order. It also includes methods for validating an order and throwing exceptions if invalid.
    /// </remarks>
    public class OrderService(OrderRepository orderRepository, ILogger<OrderService> logger, UserService userService, ProductService productService, NotificationService notificationService)
    {
        private readonly ILogger<OrderService> _logger = logger;
        private readonly OrderRepository _orderRepository = orderRepository;
        private readonly UserService _userService = userService;
        private readonly ProductService _productService = productService;
        private readonly NotificationService _notificationService = notificationService;

        // This is a method for creating an order
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

        // This is a method for deleting an order
        public void DeleteOrder(string id)
        {
            _logger.LogInformation("Deleting order {id}", id);
            _orderRepository.Delete(id);
            _logger.LogInformation("Order deleted");
        }

        // This is a method for getting an order
        public Order GetOrder(string id)
        {
            _logger.LogInformation("Getting order {id}", id);
            var order = _orderRepository.GetById(id) ?? throw new BadRequestException($"Order with id {id} not found");
            _logger.LogInformation("Order found with id {id}", order.Id);
            return order;
        }

        // This is a method for searching orders
        public Page<Order> SearchOrders(PageRequest<Order> request)
        {
            _logger.LogInformation("Searching orders with page {page} and page size {pageSize}", request.Page, request.PageSize);
            var orders = _orderRepository.GetPage(request);
            _logger.LogInformation("Found {count} orders", orders.Data.Count());
            return orders;
        }

        // This is a method for updating an order
        public Order UpdateOrder(string id, UpdateOrderRequestDto request)
        {
            _logger.LogInformation("Updating order {id}", id);

            var oldOrder = GetOrder(id);
            var order = request.ToModel();
            order.Id = new ObjectId(id);

            // Get all relevant products
            var productIdsSet = new HashSet<string>(order.Products.Select(p => p.ProductId.ToString()));
            oldOrder.Products.Select(p => p.ProductId.ToString()).ForEach(p => productIdsSet.Add(p));
            var products = _productService.GetProducts(productIdsSet);

            ValidateOrderAndThrowIfInvalid(order, oldOrder, products);

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

            // Send notifications depending on flow
            var isOrderCancelled = order.Status == OrderStatus.Cancelled && oldOrder.Status != OrderStatus.Cancelled;
            if (isOrderCancelled)
            {
                _notificationService.CreateNotification(new Notification
                {
                    Recipient = AppUserRole.Customer,
                    UserId = oldOrder.UserId,
                    Reason = $"Your order with id {oldOrder.Id} has been cancelled",
                    Status = NotificationStatus.Unread,
                    Type = NotificationType.OrderCancelledWarning,
                });
            }
            else if (order.Status == OrderStatus.Delivered && oldOrder.Status != OrderStatus.Delivered)
            {
                _notificationService.CreateNotification(new Notification
                {
                    Recipient = AppUserRole.Customer,
                    UserId = oldOrder.UserId,
                    Reason = $"Your order with id {oldOrder.Id} has been delivered",
                    Status = NotificationStatus.Unread,
                    Type = NotificationType.OrderCompletedNotification,
                });
            }

            var updatedOrder = _orderRepository.Update(order);
            _logger.LogInformation("Order updated with id {id}", updatedOrder.Id);
            return updatedOrder;
        }

        // This is a method for validating an order and throwing an exception if invalid
        public void ValidateOrderAndThrowIfInvalid(Order order)
        {
            // Check whether all id references are valid
            if (!_userService.IsUserValid(order.UserId.ToString()))
            {
                throw new BadRequestException($"User with id ${order.UserId} not found");
            }

            foreach (var vendorId in order.VendorIds)
            {
                if (!_userService.IsUserValid(vendorId.ToString()))
                {
                    throw new BadRequestException($"Vendor with id ${vendorId} not found");
                }
            }

            // Check whether enough stock exists for all products
            foreach (var product in order.Products)
            {
                if (!_productService.IsProductValid(product.ProductId.ToString()))
                {
                    throw new BadRequestException($"Product with id ${product.ProductId} not found");
                }
                if (!_productService.IsProductStocked(product.ProductId.ToString(), product.Quantity))
                {
                    throw new BadRequestException($"Product ${product.ProductId} does not have ${product.Quantity} items in stock");
                }
            }
        }

        // This is a method for validating an order and throwing an exception if invalid
        public void ValidateOrderAndThrowIfInvalid(Order order, Order oldOrder, IEnumerable<Product> products)
        {
            // Check whether all id references are valid
            if (!_userService.IsUserValid(order.UserId.ToString()))
            {
                throw new BadRequestException($"User with id ${order.UserId} not found");
            }

            foreach (var vendorId in order.VendorIds)
            {
                if (!_userService.IsUserValid(vendorId.ToString()))
                {
                    throw new BadRequestException($"Vendor with id ${vendorId} not found");
                }
            }

            // Check whether enough stock exists for all products
            foreach (var item in order.Products)
            {
                var matchedProduct = products.FirstOrDefault(p => p.Id == item.ProductId);
                var oldOrderProduct = oldOrder.Products.FirstOrDefault(p => p.ProductId == item.ProductId);
                if (matchedProduct == null)
                {
                    throw new BadRequestException($"Product {item.ProductId} not found");
                }
                var countInStockAfterOrder = matchedProduct.CountInStock + oldOrderProduct?.Quantity ?? 0 - item.Quantity;
                if (countInStockAfterOrder < 0)
                {
                    throw new BadRequestException($"Product {item.ProductId} does not have ${item.Quantity} items in stock");
                }
            }
        }
    }
}