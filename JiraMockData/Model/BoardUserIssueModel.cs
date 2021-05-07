using System;
using System.Collections.Generic;

namespace JiraMockData.Model
{
    public class BoardUserIssueModel
    {
        public string boardAndUserName { get; set; }
        public string boardName { get; set; }
        public string displayName { get; set; }
        public string accountId { get; set; }
        public string emailAddress { get; set; }
        public Metrics taskStatusCount { get; set; }
        public Metrics storyPoints { get; set; }
        public string processedDate { get; set; }
        public Sprint sprint { get; set; }
        public Project project { get; set; }
        public List<IssueWithoutAssignee> issue { get; set; }
    }
}
