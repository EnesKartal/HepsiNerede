namespace HepsiNerede.Domain.Entities
{
    public class Order : BaseEntity
    {
        public string ProductCode { get; set; }
        public decimal Quantity { get; set; }
        public decimal TotalPrice { get; set; }
    }
}
