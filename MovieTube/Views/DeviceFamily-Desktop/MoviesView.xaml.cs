using Microsoft.Toolkit.Uwp.Connectivity;
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
using MovieTube.Functions.Models;
using System.Collections.ObjectModel;
using System.Net.NetworkInformation;
using System.ComponentModel;
using System.Runtime.CompilerServices;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace MovieTube.UI.Views.DeviceFamily_Desktop
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MoviesView : Page,INotifyPropertyChanged
    {
        #region Fields
        private ObservableCollection<Movie> _movies;
        private string _categoryName;
        #endregion

        public int CategoryId { get; set; }

        public string CategoryName
        {
            get { return _categoryName; }
            set { _categoryName = value; OnPropertyChanged(); }
        }

        public ObservableCollection<Movie> Movies
        {
            get { return _movies; }
            set { _movies = value; OnPropertyChanged(); }
        }

        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public MoviesView()
        {
            this.InitializeComponent();

            var goBack = new KeyboardAccelerator();
            goBack.Key = Windows.System.VirtualKey.GoBack;
            goBack.Invoked += GoBack_Invoked;

            var altleft = new KeyboardAccelerator();
            altleft.Modifiers = Windows.System.VirtualKeyModifiers.Menu;
            altleft.Key = Windows.System.VirtualKey.Left;
            altleft.Invoked += Altleft_Invoked;

            NetworkHelper.Instance.NetworkChanged += NetworkChangedEvent;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private async void NetworkChangedEvent(object sender, EventArgs e)
        {
            if (Movies == null)
            {
                if (NetworkInterface.GetIsNetworkAvailable())
                {
                    moviesLoadingNetworkIssue.IsLoading = false;

                    moviesLoadingData.IsLoading = true;

                    HttpResponse response =
                        await Functions.Repositories.RemoteRepo.CategoriesRepo.
                        GetMoviesByCategoryIdAsync(CategoryId);

                    if (response.Result != null)
                    {
                        Movies = (ObservableCollection<Movie>)response.Result;
                    }
                    else
                    {
                        moviesIconToastNotif.Glyph = (string)this.Resources["ErrorIcon"];
                        moviesTextToastNotif.Text = response.ErrorMessage;

                        moviesToastNotif.Show();
                    }

                    moviesLoadingData.IsLoading = false;
                }
            }
        }

        protected async override void OnNavigatedTo(NavigationEventArgs e)
        {
            if (e.Parameter != null)
            {
                CategoryId = int.Parse(e.Parameter.ToString());
                PageLoading();
            }
            else
            {
                await new Windows.UI.Xaml.Controls.ContentDialog()
                {
                    CloseButtonText = "Close",
                    Title = "Error",
                    Content = "You don't sent the category id from pervious page"
                }.ShowAsync();
            }
        }

        private async void PageLoading()
        {
            if (NetworkInterface.GetIsNetworkAvailable())
            {
                moviesLoadingData.IsLoading = true;

                HttpResponse response =
                    await Functions.Repositories.RemoteRepo.CategoriesRepo.
                    GetMoviesByCategoryIdAsync(CategoryId);

                if (response.Result != null)
                {
                    Movies = (ObservableCollection<Movie>)response.Result;

                    //moviesGridView.ItemsSource =
                    //    (ObservableCollection<Movie>)response.Result;

                    txtCategoryName.Text = ((Movie)moviesGridView.Items[0]).CategoryName;
                }
                else
                {
                    moviesIconToastNotif.Glyph = (string)this.Resources["ErrorIcon"];
                    moviesTextToastNotif.Text = response.ErrorMessage;

                    moviesToastNotif.Show();
                }

                moviesLoadingData.IsLoading = false;
            }
            else
            {
                moviesLoadingNetworkIssue.IsLoading = true;
            }
        }


        #region GoBack


        private void backBtn_Click(object sender, RoutedEventArgs e)
        {
            OnGoBack();
        }

        private void Altleft_Invoked(KeyboardAccelerator sender, KeyboardAcceleratorInvokedEventArgs args)
        {
            OnGoBack();
        }

        private void GoBack_Invoked(KeyboardAccelerator sender, KeyboardAcceleratorInvokedEventArgs args)
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

        private void moviesGridView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (NetworkInterface.GetIsNetworkAvailable())
            {
                this.Frame.Navigate(typeof(MovieDetailsView), 
                    ((Movie)moviesGridView.SelectedItem).Id, 
                    new Windows.UI.Xaml.Media.Animation.DrillInNavigationTransitionInfo());
            }
            else
            {
                moviesIconToastNotif.Glyph = (string)this.Resources["NetworkIssueIcon"];
                moviesTextToastNotif.Text = "You aren't connected to internet";

                moviesToastNotif.Show(6000);
            }
        }

        private void movieslistView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (NetworkInterface.GetIsNetworkAvailable())
            {
                this.Frame.Navigate(typeof(MovieDetailsView),
                    ((Movie)movieslistView.SelectedItem).Id,
                    new Windows.UI.Xaml.Media.Animation.DrillInNavigationTransitionInfo());
            }
            else
            {
                moviesIconToastNotif.Glyph = (string)this.Resources["NetworkIssueIcon"];
                moviesTextToastNotif.Text = "You aren't connected to internet";

                moviesToastNotif.Show(6000);
            }
        }
    }
}
