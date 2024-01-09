namespace HepsiNerede.Application.DTOs.Campaign.CreateCampaign
{
    public class CreateCampaignResponseDTO
    {
        public string Name { get; set; }
        public string Product { get; set; }
        public int Duration { get; set; }
        public decimal Limit { get; set; }
        public decimal TargetSalesCount { get; set; }
    }
}
