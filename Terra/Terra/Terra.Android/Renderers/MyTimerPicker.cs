using System;
using Android.App;
using Android.Content;
using Android.Graphics.Drawables;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Terra.Droid.Renderers;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(Xamarin.Forms.TimePicker), typeof(MyTimerPicker))]
namespace Terra.Droid.Renderers
{
    public class MyTimerPicker : TimePickerRenderer
    {
        public MyTimerPicker(Context context) : base(context)
        {

        }

        protected override void OnElementChanged(ElementChangedEventArgs<Xamarin.Forms.TimePicker> e)
        {
            base.OnElementChanged(e);

            TimePickerDialogIntervals timePickerDlg = new TimePickerDialogIntervals(this.Context, new EventHandler<TimePickerDialogIntervals.TimeSetEventArgs>(UpdateDuration),
                Element.Time.Hours, Element.Time.Minutes, true);


            var control = new EditText(this.Context);

            control.Focusable = false;
            control.FocusableInTouchMode = false;
            control.Clickable = false;
            control.Click += (sender, ea) => timePickerDlg.Show();
            control.Text = Element.Time.Hours.ToString("00") + ":" + Element.Time.Minutes.ToString("00");
            
            if (e.OldElement == null)
            {
                control.Background = null;

                var layoutParams1 = new MarginLayoutParams(Control.LayoutParameters);
                layoutParams1.SetMargins(0, 0, 0, 0);
                LayoutParameters = layoutParams1;
                control.LayoutParameters = layoutParams1;
                control.SetPadding(0, 0, 0, 0);
                SetPadding(0, 0, 0, 0);
            }
            SetNativeControl(control);
        }

        void UpdateDuration(object sender, Android.App.TimePickerDialog.TimeSetEventArgs e)
        {
            Element.Time = new TimeSpan(e.HourOfDay, e.Minute, 0);
            Control.Text = Element.Time.Hours.ToString("00") + ":" + Element.Time.Minutes.ToString("00");
            var gd = new GradientDrawable();
            gd.SetStroke(0, Android.Graphics.Color.Transparent);
            Control.SetBackground(gd);
        }
    }

    public class TimePickerDialogIntervals : TimePickerDialog
    {
        public const int TimePickerInterval = 15;
        private bool _ignoreEvent = false;

        public TimePickerDialogIntervals(Context context, EventHandler<TimePickerDialog.TimeSetEventArgs> callBack, int hourOfDay, int minute, bool is24HourView)
            : base(context, TimePickerDialog.ThemeHoloLight, (sender, e) =>
            {
                callBack(sender, new TimePickerDialog.TimeSetEventArgs(e.HourOfDay, e.Minute * 5));
            }, hourOfDay, minute / TimePickerInterval, is24HourView)
        {

        }

        protected TimePickerDialogIntervals(IntPtr javaReference, JniHandleOwnership transfer) : base(javaReference, transfer)
        {
        }

        public override void SetView(Android.Views.View view)
        {
            SetupMinutePicker(view);
            base.SetView(view);
        }

        void SetupMinutePicker(Android.Views.View view)
        {
            var numberPicker = FindMinuteNumberPicker(view as ViewGroup);
            if (numberPicker != null)
            {
                numberPicker.MinValue = 0;
                numberPicker.MaxValue = 11;
                numberPicker.SetDisplayedValues(new String[] { "00", "05", "10", "15", "20", "25", "30", "35", "40", "45", "50", "55" });
            }
        }

        protected override void OnCreate(Android.OS.Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            GetButton((int)DialogButtonType.Negative).Visibility = Android.Views.ViewStates.Gone;
            this.SetCanceledOnTouchOutside(false);

        }

        private NumberPicker FindMinuteNumberPicker(ViewGroup viewGroup)
        {
            for (var i = 0; i < viewGroup.ChildCount; i++)
            {
                var child = viewGroup.GetChildAt(i);
                var numberPicker = child as NumberPicker;
                if (numberPicker != null)
                {
                    if (numberPicker.MaxValue == 59)
                    {
                        return numberPicker;
                    }
                }

                var childViewGroup = child as ViewGroup;
                if (childViewGroup != null)
                {
                    var childResult = FindMinuteNumberPicker(childViewGroup);
                    if (childResult != null)
                        return childResult;
                }
            }

            return null;
        }


    }
}
