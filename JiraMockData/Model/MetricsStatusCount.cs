using System;
namespace JiraMockData.Model
{
    public class Metrics
    {
        public Metrics(int toDo = 0, int inProgress = 0, int done = 0)
        {
            this.toDo = toDo;
            this.inProgress = inProgress;
            this.done = done;
        }

        public int inProgress { get; set; }
        public int toDo { get; set; }
        public int done { get; set; }
        public int total { get => inProgress+toDo+done;}
    }

    public class MetricsStatusCount: Metrics
    {
        public MetricsStatusCount(int toDo = 0, int inProgress = 0, int done = 0) : base(toDo, inProgress, done)
        {

        }

        public MetricsStatusCount(Metrics metrics) : base(metrics.toDo, metrics.inProgress, metrics.done)
        {

        }
        public Metrics unassigned { get; set; }
    }
}
