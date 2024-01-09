namespace HepsiNerede.Data.Entities
{
    public class Order : BaseModel
    {
        public string ProductCode { get; set; }
        public decimal Quantity { get; set; }
        public decimal TotalPrice { get; set; }
    }
}
