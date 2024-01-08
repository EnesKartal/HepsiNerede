namespace HepsiNerede.Models.DTO.Product.CreateProduct
{
    public class CreateProductRequestDTO
    {
        public string ProductCode { get; set; }
        public decimal Price { get; set; }
        public decimal Stock { get; set; }
    }
}
