using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Terra_Maui.Controls.Week
{
    public interface IWeekControlBase
    {
        void buildDayUI(List<UIDay> _DaysList);
    }
}
