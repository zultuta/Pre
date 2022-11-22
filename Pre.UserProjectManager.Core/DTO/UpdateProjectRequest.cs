using System.ComponentModel.DataAnnotations;

namespace Pre.UserProjectManager.Core.DTO
{
    public class UpdateProjectRequest
    {
        [Range(1, Int64.MaxValue)]
        public long ProjectId { get; set; }
        [Required]
        public string ProjectName { get; set; }
    }
}
