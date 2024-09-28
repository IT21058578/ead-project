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
    public class ProductService(ProductRepository productRepository, ILogger<ProductService> logger)
    {
        private readonly ILogger<ProductService> _logger = logger;
        private readonly ProductRepository _productRepository = productRepository;

        public Product CreateProduct(CreateProductRequestDto request)
        {
            _logger.LogInformation("Creating product");
            var product = request.ToModel();
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
            var updatedProduct = _productRepository.Update(product);
            _logger.LogInformation("Product updated with id {id}", updatedProduct.Id);
            return updatedProduct;
        }
    }
}