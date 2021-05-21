using Entities;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Text;
using Terra.Core.Enum;
using Terra.Core.Models;

namespace Terra.Core.Utils
{
   public class JSONUtil
    {
        public static Scheduler Build_Scheduler(List<UIDay> days, TimeSpan start, TimeSpan stop, string interval, bool _active)
        {
            Scheduler scheduleObject = new Scheduler();
            scheduleObject.start = timeSpantoSeconds(start);
            scheduleObject.stop = timeSpantoSeconds(stop);
            scheduleObject.interval = Convert.ToInt64( interval);
            scheduleObject.active = _active.ToString();
            var selecteddays = new List<string>();
            foreach(var item in days)
            {
                if(item.selectionStatus== SelectionStatus.Selected)
                {
                    selecteddays.Add(item.dateTime.ToString("dddd"));
                }                
            }
            scheduleObject.day = string.Join(",", selecteddays);
            return scheduleObject;
        }       
        static long timeSpantoSeconds(TimeSpan timeSpan)
        {
            TimeSpan sinceMidnight = timeSpan - new DateTime(year: DateTime.Now.Year,month: DateTime.Now.Month,day: DateTime.Now.Day, hour:0,minute:0,second:0).TimeOfDay;
            long secs = Convert.ToInt64( sinceMidnight.TotalSeconds);
            return secs;
        }
    }
}
