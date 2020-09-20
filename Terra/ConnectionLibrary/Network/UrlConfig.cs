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
        readonly static string host = "ws://127.0.0.1";
        public static string GetFullURL(Endpoint endpoint, Endpoint_Method method)
        {
            string url = "";
            url += host;
            if(method==Endpoint_Method.POST)
            {
                url += "/set";
            }else if (method == Endpoint_Method.POST)
            {
                url += "/get";
            }
            url += "/"+endpoint.ToString();
            return url;
        }
    }
}
