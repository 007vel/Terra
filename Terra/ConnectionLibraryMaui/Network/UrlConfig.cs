using System;
using System.Collections.Generic;
using System.Text;

namespace ConnectionLibrary.Network
{
    public enum Endpoint
    {
        info, scheduler, config, demo, deleteschedule, snapshot, upload
    }
    public enum Endpoint_Method
    {
        POST, GET,UPDATE,DELETE
    }
    public class UrlConfig
    {
        readonly static string host = "ws://192.168.1.10:12345";
      //  readonly static string host = "ws://192.168.43.57:9898";
      readonly static string uploadurl= "http://192.168.1.10/upload/TeraaDev.bin";

        public static string GetBaseURL()
        {
            return host;
        }
        public static string GetFullURL(Endpoint endpoint, Endpoint_Method method,bool isNewFW=true)
        {
            string url = "";
            url += host;
            if(endpoint == Endpoint.upload)
            {
                return uploadurl;
            }
            else if(!isNewFW)//ota_data_initial
            {
                if (method == Endpoint_Method.POST)
                {
                    url += "/set";
                }
                else if (method == Endpoint_Method.GET)
                {
                    url += "/get";
                }
                url += "/" + endpoint.ToString();
            }
            else
            {
                url += "/";
            }
            
          
            return url;
        }
    }
}
