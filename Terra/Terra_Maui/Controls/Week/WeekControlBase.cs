using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terra_Maui.Models;

namespace Terra_Maui.Controls.Week
{
    public interface IWeekControlBase
    {
        void buildDayUI(List<UIDay> _DaysList);
    }
}
