namespace Pre.UserProjectManager.Core.Entity
{
    public class Project : BaseEntity
    {
        public Project()
        {
            Products = new List<Product>();
        }
        public string Name { get; set; }
        public long UserId { get; set; }
        public User User { get; set; }
        public ICollection<Product> Products { get; set; }
    }
}
