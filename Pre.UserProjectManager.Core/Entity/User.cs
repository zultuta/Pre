namespace Pre.UserProjectManager.Core.Entity
{
    public class User : BaseEntity
    {
        public User()
        {
            Projects = new List<Project>();
        }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string PasswordSalt { get; set; }
        public DateTime LastLoginDate { get; set; }
        public ICollection<Project> Projects { get; set; }
    }
}
