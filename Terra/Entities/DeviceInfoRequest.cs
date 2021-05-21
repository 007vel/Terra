using System;
using System.Collections.Generic;
using System.Text;

namespace Entities
{
   public class DeviceInfoRequest
    {
        public string request { get; set; }
        public string info { get; set; }
        public string Dispenser_Type { get; set; } = null;
        public string value { get; set; }
        public int? index { get; set; }
    }
}
