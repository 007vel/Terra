using System;
using System.Collections.Generic;
using System.Text;

namespace Entities
{
    public class Schedules
    {
        public int scheduler_size { get; set; }
        public List<Scheduler> scheduler { get; set; }
    }
    public class Scheduler
    {
        public long start { get; set; }
        public long stop { get; set; }
        public long interval { get; set; }
        public string date { get; set; }
    }
}
