using System;
using System.Collections.Generic;
using System.Text;
using Terra.Core.Enum;

namespace Terra.Core.Models
{
   public class UIDay
    {
        public string day { get; set; }
        public SelectionStatus selectionStatus { get; set; }

        public double width { get; set; }

        public DateTime dateTime { get; set; }

    }
}
