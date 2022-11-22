namespace Pre.UserProjectManager.Core.Models.Filters
{
    public class ProductFilter
    {
        public decimal minCarbonFootPrint { get; set; }
        public decimal maxCarbonFootPrint { get; set; }
        public bool ValidCarbonFootPrinRange => maxCarbonFootPrint >= minCarbonFootPrint;
    }
}
