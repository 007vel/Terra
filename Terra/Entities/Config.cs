using System;
using System.Collections.Generic;
using System.Text;

namespace Entities
{
    // this object is used for config set epoch & sleep mode
    public class Config
    {
        public string request { get; set; }
        public int? sleep_mode { get; set; }
        public long? current_epoch { get; set; }
        public string timeZone { get; set; }
        public string info { get; set; }
    }
}
