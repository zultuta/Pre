using System.ComponentModel.DataAnnotations;

namespace Pre.UserProjectManager.Core.DTO
{
    public class NewProductRequest
    {
        [Required]
        public string Name { get; set; }

        [Range(0, double.MaxValue)]
        public double Weight { get; set; }

        [Required]
        public string Unit { get; set; }

        [Required]
        public decimal CarbonFootPrintPerGram { get; set; }

        [Range(0, Int64.MaxValue)]
        public long ProjectId { get; set; }
    }
}
