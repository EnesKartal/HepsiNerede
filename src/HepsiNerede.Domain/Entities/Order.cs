namespace HepsiNerede.Domain.Entities
{
    /// <summary>
    /// Represents an order entity.
    /// </summary>
    public class Order : BaseEntity
    {
        /// <summary>
        /// Gets or sets the product code associated with the order.
        /// </summary>
        public string ProductCode { get; set; }

        /// <summary>
        /// Gets or sets the quantity of the ordered product.
        /// </summary>
        public decimal Quantity { get; set; }

        /// <summary>
        /// Gets or sets the total price of the order.
        /// </summary>
        public decimal TotalPrice { get; set; }
    }
}
