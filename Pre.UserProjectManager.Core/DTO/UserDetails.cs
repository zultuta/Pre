namespace Pre.UserProjectManager.Core.DTO
{
    public class UserDetails : CreatedUser
    {
        public DateTime LastLoginDate { get; set; }
        public DateTime LastUpdatedTime { get; set; }
    }
}
