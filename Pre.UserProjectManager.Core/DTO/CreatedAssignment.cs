namespace Pre.UserProjectManager.Core.DTO
{
    public class CreatedAssignment
    {
        public long Id { get; set; }
        public long ProjectId { get; set; }
        public long AssigneeUserId { get; set; }
        public DateTime TimeCreated { get; set; }
    }
}
