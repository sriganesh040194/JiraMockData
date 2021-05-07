using System;
namespace JiraMockData.Model
{
    public class BacklogMetrics
    {
        public MetricsStatusCount taskStatusCount { get; set; }
        public MetricsStatusCount storyPoints { get; set; }
    }
}
