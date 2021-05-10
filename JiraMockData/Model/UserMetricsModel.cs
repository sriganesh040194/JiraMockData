using System;
namespace JiraMockData.Model
{
    public class UserMetricsModel
    {
        public UserMetricsModel()
        {
        }

        public string displayName  { get; set; }
        public string accountId { get; set; }
        public string email { get; set;}
        public Metrics taskStatusCount { get; set; }
        public Metrics storyPoints { get; set; }
        public string processedDate { get; set; }
    }
}
