using System.ComponentModel.DataAnnotations;

namespace Pre.UserProjectManager.Core.DTO
{
    public class UpdateUserRequest
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }

        [MinLength(8)]
        public string Password { get; set; }

        [EmailAddress]
        public string Email { get; set; }
    }
}
