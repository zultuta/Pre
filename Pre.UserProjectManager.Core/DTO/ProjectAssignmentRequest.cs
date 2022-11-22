using System.ComponentModel.DataAnnotations;

namespace Pre.UserProjectManager.Core.DTO
{
    public class ProjectAssignmentRequest
    {
        [Range(1, Int64.MaxValue)]
        public long AssigneeUserId { get; set; }

        [Range(1, Int64.MaxValue)]
        public long ProjectId { get; set; }
    }
}
