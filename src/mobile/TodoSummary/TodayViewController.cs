﻿using System;

using NotificationCenter;
using Foundation;
using UIKit;
using CoreGraphics;

namespace TodoSummary
{
    public partial class TodayViewController : UIViewController, INCWidgetProviding
    {
        protected TodayViewController(IntPtr handle) : base(handle)
        {
            // Note: this .ctor should not contain any initialization logic

        }

        public override void DidReceiveMemoryWarning()
        {
            // Releases the view if it doesn't have a superview.
            base.DidReceiveMemoryWarning();

            // Release any cached data, images, etc that aren't in use.
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            lblSummary.Text = "Laddar...";
            // Do any additional setup after loading the view.
        }

        [Export("widgetPerformUpdateWithCompletionHandler:")]
        public void WidgetPerformUpdate(Action<NCUpdateResult> completionHandler)
        {
            // Perform any setup necessary in order to update the view.

            // If an error is encoutered, use NCUpdateResultFailed
            // If there's no update required, use NCUpdateResultNoData
            // If there's an update, use NCUpdateResultNewData


            var shared = new NSUserDefaults(
                    "se.tinystuff.TinyShopping.shared",
                    NSUserDefaultsType.SuiteName);
            //shared.Synchronize();
            var total = shared.IntForKey("total");
            var done = shared.IntForKey("done");
            lblSummary.Text = $"{done} / {total} items is done";

            completionHandler(NCUpdateResult.NewData);
        }
    }
}
