using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terra.Core.Controls.Week;
using Terra.Core.Enum;
using Terra.Core.Models;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Terra.Core.Controls
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class WeekCardControl : WeekControl
    {
        public WeekCardControl()
        {
            InitializeComponent();
            SetRootView = weeklayout;
        }
        public void TapGestureRecognizer_Tapped(object sender, EventArgs e)
        {
            
           // var element = ((TappedEventArgs)e).Parameter as Card;
           var btn= ((ImageButton)sender).CommandParameter;
            var element = (Card)btn;
            var ctx = element.BindingContext;
            var UIday = (UIDay)ctx;
            
            UIday.selectionStatus = UIday.selectionStatus== SelectionStatus.Selected? SelectionStatus.NotSlected: SelectionStatus.Selected;
            System.Diagnostics.Debug.WriteLine("----------TapGestureRecognizer_Tapped-----------" + UIday.day + "-------" + UIday.selectionStatus);
            SetViewState(element, UIday);
            element.BindingContext = UIday;
            ((ImageButton)sender).CommandParameter = element;
        }
        public override void buildDayUI(List<UIDay> _DaysList)
        {
            Grid grid = new Grid();
            this.Children.Add(grid);
            foreach (var item in _DaysList)
            {
                ColumnDefinition columnDefinition = new ColumnDefinition();
                columnDefinition.Width = GridLength.Star;
                grid.ColumnDefinitions.Add(columnDefinition);
            }
            RowDefinition rowDefinition = new RowDefinition();
            rowDefinition.Height = 45;
            grid.RowDefinitions.Add(rowDefinition);
            grid.Margin = new Thickness(0,5,0,5);
           // grid.ColumnSpacing = 4;
           // grid.RowSpacing = 0;
            grid.VerticalOptions = LayoutOptions.Fill;
            grid.HorizontalOptions = LayoutOptions.FillAndExpand;
            int index = 0;
            foreach (var item in _DaysList)
            {
                var tapGestureRecognizer_1 = new TapGestureRecognizer();
                tapGestureRecognizer_1.Tapped += TapGestureRecognizer_Tapped;
                var tapGestureRecognizer_2 = new TapGestureRecognizer();
                tapGestureRecognizer_2.Tapped += TapGestureRecognizer_Tapped;
                var tapGestureRecognizer_3 = new TapGestureRecognizer();
                tapGestureRecognizer_3.Tapped += TapGestureRecognizer_Tapped;

                Card card = new Card();
                card.VerticalOptions = LayoutOptions.FillAndExpand;
                tapGestureRecognizer_1.CommandParameter = card;
                tapGestureRecognizer_2.CommandParameter = card;
                tapGestureRecognizer_3.CommandParameter = card;
                
                card.HasShadow = false;
                card.CornerRadius = 8;
                card.Padding = new Thickness(0, 0, 0, 0);
                card.Margin = new Thickness(0, 4, 0, 4);
                Label day = new Label();
                day.Padding = new Thickness(0,0,0,0);
                day.FontAttributes = FontAttributes.Bold;
                SetViewState(card, item);
                day.VerticalOptions = LayoutOptions.CenterAndExpand;
                day.HorizontalOptions = LayoutOptions.CenterAndExpand;
                card.BindingContext = item;
                day.Text = item.day;
               // day.GestureRecognizers.Add(tapGestureRecognizer_1);
               // card.GestureRecognizers.Add(tapGestureRecognizer_2);
                card.Content = day;
                card.Margin = new Thickness(4);
                StackLayout stlt = new StackLayout();
                stlt.HorizontalOptions = LayoutOptions.Fill;
                stlt.VerticalOptions = LayoutOptions.Fill;
                stlt.Spacing = 0;
                stlt.Children.Add(card);
                Grid.SetColumn(stlt, index);
                grid.Children.Add(stlt);

                ImageButton button = new ImageButton();
                button.VerticalOptions = LayoutOptions.Fill;
                button.BackgroundColor = Color.Transparent;
                button.Margin = new Thickness(5,0,0,-20);
                button.HeightRequest = 50;
                button.Clicked += Button_Clicked;
                button.CommandParameter = card;
                button.BindingContext = card;

                Grid.SetColumn(button, index);
                grid.Children.Add(button);

                index++;

        }
            
    }

        private void Button_Clicked(object sender, EventArgs e)
        {
            TapGestureRecognizer_Tapped(sender,e);
        }
    }
}