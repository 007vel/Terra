﻿
using System;
using System.Collections.Generic;
using System.Text;
using Acr.UserDialogs;
using Plugin.Toast;
using Xamarin.Forms;

namespace Terra.Core.Utils
{
    public class Utils
    {
        public static async void Toast(string msg)
        {
            UserDialogs.Instance.Toast(msg);
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
        public static string pad_an_int(int N, int P)
        {
            // string used in Format() method 
            string s = "{0:";
            for (int i = 0; i < P; i++)
            {
                s += "0";
            }
            s += "}";

            // use of string.Format() method 
            string value = string.Format(s, N);

            // return output 
            return value;
        }
    }
}
