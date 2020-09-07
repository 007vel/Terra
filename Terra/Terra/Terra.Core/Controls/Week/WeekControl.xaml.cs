using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terra.Core.Enum;
using Terra.Core.Models;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Terra.Core.Controls
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class WeekControl : StackLayout
    {
        StackLayout view=new StackLayout();
        public WeekControl(StackLayout view)
        {
            InitializeComponent();
            Spacing = 0;
            BindingContext = this;
            this.view = view;
            view.Children.Add(this);
        }
        public WeekControl()
        {
            InitializeComponent();
            Spacing = 0;
            BindingContext = this;
        }
        public StackLayout SetRootView
        {
            set{
                view = value;
            }
        }
        public static BindableProperty DaysProperty = BindableProperty.Create(
                                      propertyName: "DaysList",
                                      returnType: typeof(List<UIDay>),
                                      declaringType: typeof(List<UIDay>),
                                      defaultBindingMode: BindingMode.TwoWay,
                                      propertyChanged: DaysPropertyChanged);

        public static void DaysPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (bindable != null)
            {
                var control = (WeekControl)bindable;
                var uiList = (List<UIDay>)newValue;
                uiList.ForEach(i => i.width = control.view.WidthRequest / 7);
                control._DaysList = uiList;
            }
        }
        public List<UIDay> DaysList
        {
            get { return (List<UIDay>)base.GetValue(DaysProperty); }
            set { base.SetValue(DaysProperty, value); }
        }
        List<UIDay> _daysList;
        public List<UIDay> _DaysList
        {
            get
            {
                return _daysList;
            }
            set
            {
                //  DaysListUI.ItemsSource = value;
                _daysList = value;
                buildDayUI(_daysList);
                OnPropertyChanged();
            }
        }
        public virtual void buildDayUI(List<UIDay> _DaysList)
        {
            Grid grid = new Grid();
            this.Children.Add(grid);
            foreach (var item in _DaysList)
            {
                ColumnDefinition columnDefinition = new ColumnDefinition();
                columnDefinition.Width = GridLength.Star;
                grid.ColumnDefinitions.Add(columnDefinition);
            }
           // grid.BackgroundColor = Color.Blue;
            int index = 0;
            foreach (var item in _DaysList)
            {
                var tapGestureRecognizer = new TapGestureRecognizer();
                tapGestureRecognizer.Tapped += TapGestureRecognizer_Tapped;
                

                grid.ColumnSpacing = 0;
                grid.RowSpacing = 0;
                grid.VerticalOptions = LayoutOptions.Fill;
                grid.HorizontalOptions = LayoutOptions.FillAndExpand;
                Label day = new Label();
                day.FontAttributes = FontAttributes.Bold;
                SetLabelState(day, item);
                day.VerticalOptions = LayoutOptions.CenterAndExpand;
                day.Margin = new Thickness(0,0,0,5);
                grid.BindingContext = item;
                day.BindingContext = item;
                day.GestureRecognizers.Add(tapGestureRecognizer);
                grid.GestureRecognizers.Add(tapGestureRecognizer);
                day.Text = item.day;
               // day.SetBinding(Label.TextProperty, "day");
                Grid.SetColumn(day,index);
                grid.Children.Add(day);
                
                
                index++;
            }

        }

        public void TapGestureRecognizer_Tapped(object sender, EventArgs e)
        {
            var element = (View)sender;
            var ctx = element.BindingContext;
           // throw new NotImplementedException();
        }
        public Label SetLabelState(Label label, UIDay uIDay)
        {
            if(label !=null && uIDay!=null)
            {
                if(uIDay.selectionStatus== SelectionStatus.Selected)
                {
                    label.TextColor = Color.Black;
                }
                else if (uIDay.selectionStatus == SelectionStatus.Today)
                {
                    label.TextColor = Color.FromHex("#EF4736");
                }
                else if (uIDay.selectionStatus == SelectionStatus.NotSlected)
                {
                    label.TextColor = Color.Black;
                    label.Opacity = 0.5;
                }
            }
            return label;
        }
        public View SetViewState(View view, UIDay uIDay)
        {
            if (view != null && uIDay != null)
            {
                if (uIDay.selectionStatus == SelectionStatus.Selected)
                {
                    view.BackgroundColor = Color.FromHex("#EF4736");
                }
                else if (uIDay.selectionStatus == SelectionStatus.Today)
                {
                    view.BackgroundColor = Color.FromHex("#EF4736");
                }
                else if (uIDay.selectionStatus == SelectionStatus.NotSlected)
                {
                    view.BackgroundColor = Color.FromHex("#989da0"); 
                }
            }
            return view;
        }
    }
}