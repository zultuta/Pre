using System.ComponentModel.DataAnnotations;

namespace Pre.UserProjectManager.Core.DTO
{
    public class NewProjectRequest
    {
        [Required]
        public string ProjectName { get; set; }
    }
}
