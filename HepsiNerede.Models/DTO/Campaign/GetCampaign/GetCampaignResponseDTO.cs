namespace HepsiNerede.Models.DTO.Product.GetProduct
{
    public enum CampaignStatus
    {
        Incoming,
        Active,
        Ended
    }
    public class GetCampaignResponseDTO
    {
        public string? Status { get; set; }
        public decimal TargetSales { get; set; }
        public decimal TotalSales { get; set; }
        public decimal Turnover { get; set; }
        public decimal AverageItemPrice { get; set; }
    }
}
