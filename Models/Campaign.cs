public class Campaign : BaseModel
{
    public string Name { get; set; }
    public string ProductCode { get; set; }
    public int Duration { get; set; } //Hours
    public decimal PriceManipulationLimit { get; set; }
    public decimal TargetSaleCount { get; set; }
}