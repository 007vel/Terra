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
        public static Scheduler buildScheduleObject(List<UIDay> days, TimeSpan start, TimeSpan stop, string interval)
        {
            Scheduler scheduleObject = new Scheduler();
            scheduleObject.start = timeSpantoSeconds(start);
            scheduleObject.stop = timeSpantoSeconds(stop);
            scheduleObject.interval = Convert.ToInt64( interval);
            var selecteddays = new List<string>();
            foreach(var item in days)
            {
                if(item.selectionStatus== SelectionStatus.Selected)
                {
                    selecteddays.Add(item.day);
                }                
            }
            scheduleObject.date = string.Join(",", selecteddays);
            return scheduleObject;
        }
        static long timeSpantoSeconds(TimeSpan timeSpan)
        {
            var time_1 = timeSpan.Ticks;
            var now = DateTime.Now;
            new DateTime(now.Year, now.Month, now.Day, 0, 0, 0);
            var time_2 = now.Ticks*10000*1000;
            long diff = ((time_1 / 10000) / 1000)- ((time_2 / 10000) / 1000);
            return diff;
        }
    }
}
