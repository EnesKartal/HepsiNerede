public interface IProductRepository
{
    Product? GetProductByCode(string productCode);
    void AddProduct(Product product);
}

public class ProductRepository : IProductRepository
{
    private readonly List<Product> _products;

    public ProductRepository()
    {
        _products = new List<Product>();
    }

    public Product? GetProductByCode(string productCode)
    {
        return _products.FirstOrDefault(p => p.ProductCode == productCode);
    }

    public void AddProduct(Product product)
    {
        _products.Add(product);
    }
}
