namespace Pre.UserProjectManager.Core.Entity
{
    public class Product : BaseEntity
    {
        public string Name { get; set; }
        public double Weight { get; set; }
        public string Unit { get; set; }
        public decimal CarbonFootPrintPerGram { get; set; }
        public decimal CarbonFootPrint { get; set; }
        public long ProjectId { get; set; }
        public Project Project { get; set; }

    }
}
