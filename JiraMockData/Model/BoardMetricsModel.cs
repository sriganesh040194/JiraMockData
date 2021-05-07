using System;
using System.Collections.Generic;
using JiraMockData.Interface;

namespace JiraMockData.Model
{
    public class BoardMetricsModel : IBoardMetrics
    {
        public BoardMetricsModel()
        {
        }

        public int id { get; set; }
        public string name { get; set; }
        public string projectType { get; set; }
        public Project project { get; set; }
        public Sprint sprint { get; set; }
        public MetricsStatusCount taskStatusCount { get; set; }
        public MetricsStatusCount storyPoints { get; set; }
        public BacklogMetrics backlogMetrics { get; set; }
        public string processedDate { get; set; }
        public List<IssueAssignee> issues { get; set; }
        public List<Issue> backlogIssues { get; set; }
    }
}
