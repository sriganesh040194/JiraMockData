using System;
namespace JiraMockData.Model
{
    public class Issuetype
    {
        public Issuetype(string name = "Task")
        {
            this.name = name;
        }
        public string name { get; private set; }
        public Boolean subtask { get => name == "Sub Task" ? true : false; }
    }
}
