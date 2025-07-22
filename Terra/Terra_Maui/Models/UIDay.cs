using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terra_Maui.Enum;

namespace Terra_Maui.Models
{
    public class UIDay
    {
        public string day { get; set; }
        public SelectionStatus selectionStatus { get; set; }

        public double width { get; set; }

        public DateTime dateTime { get; set; }

    }
}
