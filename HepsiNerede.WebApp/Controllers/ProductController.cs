using HepsiNerede.Models.DTO.Product.CreateProduct;
using HepsiNerede.Models.DTO.Product.GetProduct;
using HepsiNerede.Services;
using HepsiNerede.WebApp.Common;
using Microsoft.AspNetCore.Mvc;

namespace HepsiNerede.WebApp.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductController : ControllerBase
    {
        private readonly IProductService _productService;

        public ProductController(IProductService productService)
        {
            _productService = productService;
        }

        [HttpGet("getProductByCode")]
        public async Task<ActionResult<ApiResponse<GetProductResponseDTO>>> GetProductByCodeAsync([FromQuery] string? productCode)
        {
            if (string.IsNullOrEmpty(productCode))
                return BadRequest("Product code is required.");

            var product = await _productService.GetProductByCodeAsync(productCode);

            if (product == null)
                return NotFound($"Product with code '{productCode}' not found.");

            return Ok(new ApiResponse<GetProductResponseDTO>(new GetProductResponseDTO { Price = product.Price, Stock = product.Stock }, message: $"Product {productCode} info"));
        }

        [HttpPost("createProduct")]
        public async Task<ActionResult<ApiResponse<CreateProductResponseDTO>>> CreateProductAsync([FromBody] CreateProductRequestDTO product)
        {
            if (product == null)
                return BadRequest("Product is required.");

            var createdProduct = await _productService.CreateProductAsync(product);

            return Ok(new ApiResponse<CreateProductResponseDTO>(new CreateProductResponseDTO { Price = createdProduct.Price, Code = createdProduct.ProductCode, Stock = createdProduct.Stock }, message: "Product created"));
        }
    }
}