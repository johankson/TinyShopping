using System;
using TinyShopping.iOS.Renderers;
using TinyShopping.Views;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(ContentPage),typeof(TilePageRenderer))]

namespace TinyShopping.iOS.Renderers
{
    public class TilePageRenderer : PageRenderer
    {
        public override void WillMoveToParentViewController(UIKit.UIViewController parent)
        {
            base.WillMoveToParentViewController(parent);
            if (Element is ICustomTitleView titleViewContainer)
            {
                var renderer = Platform.CreateRenderer(titleViewContainer.TitleView);
                var view = renderer.NativeView;
                view.SizeToFit();
                //parent.NavigationItem.view

               // parent.NavigationItem.TitleView = view.Subviews[0];
            }
        }

        public override void ViewWillAppear(bool animated)
        {
            base.ViewWillAppear(animated);

            if (UIDevice.CurrentDevice.CheckSystemVersion(11, 0) && NavigationController != null && NavigationController.NavigationBar != null)
            {
                NavigationController.NavigationBar.PrefersLargeTitles = true;
            }
        }

        public override void ViewDidDisappear(bool animated)
        {
            base.ViewDidDisappear(animated);

            if (UIDevice.CurrentDevice.CheckSystemVersion(11, 0) && NavigationController != null && NavigationController.NavigationBar != null)
            {
                NavigationController.NavigationBar.PrefersLargeTitles = false;
            }
        }
    }
}
