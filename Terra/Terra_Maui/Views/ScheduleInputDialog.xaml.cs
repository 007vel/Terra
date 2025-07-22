namespace Terra_Maui.Views;

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