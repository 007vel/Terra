
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace Terra.Core.Utils
{
    public class Utils
    {
        public static async void Toast(string title, string msg)
        {


            //    var result = await notificator.Notify(options);
        }

        public static long GetEpochSeconds()
        {
            TimeSpan t = DateTime.UtcNow - new DateTime(1970, 1, 1);
            long secondsSinceEpoch = (long)t.TotalSeconds;
            Console.WriteLine(secondsSinceEpoch);
            return secondsSinceEpoch;
        }
        public static string GetTimeZoneInfo()
        {
            TimeZone curTimeZone = TimeZone.CurrentTimeZone;
            TimeSpan currentOffset = curTimeZone.GetUtcOffset(DateTime.Now);
            Console.WriteLine("UTC offset:", currentOffset);
            string offset = "GMT ";
            if (!currentOffset.ToString().Contains("-"))
            {
                offset = offset + "+"+currentOffset.ToString();
            }
            else
            {
                offset=offset + currentOffset.ToString();
            }
            return offset;

        }
    }
}
