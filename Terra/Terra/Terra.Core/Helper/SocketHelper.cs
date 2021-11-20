using System;
using System.Threading;
using System.Threading.Tasks;
using ConnectionLibrary.Interface;
using ConnectionLibrary.Network;
using Entities;
using Newtonsoft.Json;

namespace Terra.Core.Helper
{
    public class SocketHelper
    {
        private bool _enableHeartBeat = false;
        private static SocketHelper socketHelper = null;


        private SocketHelper()
        {

        }

       
        public static SocketHelper Instance
        {
            get
            {
                if(socketHelper == null)
                {
                    socketHelper = new SocketHelper();
                }
                return socketHelper;
            }
        }

        public async Task StartHeartBeatcheck(IDevice deviceService)
        {
            if (_enableHeartBeat) return;

            _enableHeartBeat = true;
            while (_enableHeartBeat)
            {
                await Task.Delay(10 * 1000);
                GetHeartBeat(deviceService);
            }
        }
        public void StopHeartBeatcheck()
        {
            _enableHeartBeat = false;
        }

        /// <summary>
        /// the infinet call is to keep alive the socket, otherwise it will be disconnected
        /// </summary>
        /// <returns></returns>
        private async Task<HeartBeat> GetHeartBeat(IDevice deviceService)
        {
            try
            {
                HeartBeat heartBeat = new HeartBeat();
                heartBeat.type = "check";
                var deviceRes = await deviceService.GetHealthCheck(heartBeat);
                NetworkServiceUtil.Log("SocketHelper GetHeartBeat: " + deviceRes);
                if (!string.IsNullOrEmpty(deviceRes))
                {
                    var health = JsonConvert.DeserializeObject<HeartBeat>(deviceRes);
                    return health;
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex);
                NetworkServiceUtil.Log("SocketHelper GetHeartBeat Exception: " + ex);
            }
            return null;
        }

    }
}
