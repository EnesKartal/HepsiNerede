namespace HepsiNerede.Domain.Entities
{
    public class Product : BaseEntity
    {
        public string ProductCode { get; set; }
        public decimal Price { get; set; }
        public decimal Stock { get; set; }
    }
}
