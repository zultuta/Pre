namespace Pre.UserProjectManager.Core.Entity
{
    public class ProjectAssignment : BaseEntity
    {
        public long ProjectId { get; set; }
        public long AssigneeUserId { get; set; }
        public long AssignedBy { get; set; }
    }
}
