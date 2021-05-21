using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace Entities
{
    public class Schedules
    {
        public int scheduler_size {
            get
            {
                if(scheduler!=null)
                {
                    return scheduler.Count;
                }
                return 0;
            }
        }

        public string request { get; set; }
        public string info { get; set; }
        public List<Scheduler> scheduler { get; set; }
    }
    public class Scheduler
    {
        [JsonIgnore]
        public string index { get; set; }
        public long start { get; set; }
        public long stop { get; set; }
        public long interval { get; set; }
        public string day { get; set; }
        public string active { get; set; }
    }
}
