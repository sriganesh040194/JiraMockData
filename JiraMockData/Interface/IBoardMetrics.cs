using System;
using System.Collections.Generic;
using JiraMockData.Model;

namespace JiraMockData.Interface
{
    public interface IBoardMetrics
    {
        int id { get; set; }
        string name { get; set; }
        string projectType { get; set; }
        Project project { get; set; }
        Sprint sprint { get; set; }
        MetricsStatusCount taskStatusCount { get; set; }
        MetricsStatusCount storyPoints { get; set; }
        BacklogMetrics backlogMetrics { get; set; }
        string processedDate { get; set; }
        List<IssueAssignee> issues { get; set; }
        List<Issue> backlogIssues { get; set; }
    }
}
