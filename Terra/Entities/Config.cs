using System;
using System.Collections.Generic;
using System.Text;

namespace Entities
{
   public class Config
    {
        public string request { get; set; }
        public int? sleep_mode { get; set; }
        public long? current_epoch { get; set; }
        public string timeZone { get; set; }
    }
}
