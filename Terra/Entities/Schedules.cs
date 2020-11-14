using System;
using System.Collections.Generic;
using System.Text;

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
        public List<Scheduler> scheduler { get; set; }
    }
    public class Scheduler
    {
        public long start { get; set; }
        public long stop { get; set; }
        public long interval { get; set; }
        public string day { get; set; }
    }
}
