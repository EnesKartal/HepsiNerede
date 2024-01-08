﻿using HepsiNerede.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace HepsiNerede.Data.Repositories
{
    public interface IProductRepository
    {
        Product? GetProductByCode(string productCode);
        Product AddProduct(Product product);
    }

    public class ProductRepository : IProductRepository
    {
        private readonly HepsiNeredeDBContext _dbContext;

        public ProductRepository(HepsiNeredeDBContext dbContext)
        {
            _dbContext = dbContext;
        }

        public Product? GetProductByCode(string productCode)
        {
            return _dbContext.Products
            .AsNoTracking()
            .FirstOrDefault(p => p.ProductCode == productCode);
        }

        public Product AddProduct(Product product)
        {
            product.CreatedAt = DateTime.Now;
            _dbContext.Add(product);
            _dbContext.SaveChanges();
            return product;
        }
    }
}
