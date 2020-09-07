using System;
using System.Threading.Tasks;
using Xamarin.Essentials;
using static Xamarin.Essentials.Permissions;

namespace Terra.Core.Helper
{
    public class PermissionHelper
    {
        private PermissionHelper() { }
        static PermissionHelper permissionHelper = null;
        public static PermissionHelper Instance
        {
            get
            {
                if (permissionHelper == null)
                {
                    permissionHelper = new PermissionHelper();
                }
                return permissionHelper;
            }
        }
        public async Task<PermissionStatus> CheckAndRequestPermissionAsync<T>(T permission)
             where T : BasePermission
        {
            var status = await permission.CheckStatusAsync();
            if (status != PermissionStatus.Granted)
            {
                /*The request will display the permission only if the permission in not given*/
                status = await permission.RequestAsync();
            }
            return status;
        }
    }
}
