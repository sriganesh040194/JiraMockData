using System;
namespace JiraMockData.Model
{
    public class Sprint
    {
        public int id { get; set; }
        public string state { get; set; }
        public string name { get; set; }
        public string startDate { get; set; }
        public string endDate { get; set; }
        public string goal { get; set; }
    }
}
