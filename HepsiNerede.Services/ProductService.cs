using HepsiNerede.Data.Entities;
using HepsiNerede.Data.Repositories;
using HepsiNerede.Models.DTO.Product.CreateProduct;

namespace HepsiNerede.Services
{
    public interface IProductService
    {
        Product? GetProductByCode(string productCode);
        Product CreateProduct(CreateProductRequestDTO createProductDTO);
    }

    public class ProductService : IProductService
    {
        private readonly IProductRepository _productRepository;

        public ProductService(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        public Product? GetProductByCode(string productCode)
        {
            return _productRepository.GetProductByCode(productCode);
        }

        public Product CreateProduct(CreateProductRequestDTO createProductDTO)
        {
            var newProduct = new Product
            {
                ProductCode = createProductDTO.ProductCode,
                Price = createProductDTO.Price,
                Stock = createProductDTO.Stock
            };

            return _productRepository.CreateProduct(newProduct);
        }
    }
}
