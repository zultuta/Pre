namespace Pre.UserProjectManager.Core.Entity
{
    public class BaseEntity
    {
        public long Id { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime TimeCreated { get; set; }
        public DateTime LastUpdatedTime { get; set; }
    }
}
