using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;

using Android.Widget;
using Plugin.CurrentActivity;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using XFSampleApp.CustomeRenderers;
using XFSampleApp.Droid.CustomeRenderers;
using XFSampleApp.Droid.CustomeRenderers.Helpers;
using XFSampleApp.Helpers;

[assembly: ExportRenderer(typeof(CustomWebView), typeof(CustomWebViewCustomRenderer))]
namespace XFSampleApp.Droid.CustomeRenderers
{

    public class CustomWebViewCustomRenderer : ViewRenderer<CustomWebView, Android.Webkit.WebView>, IEvalJs
    {
        const string JavascriptFunction = "function invokeCSharpAction(data){jsBridge.invokeAction(data);}";

        readonly Context _context;

        public CustomWebViewCustomRenderer(Context context) : base(context)
        {
            _context = context;
        }

        protected override void OnElementChanged(ElementChangedEventArgs<CustomWebView> e)
        {
            base.OnElementChanged(e);

            if (Control == null)
            {
                var webView = new Android.Webkit.WebView(_context);
                webView.SetWebViewClient(new JSWebViewClient($"javascript: {JavascriptFunction}"));
                webView.Settings.JavaScriptEnabled = true;              
                SetNativeControl(webView);
            }

            if (e.OldElement != null)
            {
                Control.RemoveJavascriptInterface("jsBridge");
                var customWebView = e.OldElement as CustomWebView;
                customWebView.Cleanup();
            }

            if (e.NewElement != null)
            {
                e.NewElement.EvalJsInstance = this;
                Control.AddJavascriptInterface(new JSBridge(this), "jsBridge");
                Control.LoadUrl($"{e.NewElement.Uri}");
            }
        }

        public void ExecuteJavaScript(string scriptStr)
        {
            Control?.EvaluateJavascript(scriptStr, null);
        }
    }
}