using System;
using System.Collections.Generic;
using System.Linq;

using Foundation;
using UIKit;

namespace TinyShopping.iOS
{
    [Register("AppDelegate")]
    public partial class AppDelegate : global::Xamarin.Forms.Platform.iOS.FormsApplicationDelegate
    {
        public override bool FinishedLaunching(UIApplication app, NSDictionary options)
        {

            global::Xamarin.Forms.Forms.Init();

            LoadApplication(new App());



            TinyPubSubLib.TinyPubSub.Subscribe<Core.Services.SummaryData>("SummaryData", (Core.Services.SummaryData data) =>
            {
                var shared = new NSUserDefaults(
                    "se.tinystuff.TinyShopping.shared",
                    NSUserDefaultsType.SuiteName);

                shared.SetInt(data.TotalItems, "total");
                shared.SetInt(data.DoneItems, "done");
               // shared.Synchronize();
            });

            return base.FinishedLaunching(app, options);
        }

        public override void WillEnterForeground(UIApplication uiApplication)
        {
            base.WillEnterForeground(uiApplication);
        }
    }
}
