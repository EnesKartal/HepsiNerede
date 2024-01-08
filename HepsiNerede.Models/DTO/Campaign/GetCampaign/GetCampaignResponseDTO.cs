namespace HepsiNerede.Models.DTO.Product.GetProduct
{
    public class GetCampaignResponseDTO
    {
        public bool Status { get; set; }
        public decimal TargetSales { get; set; }
        public decimal TotalSales { get; set; }
        public decimal Turnover { get; set; }
        public decimal AverageItemPrice { get; set; }
    }
}
