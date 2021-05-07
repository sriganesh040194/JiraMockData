using System;
namespace JiraMockData.Model
{
    public class IssueWithoutAssignee
    {
        public int id { get; set; }
        public string summary { get; set; }
        public string key { get; set; }
        public string created { get; set; }
        public string updated { get; set; }
        public string lastViewed { get; set; }
        public string resolutiondate { get; set; }
        public string timeestimate { get; set; }
        public string timeoriginalestimate { get; set; }
        public string timespent { get; set; }
        public CustomField customField { get; set; }
        public Status status { get; set; }
        public Issuetype issuetype { get; set; }
        public Priority priority { get; set; }
    }

    public class Issue : IssueWithoutAssignee
    {
        public IssueAssignee assignee { get; set; }

    }


}
