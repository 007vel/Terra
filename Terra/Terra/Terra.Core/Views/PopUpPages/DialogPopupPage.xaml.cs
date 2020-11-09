using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Rg.Plugins.Popup.Pages;
using Rg.Plugins.Popup.Services;
using Terra.Core.common;
using Xamarin.Forms;

namespace Terra.Core.Views.PopUpPages
{
    public partial class DialogPopupPage : PopupPage
    {
        IDialog _IDialog;
        public DialogPopupPage(IDialog dialog)
        {
            InitializeComponent();
            _IDialog = dialog;
            titleLbl.Text = _IDialog.getTitle();
            inputEntry.Text = _IDialog.getValue();
            inputEntry.Keyboard = _IDialog.getkeyBoardType();
        }
        private async void OnClose(object sender, EventArgs e)
        {
            await PopupNavigation.Instance.PopAsync();
        }

        protected override Task OnAppearingAnimationEndAsync()
        {
            return Content.FadeTo(1);
        }

        protected override Task OnDisappearingAnimationBeginAsync()
        {
            return Content.FadeTo(0.5);
        }

        void Button_Clicked(System.Object sender, System.EventArgs e)
        {
            _IDialog.setValue(inputEntry.Text);
            OnClose(null,null);
        }
    }
}
