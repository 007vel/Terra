using System;
using System.Collections.Generic;
using System.Text;

namespace ConnectionLibrary.Network
{
    public enum Endpoint
    {
        info, scheduler
    }
    public enum Endpoint_Method
    {
        POST, GET,UPDATE,DELETE
    }
    public class UrlConfig
    {
        readonly static string host = "ws://192.168.1.10:12345";
      //  readonly static string host = "ws://192.168.43.57:9898";

        public static string GetBaseURL()
        {
            return host;
        }
        public static string GetFullURL(Endpoint endpoint, Endpoint_Method method)
        {
            string url = "";
            url += host;
            if(method==Endpoint_Method.POST)
            {
                url += "/set";
            }else if (method == Endpoint_Method.GET)
            {
                url += "/get";
            }
            url += "/"+endpoint.ToString();
            return url;
        }
    }
}
