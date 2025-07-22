using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Terra_Maui.Controls.UIInterface
{
    public interface IScheduleOperation
    {
        void ReceiveEditedORNewschedule(object arg, string id, TimeSpan start, TimeSpan stop, string interval, bool active);
    }
}
