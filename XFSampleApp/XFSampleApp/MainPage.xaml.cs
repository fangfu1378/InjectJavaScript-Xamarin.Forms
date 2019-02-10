using Plugin.Media;
using Plugin.Media.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using XFSampleApp.CustomeRenderers;

namespace XFSampleApp
{
    public partial class MainPage : ContentPage
    {
        private CustomWebView _customWebView;
        public MainPage()
        {
            InitializeComponent();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            PrepareCustomWebView();
        }

        private void PrepareCustomWebView()
        {
            var childrenFirstViewControl = (Content as StackLayout).Children[0];
            if (childrenFirstViewControl is CustomWebView)
            {
                _customWebView = childrenFirstViewControl as CustomWebView;
            }

            _customWebView.RegisterAction(async (data) =>
            {
                var isCrossMediaInitalized = await CrossMedia.Current.Initialize();

                MediaFile mediaFile = isCrossMediaInitalized ? await TakePictureAsync() : null;

                //Please insert handling code for your media file...
            });
        }

        private async Task<MediaFile> TakePictureAsync()
        {
            if (CrossMedia.Current.IsCameraAvailable && CrossMedia.Current.IsTakePhotoSupported)
            {
                var mediaFile = await  CrossMedia.Current.TakePhotoAsync(new StoreCameraMediaOptions
                {
                    Name = $"{Guid.NewGuid().ToString()}.jpg",
                    Directory = $"MyTest",
                    PhotoSize = PhotoSize.Custom,
                    CustomPhotoSize = 90
                });
                return mediaFile;
            }
            else
            {
                await DisplayAlert("Error", "This device doesn't support TakePicture feature...", "OK");
                return null;
            }
        }

        private void RefreshButton_Clicked(object sender, EventArgs e)
        {
            if(_customWebView != null)
                _customWebView.Reload();
        }

        protected override void OnDisappearing()
        {
            _customWebView.Cleanup();
            base.OnDisappearing();
        }
    }
}
