using System;
using Terra.Droid.Renderers;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(ContentPage), typeof(MyContentPageRenderer))]
namespace Terra.Droid.Renderers
{
    public class MyContentPageRenderer : PageRenderer
    {
        public MyContentPageRenderer()
        {
        }


        protected override void OnElementChanged(ElementChangedEventArgs<Page> e)
        {
            base.OnElementChanged(e);


        }

        protected override void OnElementPropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            base.OnElementPropertyChanged(sender, e);


        }
    }
}
