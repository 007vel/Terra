namespace Terra_Maui.Views;

public partial class NetworkListPage : ContentPage
{
    public NetworkListPage()
    {
        InitializeComponent();
    }
    NetworkViewModel context = null;
    public NetworkViewModel Context
    {
        get
        {
            if (context == null)
            {
                context = this.BindingContext as NetworkViewModel;
            }
            return context;
        }
    }
    protected override void OnAppearing()
    {
        base.OnAppearing();
        OTAHelper.Instance.EnableHeartBeat = false;
        if (context != null)
        {
            context.PageNavigation = Navigation;
            context.Init();
        }
        // SocketHelper.Instance.StopHeartBeatcheck();
    }
    protected override void OnDisappearing()
    {
        base.OnDisappearing();
    }

}