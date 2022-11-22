namespace Pre.UserProjectManager.Core.Models.Filters
{
    public class ProjectFilter
    {
        public DateTime minDateAssigned { get; set; }
        public DateTime maxDateAssigned { get; set; } = DateTime.Now;
        public bool ValidDateRange => maxDateAssigned >= minDateAssigned;
    }
}
