using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Acr.UserDialogs;


namespace MobileApp.App.Controls
{
    public static class AcrUserDialogExt
    {
        public static void ShowLoading(string str)
        {
            UserDialogs.Instance.ShowLoading(str);
        }

        public static void HideHud()
        {
            UserDialogs.Instance.HideLoading();
        }
    }
}
