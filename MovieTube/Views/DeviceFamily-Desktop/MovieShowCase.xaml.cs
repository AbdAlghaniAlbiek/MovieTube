using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Microsoft.Toolkit.Uwp.UI.Animations;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Collections.ObjectModel;
using Windows.Media.Core;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace MovieTube.UI.Views.DeviceFamily_Desktop
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MovieShowCase : Page,INotifyPropertyChanged
    {

        #region Fields
        private ObservableCollection<string> _videoParame;
        #endregion

        public ObservableCollection<string> VideoParams
        {
            get { return _videoParame; }
            set { _videoParame = value; OnPropertyChanged(); }
        }


        private void OnPropertyChanged([CallerMemberName]string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public MovieShowCase()
        {
            this.InitializeComponent();

            var goBack = new KeyboardAccelerator();
            goBack.Key = Windows.System.VirtualKey.GoBack;
            goBack.Invoked += GoBack_Invoked;

            var altLeft = new KeyboardAccelerator();
            altLeft.Key = Windows.System.VirtualKey.Left;
            altLeft.Modifiers = Windows.System.VirtualKeyModifiers.Menu;
            altLeft.Invoked += AltLeft_Invoked;
        }

        protected async override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            var videoParameters = e.Parameter as ObservableCollection<string>;

            if (e.Parameter != null)
            {
                VideoParams = videoParameters;
                PageLoading();
            }
            else
            {
                await new Windows.UI.Xaml.Controls.ContentDialog()
                {
                    Title = "Error!!!",
                    Content = "You don't passed parameters to this page",
                    CloseButtonText = "Close"
                }.ShowAsync();
            }
        }

        private void PageLoading()
        {
            System.Uri manifestUri = null;

            if (VideoParams[1] == "movie")
            {
                 manifestUri = 
                    new Uri(MovieTube.Functions.Common.MOVIES + VideoParams[0]);
            }
            else
            {
                 manifestUri =
                     new Uri(MovieTube.Functions.Common.TRAILERS + VideoParams[0]);
            }

            mediaMovie.Source = MediaSource.CreateFromUri(manifestUri);
            mediaMovie.MediaPlayer.Play();
        }

        #region GoBack

        private void AltLeft_Invoked(KeyboardAccelerator sender, KeyboardAcceleratorInvokedEventArgs args)
        {
            OnGoBack();
        }

        private void GoBack_Invoked(KeyboardAccelerator sender, KeyboardAcceleratorInvokedEventArgs args)
        {
            OnGoBack();
        }

        private void backBtn_Click(object sender, RoutedEventArgs e)
        {
            OnGoBack();
        }

        private void OnGoBack()
        {
            if (this.Frame.CanGoBack)
            {
                this.Frame.GoBack();
            }
        }

        #endregion

        private async void mainGrid_PointerMoved(object sender, PointerRoutedEventArgs e)
        {
            var pointerPosition = e.GetCurrentPoint(mainGrid);

            if (pointerPosition.Position.Y > Window.Current.Bounds.Y - 20)
            {
                await backBtn.Offset(offsetX: 1f, offsetY: 4f, duration: 2000, delay: 0).StartAsync();
            }
            else
            {
                await backBtn.Offset(offsetX: 1f, offsetY: -4f, duration: 2000, delay: 0).StartAsync();
            }
        }
    }
}
