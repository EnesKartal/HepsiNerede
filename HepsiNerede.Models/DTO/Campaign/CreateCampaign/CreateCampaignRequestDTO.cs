namespace HepsiNerede.Models.DTO.Campaign.CreateCampaign
{
    public class CreateCampaignRequestDTO
    {
        public string Name { get; set; }
        public string ProductCode { get; set; }
        public int Duration { get; set; }
        public decimal PMLimit { get; set; }
        public decimal TSCount { get; set; }
    }
}
