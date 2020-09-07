using System;
using ConnectionLibrary.Network;
using Unity;

namespace Terra.Service
{
    public class ServiceProvider
    {
        private static ServiceProvider serviceProvider;
        private IUnityContainer container = new UnityContainer();

        public static ServiceProvider Instance
        {
            get
            {
                if(serviceProvider==null)
                {
                    serviceProvider = new ServiceProvider();
                }
                return serviceProvider;
            }
        }

        public IUnityContainer Container
        {
            get
            {
                return container;
            }
        }
        private ServiceProvider()
        {
            buildMappings();
        }

        private void buildMappings()
        {
            container.RegisterType<WifiAdapter>();
           // container.RegisterType<IWifiManager, WifiAdapter>(); 
        }
    }
}
