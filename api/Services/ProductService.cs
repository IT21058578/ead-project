
using api.DTOs.Requests;
using api.Exceptions;
using api.Models;
using api.Repositories;
using api.Transformers;
using api.Utilities;

namespace api.Services
{
    /// <summary>
    /// The ProductService class provides methods for managing products.
    /// </summary>
    /// 
    /// <remarks>
    /// The ProductService class is responsible for creating, deleting, updating, and retrieving products.
    /// It interacts with the ProductRepository to perform CRUD operations on products.
    /// The class also utilizes the ILogger for logging information and the NotificationService and UserService for additional functionality.
    /// </remarks>
    public class ProductService(ProductRepository productRepository, ILogger<ProductService> logger, NotificationService notificationService, UserService userService)
    {
        private readonly ILogger<ProductService> _logger = logger;
        private readonly ProductRepository _productRepository = productRepository;
        private readonly NotificationService _notificationService = notificationService;
        private readonly UserService _userService = userService;

        // This is a method for creating a product
        public Product CreateProduct(CreateProductRequestDto request)
        {
            _logger.LogInformation("Creating product");
            var product = request.ToModel();
            ValidateProductAndThrowIfInvalid(product);
            var savedProduct = _productRepository.Add(product);
            _logger.LogInformation("Product created with id {id}", savedProduct.Id);
            return savedProduct;
        }

        // This is a method for deleting a product
        public void DeleteProduct(string id)
        {
            _logger.LogInformation("Deleting product {id}", id);
            _productRepository.Delete(id);
            _logger.LogInformation("Product deleted");
        }

        // This is a method for getting a product
        public Product GetProduct(string id)
        {
            _logger.LogInformation("Getting product {id}", id);
            var product = _productRepository.GetById(id) ?? throw new BadRequestException($"Product with id {id} not found");
            _logger.LogInformation("Product found with id {id}", product.Id);
            return product;
        }

        //  This is a method for getting all products
        public IEnumerable<Product> GetProducts(IEnumerable<string> ids)
        {
            _logger.LogInformation("Getting all products");
            var products = _productRepository.GetByIds(ids);
            _logger.LogInformation("Found {count} products", products.Count());
            return products;
        }

        // This is a method for searching products
        public Page<Product> SearchProducts(PageRequest<Product> request)
        {
            _logger.LogInformation("Searching products with page {page} and page size {pageSize}", request.Page, request.PageSize);
            var products = _productRepository.GetPage(request);
            _logger.LogInformation("Found {count} products", products.Data.Count());
            return products;
        }

        // This is a method for updating a product
        public Product UpdateProduct(string id, UpdateProductRequestDto request)
        {
            _logger.LogInformation("Updating product {id}", id);
            var existingProduct = GetProduct(id);
            var product = request.ToModel(existingProduct);
            ValidateProductAndThrowIfInvalid(product);
            var updatedProduct = _productRepository.Update(product);
            _logger.LogInformation("Product updated with id {id}", updatedProduct.Id);
            return updatedProduct;
        }

        // This is a method for updating the rating of a product
        public async Task UpdateRating(string id, double rating)
        {
            _logger.LogInformation("Updating product rating for product {id}", id);
            var product = GetProduct(id);
            product.Rating = rating;
            _productRepository.Update(product);
            _logger.LogInformation("Product rating updated to {rating}", rating);
        }

        // This is a method for checking whether a product is valid
        public bool IsProductValid(string id)
        {
            _logger.LogInformation("Checking whether product {id} is valid", id);
            var product = _productRepository.GetById(id);
            return product != null;
        }

        // This is a method for checking whether a product has enough stock
        public bool IsProductStocked(string id, int quantity)
        {
            _logger.LogInformation("Checking whether product {id} has enough stock", id);
            var product = _productRepository.GetById(id);
            return product != null && product.CountInStock >= quantity;
        }

        // This is a method for decreasing the stock of a product
        public void DecreaseProductStock(string id, int quantity)
        {
            _logger.LogInformation("Decreasing stock for product {id} by {quantity}", id, quantity);
            var product = GetProduct(id);
            if (product.CountInStock < quantity)
            {
                throw new BadRequestException($"Product with id {id} does not have enough stock");
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

        // This is a method for increasing the stock of a product
        public void IncreaseProductStock(string id, int quantity)
        {
            _logger.LogInformation("Increasing stock for product {id} by {quantity}", id, quantity);
            var product = GetProduct(id);
            product.CountInStock += quantity;
            _productRepository.Update(product);
            _logger.LogInformation("Stock increased for product {id}", id);
        }

        // This is a method for validating a product and throwing an exception if invalid
        public void ValidateProductAndThrowIfInvalid(Product product)
        {
            if (product.Price <= 0)
            {
                throw new BadRequestException($"Invalid price");
            }
            if (product.CountInStock < 0)
            {
                throw new BadRequestException($"Invalid stock");
            }
            if (product.LowStockThreshold < 0)
            {
                throw new BadRequestException($"Invalid low stock threshold");
            }
            if (!_userService.IsUserValid(product.VendorId.ToString()))
            {
                throw new BadRequestException($"User with id ${product.VendorId} not found");
            }
        }
    }
}