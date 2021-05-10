using System;
namespace JiraMockData.Model
{
    public class ELK
    {
        public string host { get; set; }
        public string port { get; set; }
    }
    public class AppConfigModel
    {
        public ELK elk { get; set; }
    }
}
