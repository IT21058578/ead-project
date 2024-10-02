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
    public class ProductService(ProductRepository productRepository, ILogger<ProductService> logger, NotificationService notificationService, UserService userService)
    {
        private readonly ILogger<ProductService> _logger = logger;
        private readonly ProductRepository _productRepository = productRepository;
        private readonly NotificationService _notificationService = notificationService;
        private readonly UserService _userService = userService;

        public Product CreateProduct(CreateProductRequestDto request)
        {
            _logger.LogInformation("Creating product");
            var product = request.ToModel();
            ValidateProductAndThrowIfInvalid(product);
            var savedProduct = _productRepository.Add(product);
            _logger.LogInformation("Product created with id {id}", savedProduct.Id);
            return savedProduct;
        }

        public void DeleteProduct(string id)
        {
            _logger.LogInformation("Deleting product {id}", id);
            _productRepository.Delete(id);
            _logger.LogInformation("Product deleted");
        }

        public Product GetProduct(string id)
        {
            _logger.LogInformation("Getting product {id}", id);
            var product = _productRepository.GetById(id) ?? throw new Exception("Product not found");
            _logger.LogInformation("Product found with id {id}", product.Id);
            return product;
        }

        public IEnumerable<Product> GetProducts(IEnumerable<string> ids)
        {
            _logger.LogInformation("Getting all products");
            var products = _productRepository.GetByIds(ids);
            _logger.LogInformation("Found {count} products", products.Count());
            return products;
        }

        public Page<Product> SearchProducts(PageRequest<Product> request)
        {
            _logger.LogInformation("Searching products with page {page} and page size {pageSize}", request.Page, request.PageSize);
            var products = _productRepository.GetPage(request);
            _logger.LogInformation("Found {count} products", products.Data.Count());
            return products;
        }

        public Product UpdateProduct(string id, CreateProductRequestDto request)
        {
            _logger.LogInformation("Updating product {id}", id);
            var product = request.ToModel();
            ValidateProductAndThrowIfInvalid(product);
            var updatedProduct = _productRepository.Update(product);
            _logger.LogInformation("Product updated with id {id}", updatedProduct.Id);
            return updatedProduct;
        }

        public async Task UpdateRating(string id, double rating)
        {
            _logger.LogInformation("Updating product rating for product {id}", id);
            var product = GetProduct(id);
            product.Rating = rating;
            _productRepository.Update(product);
            _logger.LogInformation("Product rating updated to {rating}", rating);
        }

        public bool IsProductValid(string id)
        {
            _logger.LogInformation("Checking whether product {id} is valid", id);
            var product = _productRepository.GetById(id);
            return product != null;
        }

        public bool IsProductStocked(string id, int quantity)
        {
            _logger.LogInformation("Checking whether product {id} has enough stock", id);
            var product = _productRepository.GetById(id);
            return product != null && product.CountInStock >= quantity;
        }

        public void DecreaseProductStock(string id, int quantity)
        {
            _logger.LogInformation("Decreasing stock for product {id} by {quantity}", id, quantity);
            var product = GetProduct(id);
            if (product.CountInStock < quantity)
            {
                throw new Exception("Not enough stock");
            }
            product.CountInStock -= quantity;
            _productRepository.Update(product);


            if (product.CountInStock < product.LowStockThreshold)
            {
                _logger.LogWarning("Product {id} has low stock", id);
                _notificationService.CreateNotification(new Notification
                {
                    Type = NotificationType.LowStockWarning,
                    Recipient = AppUserRole.Vendor,
                    ProductId = product.Id,
                    UserId = product.VendorId,
                    Reason = $"Product with Id {product.Id} and Name {product.Name} has low stock",
                });
                _logger.LogInformation("Low stock notification created for product {id}", id);
            }
            _logger.LogInformation("Stock decreased for product {id}", id);
        }

        public void IncreaseProductStock(string id, int quantity)
        {
            _logger.LogInformation("Increasing stock for product {id} by {quantity}", id, quantity);
            var product = GetProduct(id);
            product.CountInStock += quantity;
            _productRepository.Update(product);
            _logger.LogInformation("Stock increased for product {id}", id);
        }

        public void ValidateProductAndThrowIfInvalid(Product product)
        {
            if (product.Price <= 0)
            {
                throw new Exception("Invalid price");
            }
            if (product.CountInStock < 0)
            {
                throw new Exception("Invalid stock");
            }
            if (product.LowStockThreshold < 0)
            {
                throw new Exception("Invalid low stock threshold");
            }
            if (_userService.IsUserValid(product.VendorId.ToString()))
            {
                throw new Exception("Invalid vendor user");
            }
        }
    }
}