using Mopups.Pages;
using Terra_Maui.common;

namespace Terra_Maui.Views.PopUpPages;

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
        // await PopupNavigation.Instance.PopAsync();
        await Mopups.Services.MopupService.Instance.PopAsync();
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

        OnClose(null, null);
    }
}