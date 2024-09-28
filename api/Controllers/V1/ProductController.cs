using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using api.DTOs.Requests;
using api.Models;
using api.Services;
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

        [HttpGet("{id}")]
        public IActionResult GetProduct([FromRoute] string id)
        {
            var result = _productService.GetProduct(id);
            return Ok(result);
        }

        [HttpPost("search")]
        public IActionResult SearchProducts([FromBody] SearchRequestDto<Product> request)
        {
            var result = _productService.SearchProducts(request.ToPageRequest());
            return Ok(result);
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteProduct([FromRoute] string id)
        {
            _productService.DeleteProduct(id);
            return NoContent();
        }

        [HttpPut("{id}")]
        public IActionResult UpdateProduct([FromRoute] string id, [FromBody] CreateProductRequestDto request)
        {
            var result = _productService.UpdateProduct(id, request);
            return Ok(result);
        }

        [HttpPost]
        public IActionResult CreateProduct([FromBody] CreateProductRequestDto request)
        {
            var result = _productService.CreateProduct(request);
            return Ok(result);
        }
    }
}