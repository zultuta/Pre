using System.ComponentModel.DataAnnotations;

namespace Pre.UserProjectManager.Core.DTO
{
    public class LoginRequest
    {
        [Required]
        public string UserName { get; set; }
        [Required]
        public string Password { get; set; }
    }
}
