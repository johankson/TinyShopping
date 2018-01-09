// WARNING
//
// This file has been generated automatically by Visual Studio from the outlets and
// actions declared in your storyboard file.
// Manual changes to this file will not be maintained.
//
using Foundation;
using System;
using System.CodeDom.Compiler;

namespace TodoSummary
{
    [Register ("TodayViewController")]
    partial class TodayViewController
    {
        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel lblSummary { get; set; }

        void ReleaseDesignerOutlets ()
        {
            if (lblSummary != null) {
                lblSummary.Dispose ();
                lblSummary = null;
            }
        }
    }
}