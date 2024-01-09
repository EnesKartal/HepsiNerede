namespace HepsiNerede.Domain.Entities
{
    /// <summary>
    /// Represents a campaign entity.
    /// </summary>
    public class Campaign : BaseEntity
    {
        /// <summary>
        /// Gets or sets the name of the campaign.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the product code associated with the campaign.
        /// </summary>
        public string ProductCode { get; set; }

        /// <summary>
        /// Gets or sets the duration of the campaign in hours.
        /// </summary>
        public int Duration { get; set; }

        /// <summary>
        /// Gets or sets the price manipulation limit for the campaign.
        /// </summary>
        public decimal PriceManipulationLimit { get; set; }

        /// <summary>
        /// Gets or sets the target sales count for the campaign.
        /// </summary>
        public decimal TargetSalesCount { get; set; }
    }
}
