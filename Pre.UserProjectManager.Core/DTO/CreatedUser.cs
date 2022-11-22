namespace Pre.UserProjectManager.Core.DTO
{
    public class CreatedUser
    {  
        public long Id { get; set; }
        public string FirstName { get; set; }      
        public string LastName { get; set; }        
        public string UserName { get; set; }            
        public string Email { get; set; }
        public DateTime TimeCreated { get; set; }
    }
}
