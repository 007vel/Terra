using System;
using ConnectionLibrary.Interface;
using ConnectionLibrary.Network;
using Terra.Core.ViewModels;
using Unity;
using Xamarin.Forms;

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
            SetMappings();
        }

        private void SetMappings()
        {
            //ViewModel
            container.RegisterType<DeviceDetailsViewModel>();
            container.RegisterType<NetworkViewModel>();

            //Service/adapter
            container.RegisterType<WifiAdapter>();
            container.RegisterType<IDevice, DeviceService>(); 
        }
        public object Resolve(Type type)
        {
            return container.Resolve(type);
        }
        public void SetBinding(Page page, Type viewModelBaseType)
        {
            page.BindingContext = (ViewModelBase)ServiceProvider.Instance.Resolve(viewModelBaseType);
        }
    }
}
