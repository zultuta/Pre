namespace Pre.UserProjectManager.Core.DTO
{
    public class ProductDetail
    {
        public long Id { get; set; }
        public string ProductName { get; set; }
        public double Weight { get; set; }
        public string Unit { get; set; }
        public decimal CarbonFootPrintPerGram { get; set; }
        public decimal CarbonFootPrint { get; set; }
        public DateTime TimeCreated { get; set; }
    }
}
