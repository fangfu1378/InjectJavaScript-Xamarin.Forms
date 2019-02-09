using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace XFSampleApp
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
        }

        private void RefreshButton_Clicked(object sender, EventArgs e)
        {
            View childrenFirstViewControl = (Content as StackLayout).Children[0];
            if(childrenFirstViewControl is WebView)
            {
                var webView = childrenFirstViewControl as WebView;
                webView.Reload();
            }
        }
    }
}
