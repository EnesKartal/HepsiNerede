using HepsiNerede.Application.DTOs.Product.CreateProduct;
using HepsiNerede.Application.DTOs.Product.GetProduct;
using HepsiNerede.Application.Services.Product;
using HepsiNerede.WebApp.Common;
using Microsoft.AspNetCore.Mvc;

namespace HepsiNerede.WebApp.Controllers
{
    /// <summary>
    /// API controller for managing products.
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class ProductController : ControllerBase
    {
        private readonly IProductCampaignService _productCampaignService;
        private readonly IProductService _productService;

        /// <summary>
        /// Initializes a new instance of the <see cref="ProductController"/> class.
        /// </summary>
        /// <param name="productCampaignService">The product campaign service.</param>
        /// <param name="productService">The product service.</param>
        public ProductController(IProductCampaignService productCampaignService, IProductService productService)
        {
            _productCampaignService = productCampaignService;
            _productService = productService;
        }

        /// <summary>
        /// Retrieves product information by product code.
        /// </summary>
        /// <param name="productCode">The product code.</param>
        /// <returns>Returns the product details wrapped in an API response.</returns>
        [HttpGet("getProductByCode")]
        public async Task<ActionResult<ApiResponse<GetProductResponseDTO>>> GetProductByCodeAsync([FromQuery] string? productCode)
        {
            if (string.IsNullOrEmpty(productCode))
                return BadRequest("Product code is required.");

            var product = await _productCampaignService.GetProductByCodeAsync(productCode);

            if (product == null)
                return NotFound($"Product with code '{productCode}' not found.");

            return Ok(new ApiResponse<GetProductResponseDTO>(new GetProductResponseDTO { Price = product.Price, Stock = product.Stock }, message: $"Product {productCode} info"));
        }

        /// <summary>
        /// Creates a new product.
        /// </summary>
        /// <param name="product">The product information for creation.</param>
        /// <returns>Returns the created product details wrapped in an API response.</returns>
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