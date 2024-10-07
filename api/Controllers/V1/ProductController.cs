using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using api.DTOs.Models;
using api.DTOs.Requests;
using api.Models;
using api.Services;
using api.Transformers;
using api.Utilities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace api.Controllers.V1
{
    [Route("api/v1/products")]
    [ApiController]
    public class ProductController(ILogger<ProductController> logger, ProductService productService) : ControllerBase
    {
        private readonly ILogger<ProductController> _logger = logger;
        private readonly ProductService _productService = productService;

        // This is an endpoint exposed for getting a product
        [HttpGet("{id}")]
        public IActionResult GetProduct([FromRoute] string id)
        {
            var result = _productService.GetProduct(id);
            return Ok(result.ToDto());
        }

        // This is an endpoint exposed for searching products
        [HttpPost("search")]
        public IActionResult SearchProducts([FromBody] SearchRequestDto<Product> request)
        {
            var result = _productService.SearchProducts(request.ToPageRequest());
            var resultDtos = result.Data.Select(item => item.ToDto());
            return Ok(new Page<ProductDto>
            {
                Data = resultDtos,
                Meta = result.Meta,
            });
        }

        // This is an endpoint exposed for deleting a product
        [HttpDelete("{id}")]
        public IActionResult DeleteProduct([FromRoute] string id)
        {
            _productService.DeleteProduct(id);
            return NoContent();
        }

        // This is an endpoint exposed for updating a product
        [HttpPut("{id}")]
        public IActionResult UpdateProduct([FromRoute] string id, [FromBody] UpdateProductRequestDto request)
        {
            var result = _productService.UpdateProduct(id, request);
            return Ok(result.ToDto());
        }

        // This is an endpoint exposed for creating a product
        [HttpPost]
        public IActionResult CreateProduct([FromBody] CreateProductRequestDto request)
        {
            var result = _productService.CreateProduct(request);
            return Ok(result.ToDto());
        }
    }
}