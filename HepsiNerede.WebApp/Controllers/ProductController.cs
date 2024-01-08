using HepsiNerede.Models.DTO;
using HepsiNerede.Services;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/product")]
public class ProductController : ControllerBase
{
    private readonly IProductService _productService;

    public ProductController(IProductService productService)
    {
        _productService = productService;
    }

    [HttpGet("getProductByCode")]
    public IActionResult GetProductByCode([FromQuery] string? productCode)
    {
        if (string.IsNullOrEmpty(productCode))
            return BadRequest("Product code is required.");

        var product = _productService.GetProductByCode(productCode);

        if (product == null)
            return NotFound($"Product with code '{productCode}' not found.");

        return Ok(product);
    }

    [HttpPost("addProduct")]
    public IActionResult AddProduct([FromBody] AddProductDTO product)
    {
        if (product == null)
            return BadRequest("Product is required.");

        var createdProduct = _productService.AddProduct(product);

        return Ok(createdProduct);
    }
}
