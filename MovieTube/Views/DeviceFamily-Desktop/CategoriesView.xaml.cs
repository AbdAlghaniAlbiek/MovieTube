using Microsoft.Toolkit.Uwp.Connectivity;
using MovieTube.Functions.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using MovieTube.Functions;
using Microsoft.Toolkit.Uwp.Helpers;
using System.Net.NetworkInformation;
using System.ComponentModel;
using System.Runtime.CompilerServices;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace MovieTube.UI.Views.DeviceFamily_Desktop
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class CategoriesView : Page,INotifyPropertyChanged
    {

        #region Fields
        private ObservableCollection<Movie> _bestMovies;
        private ObservableCollection<Category> _categories;
        private SolidColorBrush _transparentFill =
            new SolidColorBrush() { Color = Windows.UI.Colors.Transparent };
        #endregion

        private SolidColorBrush TransparentFill {  get => _transparentFill; }

        public ObservableCollection<Movie> BestMovies
        {
            get { return _bestMovies; }
            set { _bestMovies = value; OnPropertyChanged(); }
        }

        public ObservableCollection<Category> Categories
        {
            get { return _categories; }
            set { _categories = value; OnPropertyChanged(); }
        }

        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public CategoriesView()
        {
            this.InitializeComponent();

            NetworkHelper.Instance.NetworkChanged += NetworkChangedEvent;

            var goBack = new KeyboardAccelerator();
            goBack.Key = Windows.System.VirtualKey.GoBack;
            goBack.Invoked += GoBack_Invoked;

            var altLeft = new KeyboardAccelerator();
            altLeft.Key = Windows.System.VirtualKey.Left;
            altLeft.Modifiers = Windows.System.VirtualKeyModifiers.Menu;
            altLeft.Invoked += AltLeft_Invoked;
        }

        private async void NetworkChangedEvent(object sender, EventArgs e)
        {
            if (Categories == null && BestMovies == null)
            {
                if (NetworkInterface.GetIsNetworkAvailable())
                {
                    categoriesLoadingNetworkIssue.IsLoading = false;

                    categoriesLoadingData.IsLoading = true;

                    HttpResponse categoriesResponse =
                        await Functions.Repositories.RemoteRepo.CategoriesRepo.GetCategoriesAsync();

                    HttpResponse bestMoviesResponse =
                      await Functions.Repositories.RemoteRepo.MoviesRepo.GetBestMoviesAsync();

                    //Check the result from categoriesReponse
                    if (categoriesResponse.Result != null)
                    {
                        Categories =
                            (ObservableCollection<Category>)categoriesResponse.Result;
                    }
                    else
                    {
                        categoriesIconToastNotif.Glyph = (string)this.Resources["ErrorIcon"];
                        categoriesTextToastNotif.Text = categoriesResponse.ErrorMessage;

                        categoriesToastNotif.Show();
                    }

                    //Check the result from bestMoviesResponse
                    if (bestMoviesResponse.Result != null)
                    {
                        BestMovies =
                            (ObservableCollection<Movie>)bestMoviesResponse.Result;
                    }
                    else
                    {
                        categoriesIconToastNotif.Glyph = (string)this.Resources["ErrorIcon"];
                        categoriesTextToastNotif.Text = bestMoviesResponse.ErrorMessage;

                        categoriesToastNotif.Show();
                    }

                    categoriesLoadingData.IsLoading = false;
                }
            }
        }

        private async void Page_Loading(FrameworkElement sender, object args)
        {  
            if (NetworkInterface.GetIsNetworkAvailable())
            {
                categoriesLoadingData.IsLoading = true;

                HttpResponse categoriesResponse =
                    await Functions.Repositories.RemoteRepo.CategoriesRepo.GetCategoriesAsync();

                HttpResponse bestMoviesResponse =
                  await Functions.Repositories.RemoteRepo.MoviesRepo.GetBestMoviesAsync();

                //Check the result from categoriesResponse
                //Check the result from bestMoviesResponse
                if (categoriesResponse.Result != null && bestMoviesResponse.Result != null)
                {
                    Categories =
                        (ObservableCollection<Category>)categoriesResponse.Result;

                    BestMovies =
                       (ObservableCollection<Movie>)bestMoviesResponse.Result;

                    //categoriesDesktopState.ItemsSource =
                    //    (ObservableCollection<Category>)categoriesResponse.Result;

                    //categoriesMobileState.ItemsSource =
                    //   (ObservableCollection<Category>)categoriesResponse.Result;

                    //bestMoviesFlipView.ItemsSource =
                    //    (ObservableCollection<Movie>)bestMoviesResponse.Result;

                    //This method make flipview flips its items automatically
                    MakeBestMoviesFliperTimer();
                }
                else
                {
                    categoriesIconToastNotif.Glyph = (string)this.Resources["ErrorIcon"];
                    categoriesTextToastNotif.Text = categoriesResponse.ErrorMessage;

                    categoriesToastNotif.Show();
                }

                categoriesLoadingData.IsLoading = false;
            }
            else
            {
                categoriesLoadingNetworkIssue.IsLoading = true;
            }
        }

        private void MakeBestMoviesFliperTimer()
        {
            DispatcherTimer bestMoviesFliperTimer = new DispatcherTimer();
            bestMoviesFliperTimer.Interval = TimeSpan.FromMilliseconds(6000);
            bestMoviesFliperTimer.Tick += BestMoviesFliperTimer_Tick;
        }

        private void BestMoviesFliperTimer_Tick(object sender, object e)
        {
            if (bestMoviesFlipView.SelectedIndex == 4)
            {
                bestMoviesFlipView.SelectedIndex = 0;
                firstMovie.Fill = (SolidColorBrush)this.Resources["EllipseFill"];
                secondMovie.Fill = thirdMovie.Fill = fourthMovie.Fill = fifthMovie.Fill = TransparentFill;
            }
            else
            {
                bestMoviesFlipView.SelectedIndex += 1;

                if (bestMoviesFlipView.SelectedIndex == 1)
                {
                    secondMovie.Fill = (SolidColorBrush)this.Resources["EllipseFill"];
                    firstMovie.Fill = thirdMovie.Fill = fourthMovie.Fill = fifthMovie.Fill = TransparentFill;
                }
                else if (bestMoviesFlipView.SelectedIndex == 2)
                {
                    firstMovie.Fill = secondMovie.Fill = fourthMovie.Fill = fifthMovie.Fill = TransparentFill;
                    thirdMovie.Fill = (SolidColorBrush)this.Resources["EllipseFill"];
                }
                else if (bestMoviesFlipView.SelectedIndex == 3)
                {
                    firstMovie.Fill = thirdMovie.Fill = secondMovie.Fill = fifthMovie.Fill = TransparentFill;
                    fourthMovie.Fill = (SolidColorBrush)this.Resources["EllipseFill"];
                }
                else if (bestMoviesFlipView.SelectedIndex == 4)
                {
                    firstMovie.Fill = secondMovie.Fill = thirdMovie.Fill = fourthMovie.Fill = TransparentFill;
                    fifthMovie.Fill = (SolidColorBrush)this.Resources["EllipseFill"];
                }
            }
        }

        private void categoriesMobileState_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (NetworkInterface.GetIsNetworkAvailable())
            {
                this.Frame.Navigate(
                    typeof(MoviesView), 
                    ((Category)categoriesMobileState.SelectedItem).Id,
                    new Windows.UI.Xaml.Media.Animation.DrillInNavigationTransitionInfo());
            }
            else
            {
                categoriesIconToastNotif.Glyph = (string)this.Resources["NetworkIssueIcon"];
                categoriesTextToastNotif.Text = "You aren't connected to internet";

                categoriesToastNotif.Show(6000);
            }
        }

        private void categoriesDesktopState_SelectionChanged(object sender, SelectionChangedEventArgs e)
        { 
            if (NetworkInterface.GetIsNetworkAvailable())
            {
                this.Frame.Navigate(typeof(MoviesView),
                    ((Category)categoriesDesktopState.SelectedItem).Id, 
                    new Windows.UI.Xaml.Media.Animation.DrillInNavigationTransitionInfo());
            }
            else
            {
                categoriesIconToastNotif.Glyph = (string)this.Resources["NetworkIssueIcon"];
                categoriesTextToastNotif.Text = "you aren't connected to internet";

                categoriesToastNotif.Show(6000);
            }
        }

        #region FlipViewButtonTappedEvents

        private void backBesMovBtn_Click(object sender, RoutedEventArgs e)
        {
            if (bestMoviesFlipView.SelectedIndex == 0)
            {
                bestMoviesFlipView.SelectedIndex = 4;
                firstMovie.Fill = secondMovie.Fill = thirdMovie.Fill = fourthMovie.Fill = TransparentFill;
                fifthMovie.Fill = (SolidColorBrush)this.Resources["EllipseFill"];
            }
            else if(bestMoviesFlipView.SelectedIndex == 1)
            {
                bestMoviesFlipView.SelectedIndex -= 1;
                fifthMovie.Fill = secondMovie.Fill = thirdMovie.Fill = fourthMovie.Fill = TransparentFill;
                firstMovie.Fill = (SolidColorBrush)this.Resources["EllipseFill"]; 
            }
            else if (bestMoviesFlipView.SelectedIndex == 2)
            {
                bestMoviesFlipView.SelectedIndex -= 1;
                fifthMovie.Fill = firstMovie.Fill = thirdMovie.Fill = fourthMovie.Fill = TransparentFill;
                secondMovie.Fill = (SolidColorBrush)this.Resources["EllipseFill"]; 
            }
            else if (bestMoviesFlipView.SelectedIndex == 3)
            {
                bestMoviesFlipView.SelectedIndex -= 1;
                fifthMovie.Fill = secondMovie.Fill = fourthMovie.Fill = firstMovie.Fill = TransparentFill;
                thirdMovie.Fill = (SolidColorBrush)this.Resources["EllipseFill"]; 
            }
            else if (bestMoviesFlipView.SelectedIndex == 4)
            {
                bestMoviesFlipView.SelectedIndex -= 1;
                fifthMovie.Fill = secondMovie.Fill = thirdMovie.Fill = firstMovie.Fill = TransparentFill;
                fourthMovie.Fill = (SolidColorBrush)this.Resources["EllipseFill"];
            }
            else
            {
                bestMoviesFlipView.SelectedIndex -= 1;
            }
        }

        private void forwarBesMovBtn_Click(object sender, RoutedEventArgs e)
        {
            if (bestMoviesFlipView.SelectedIndex == 4)
            {
                bestMoviesFlipView.SelectedIndex = 0;
                firstMovie.Fill = (SolidColorBrush)this.Resources["EllipseFill"];
                secondMovie.Fill = thirdMovie.Fill = fourthMovie.Fill = fifthMovie.Fill = TransparentFill;
            }
            else if (bestMoviesFlipView.SelectedIndex == 0)
            {
                bestMoviesFlipView.SelectedIndex += 1;
                fifthMovie.Fill = firstMovie.Fill = thirdMovie.Fill = fourthMovie.Fill = TransparentFill;
                secondMovie.Fill = (SolidColorBrush)this.Resources["EllipseFill"]; 
            }
            else if (bestMoviesFlipView.SelectedIndex == 1)
            {
                bestMoviesFlipView.SelectedIndex += 1;
                fifthMovie.Fill = firstMovie.Fill = secondMovie.Fill = fourthMovie.Fill = TransparentFill;
                thirdMovie.Fill = (SolidColorBrush)this.Resources["EllipseFill"]; 
            }
            else if (bestMoviesFlipView.SelectedIndex == 2)
            {
                bestMoviesFlipView.SelectedIndex += 1;
                fifthMovie.Fill = secondMovie.Fill = thirdMovie.Fill = firstMovie.Fill = TransparentFill;
                fourthMovie.Fill = (SolidColorBrush)this.Resources["EllipseFill"]; 
            }
            else if (bestMoviesFlipView.SelectedIndex == 3)
            {
                bestMoviesFlipView.SelectedIndex += 1;
                fourthMovie.Fill = secondMovie.Fill = thirdMovie.Fill = firstMovie.Fill = TransparentFill;
                fifthMovie.Fill = (SolidColorBrush)this.Resources["EllipseFill"]; 
            }
            else
            {
                bestMoviesFlipView.SelectedIndex += 1;
            }
        }

        private void firstMovie_Tapped(object sender, TappedRoutedEventArgs e)
        {
            bestMoviesFlipView.SelectedIndex = 0;
            firstMovie.Fill = (SolidColorBrush)this.Resources["EllipseFill"];
            secondMovie.Fill = thirdMovie.Fill = fourthMovie.Fill = fifthMovie.Fill = TransparentFill;
        }

        private void secondMovie_Tapped(object sender, TappedRoutedEventArgs e)
        {
            bestMoviesFlipView.SelectedIndex = 1;
            secondMovie.Fill = (SolidColorBrush)this.Resources["EllipseFill"];
            firstMovie.Fill = thirdMovie.Fill = fourthMovie.Fill = fifthMovie.Fill = TransparentFill;
        }

        private void thirdMovie_Tapped(object sender, TappedRoutedEventArgs e)
        {
            bestMoviesFlipView.SelectedIndex = 2;
            firstMovie.Fill = secondMovie.Fill = fourthMovie.Fill = fifthMovie.Fill = TransparentFill;
            thirdMovie.Fill = (SolidColorBrush)this.Resources["EllipseFill"];
        }

        private void fourthMovie_Tapped(object sender, TappedRoutedEventArgs e)
        {
            bestMoviesFlipView.SelectedIndex = 3;
            firstMovie.Fill = thirdMovie.Fill = secondMovie.Fill = fifthMovie.Fill = TransparentFill;
            fourthMovie.Fill = (SolidColorBrush)this.Resources["EllipseFill"];
        }

        private void fifthMovie_Tapped(object sender, TappedRoutedEventArgs e)
        {
            bestMoviesFlipView.SelectedIndex = 4;
            firstMovie.Fill = secondMovie.Fill = thirdMovie.Fill = fourthMovie.Fill = TransparentFill;
            fifthMovie.Fill = (SolidColorBrush)this.Resources["EllipseFill"];
        }

        #endregion

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
    }
}
