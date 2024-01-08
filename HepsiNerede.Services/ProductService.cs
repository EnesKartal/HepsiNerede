using HepsiNerede.Data.Entities;
using HepsiNerede.Data.Repositories;
using HepsiNerede.Models.DTO;

namespace HepsiNerede.Services
{
    public interface IProductService
    {
        Product? GetProductByCode(string productCode);
        Product AddProduct(AddProductDTO addProductDTO);
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

        public Product AddProduct(AddProductDTO addProductDTO)
        {
            var newProduct = new Product
            {
                ProductCode = addProductDTO.ProductCode,
                Price = addProductDTO.Price,
                Stock = addProductDTO.Stock
            };

            return _productRepository.AddProduct(newProduct);
        }
    }
}
