using Microsoft.Maui.Controls.Platform;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

[assembly: ExportRenderer(typeof(ContentPage), typeof(MyContentPageRenderer))]
namespace Terra_Maui.Platforms.Android.Renderers
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
