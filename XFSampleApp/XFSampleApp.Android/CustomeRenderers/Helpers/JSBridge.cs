using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Webkit;
using Android.Widget;
using Java.Interop;
using XFSampleApp.CustomeRenderers;

namespace XFSampleApp.Droid.CustomeRenderers.Helpers
{
    public class JSBridge : Java.Lang.Object
    {
        readonly WeakReference<CustomWebViewCustomRenderer> _customWebViewCustomRenderer;

        public JSBridge(CustomWebViewCustomRenderer customWebViewCustomRenderer)
        {
            _customWebViewCustomRenderer = new WeakReference<CustomWebViewCustomRenderer>(customWebViewCustomRenderer);
        }

        [JavascriptInterface]
        [Export("invokeAction")]
        public void InvokeAction(string data)
        {
            if (_customWebViewCustomRenderer != null && _customWebViewCustomRenderer.TryGetTarget(out CustomWebViewCustomRenderer customWebViewCustomRenderer))
            {
                customWebViewCustomRenderer.Element.InvokeAction(data);
            }
        }
    }
}