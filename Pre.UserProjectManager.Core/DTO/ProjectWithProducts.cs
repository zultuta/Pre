namespace Pre.UserProjectManager.Core.DTO
{
    public class ProjectWithProducts
    {
        public ProjectWithProducts()
        {
            Products = new List<ProductDetail>();
        }
        public long Id { get; set; }
        public string Name { get; set; }
        public DateTime TimeCreated { get; set; }
        public List<ProductDetail> Products { get; set; }
    }
}
