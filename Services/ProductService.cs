public interface IProductService
{
    Product? GetProductByCode(string productCode);
    void AddProduct(string productCode, decimal price, int stock);
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

    public void AddProduct(string productCode, decimal price, int stock)
    {
        var newProduct = new Product
        {
            ProductCode = productCode,
            Price = price,
            Stock = stock
        };

        _productRepository.AddProduct(newProduct);
    }
}