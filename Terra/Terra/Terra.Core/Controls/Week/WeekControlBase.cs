using System;
using System.Collections.Generic;
using System.Text;
using Terra.Core.Models;

namespace Terra.Core.Controls.Week
{
   public interface IWeekControlBase
    {
        void buildDayUI(List<UIDay> _DaysList);
    }
}
