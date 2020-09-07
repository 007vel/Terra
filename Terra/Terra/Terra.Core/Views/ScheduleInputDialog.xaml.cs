using Rg.Plugins.Popup.Pages;
using Rg.Plugins.Popup.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Terra.Core.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ScheduleInputDialog : PopupPage
    {
        public ScheduleInputDialog()
        {
            InitializeComponent();
        }
        private async void OnClose(object sender, EventArgs e)
        {
            await PopupNavigation.Instance.PopAsync();
        }


    }
}