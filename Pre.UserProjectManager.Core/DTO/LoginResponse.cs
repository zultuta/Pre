namespace Pre.UserProjectManager.Core.DTO
{
    public class LoginResponse
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public long UserId { get; set; }
        public string Token { get; set; }
    }
}
