using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace Terra.Core.Helper
{
   public class AnimationHelper
    {
        static readonly AnimationHelper animation = new AnimationHelper();
        public static AnimationHelper Instance
        {
            get
            {
                return animation;
            }
        }

        
        private AnimationHelper() { }
        public async void AnimationInvisible(StackLayout InputLayout,double startHeight)
        {
            var layout = InputLayout;

            // setup information for animation
            Action<double> callback = input => { layout.HeightRequest = input; }; // update the height of the layout with this callback

            double startingHeight = layout.Height; // the layout's height when we begin animation
            double endingHeight = 0; // final desired height of the layout
            uint rate = 16; // pace at which aniation proceeds
            uint length = 500; // one second animation
            Easing easing = Easing.Linear; // There are a couple easing types, just tried this one for effect

            // now start animation with all the setup information
            layout.Animate("invis", callback, startingHeight, endingHeight, rate, length, easing);
            //layout.IsVisible = false;
            layout.FadeTo(0, 400);
        }
        public async void AnimationVisible(StackLayout InputLayout, double startHeight)
        {
            var layout = InputLayout;

            // setup information for animation
            Action<double> callback = input => { layout.HeightRequest = startHeight; }; // update the height of the layout with this callback
            double startingHeight = 0; // the layout's height when we begin animation
            double endingHeight = startHeight; // final desired height of the layout
            uint rate = 16; // pace at which aniation proceeds
            uint length = 500; // one second animation
            Easing easing = Easing.Linear; // There are a couple easing types, just tried this one for effect

            // now start animation with all the setup information
            layout.Animate("vis", callback, startingHeight, endingHeight, rate, length, easing);
            //layout.IsVisible = true;
            layout.FadeTo(1, 500);
        }
    }
}
