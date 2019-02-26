// WARNING
//
// This file has been generated automatically by Visual Studio from the outlets and
// actions declared in your storyboard file.
// Manual changes to this file will not be maintained.
//
using Foundation;
using System;
using System.CodeDom.Compiler;
using UIKit;

namespace test_interpreter
{
    [Register ("ViewController")]
    partial class ViewController
    {
        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UITextField Columns { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UITextView outputView { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UITextField Rows { get; set; }

        [Action ("OnButtonClick:")]
        [GeneratedCode ("iOS Designer", "1.0")]
        partial void OnButtonClick (UIKit.UIButton sender);

        void ReleaseDesignerOutlets ()
        {
            if (Columns != null) {
                Columns.Dispose ();
                Columns = null;
            }

            if (outputView != null) {
                outputView.Dispose ();
                outputView = null;
            }

            if (Rows != null) {
                Rows.Dispose ();
                Rows = null;
            }
        }
    }
}