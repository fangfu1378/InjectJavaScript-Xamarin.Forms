using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Foundation;
using UIKit;
using WebKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;
using XFSampleApp.CustomeRenderers;
using XFSampleApp.Helpers;
using XFSampleApp.iOS.CustomeRenderers;

[assembly: ExportRenderer(typeof(CustomWebView), typeof(CustomWebViewCustomRenderer))]
namespace XFSampleApp.iOS.CustomeRenderers
{
    public class CustomWebViewCustomRenderer : ViewRenderer<CustomWebView, WKWebView>, IWKScriptMessageHandler, IEvalJs
    {
        const string JavaScriptFunction = "function invokeCSharpAction(data){window.webkit.messageHandlers.invokeAction.postMessage(data);}";

        WKUserContentController userController;

        protected override void OnElementChanged(ElementChangedEventArgs<CustomWebView> e)
        {
            base.OnElementChanged(e);

            if (Control == null)
            {
                userController = new WKUserContentController();
                var script = new WKUserScript(new NSString(JavaScriptFunction), WKUserScriptInjectionTime.AtDocumentEnd, false);
                userController.AddUserScript(script);
                userController.AddScriptMessageHandler(this, "invokeAction");

                var config = new WKWebViewConfiguration { UserContentController = userController };
                var webView = new WKWebView(Frame, config);
                SetNativeControl(webView);
            }

            if (e.OldElement != null)
            {
                userController.RemoveAllUserScripts();
                userController.RemoveScriptMessageHandler("invokeAction");

                var customWebView = e.OldElement as CustomWebView;
                customWebView.Cleanup();
            }

            if (e.NewElement != null)
            {
                System.Diagnostics.Debug.WriteLine(Element.Uri);
                Control.LoadRequest(new NSUrlRequest(new NSUrl(Element.Uri)));
            }
        }

        public void DidReceiveScriptMessage(WKUserContentController userContentController, WKScriptMessage message)
        {
            Element.InvokeAction(message.Body.ToString());
        }

        public void ExecuteJavaScript(string scriptStr)
        {
            Control?.EvaluateJavaScript(new NSString(scriptStr), null);
        }
    }
}