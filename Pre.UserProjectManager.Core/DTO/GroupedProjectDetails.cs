namespace Pre.UserProjectManager.Core.DTO
{
    public class GroupedProjectDetails
    {
        public GroupedProjectDetails()
        {
            MyProjects = new List<ProjectDetail>();
            OtherProjects = new List<ProjectDetail>();
        }
        public IEnumerable<ProjectDetail> MyProjects { get; set; }
        public IEnumerable<ProjectDetail> OtherProjects { get; set; }
    }
}
