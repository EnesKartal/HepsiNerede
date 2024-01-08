namespace HepsiNerede.Data.Entities
{
    public class Product : BaseModel
    {
        public string ProductCode { get; set; }
        public decimal Price { get; set; }
        public decimal Stock { get; set; }
    }
}
