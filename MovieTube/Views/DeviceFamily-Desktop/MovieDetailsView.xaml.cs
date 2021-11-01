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
using Microsoft.Toolkit.Uwp.Connectivity;
using System.Net.NetworkInformation;
using MovieTube.Functions.Repositories;
using Microsoft.Toolkit.Uwp.UI.Animations;
using Newtonsoft.Json;
using Windows.UI.Xaml.Media.Imaging;
using System.ComponentModel;
using System.Runtime.CompilerServices;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace MovieTube.UI.Views.DeviceFamily_Desktop
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MovieDetailsView : Page,INotifyPropertyChanged
    {
        #region Fields
        private int _userFavourite;
        private int _userLiked;
        private int _userDisliked;
        private MovieDetails _movieDetailsData;
        private ObservableCollection<string> _movieImages;
        private ObservableCollection<Actor> _movieActors;
        private ObservableCollection<UserComment> _usersComments;
        private UsersFeedback _usersFeedbackData;
        #endregion

        public static string Result { get; set; } = "";
        public int MovieId { get; set; }
        public MovieDetails MovieDetailsData
        {
            get { return _movieDetailsData; }
            set { _movieDetailsData = value; OnPropertyChanged(); }
        }
        public ObservableCollection<string> MovieImages
        {
            get { return _movieImages; }
            set { _movieImages = value; OnPropertyChanged(); }
        }
        public ObservableCollection<Actor> MovieActors
        {
            get { return _movieActors; }
            set { _movieActors = value; OnPropertyChanged(); }
        }
        public ObservableCollection<UserComment> UsersComments
        {
            get { return _usersComments; }
            set { _usersComments = value; OnPropertyChanged(); }
        }
        public UsersFeedback UsersFeedbackData
        {
            get { return _usersFeedbackData; }
            set { _usersFeedbackData = value; OnPropertyChanged(); }
        }
        public int UserFavourite
        {
            get { return _userFavourite; }
            set { _userFavourite = value; OnPropertyChanged(); }
        }
        public int UserLiked
        {
            get { return _userLiked; }
            set { _userLiked = value; OnPropertyChanged(); }
        }
        public int UserDisliked
        {
            get { return _userDisliked; }
            set { _userDisliked = value; OnPropertyChanged(); }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public MovieDetailsView()
        {
            this.InitializeComponent();

            var goBack = new KeyboardAccelerator();
            goBack.Key = Windows.System.VirtualKey.GoBack;
            goBack.Invoked += GoBack_Invoked;

            var altleft = new KeyboardAccelerator();
            altleft.Modifiers = Windows.System.VirtualKeyModifiers.Menu;
            altleft.Key = Windows.System.VirtualKey.Left;
            altleft.Invoked += Altleft_Invoked;

            NetworkHelper.Instance.NetworkChanged += NetworkChanged;
        }

        public void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }


        protected async override void OnNavigatedTo(NavigationEventArgs e)
        {
            if (e.Parameter != null)
            {
                MovieId = int.Parse(e.Parameter.ToString());
                PageLoading();
            }
            else
            {
                await new Windows.UI.Xaml.Controls.ContentDialog()
                {
                    CloseButtonText = "Close",
                    Title = "Error",
                    Content = "You don't sent the movie id from pervious page"
                }.ShowAsync();
            }
        }

        private async void PageLoading()
        {
            if (NetworkInterface.GetIsNetworkAvailable())
            {
                movieDetailsLoadingData.IsLoading = true;

                HttpResponse movieDetailsResponse =
                    await Functions.Repositories.RemoteRepo.MoviesRepo.GetMovieDetailsAsync(MovieId);

                HttpResponse movieImagesResponse =
                    await Functions.Repositories.RemoteRepo.MoviesRepo.GetMovieImages(MovieId);

                HttpResponse movieActorsResponse =
                    await Functions.Repositories.RemoteRepo.MoviesRepo.GetMovieActorsAsync(MovieId);

                HttpResponse UsersCommentsResponse =
                    await Functions.Repositories.RemoteRepo.MoviesRepo.GetMovieCommentsAsync(MovieId);

                HttpResponse UsersFeedbackDataResponse =
                      await Functions.Repositories.RemoteRepo.MoviesRepo.GetMovieCountCommentsLikesRatingsAsync(MovieId);

                HttpResponse userFavouriteResponse =
                    await Functions.Repositories.RemoteRepo.FavouritesRepo.GetUserFavouriteIdAsync(Functions.Common.User.Id, MovieId);

                HttpResponse userLikedResponse =
                    await Functions.Repositories.RemoteRepo.UsersRepo.GetLikeAsync(MovieId, Functions.Common.User.Id);

                if (movieDetailsResponse.Result != null &&
                    movieImagesResponse.Result != null &&
                    movieActorsResponse.Result != null &&
                    UsersCommentsResponse.Result != null &&
                    UsersFeedbackDataResponse.Result != null &&
                    userFavouriteResponse.Result != null &&
                    userLikedResponse.Result != null)
                {
                    MovieDetailsData =
                      (MovieDetails)movieDetailsResponse.Result;

                    MovieImages =
                      (ObservableCollection<string>)movieImagesResponse.Result;

                    MovieActors =
                      (ObservableCollection<Actor>)movieActorsResponse.Result;

                    UsersComments =
                      (ObservableCollection<UserComment>)UsersCommentsResponse.Result;

                    UsersFeedbackData =
                        (UsersFeedback)UsersFeedbackDataResponse.Result;

                    UserFavourite =
                        int.Parse(userFavouriteResponse.Result.ToString());

                    UserLiked =
                        int.Parse(userLikedResponse.Result.ToString()) == 1 ? 1 : 0;

                    UserDisliked =
                        int.Parse(userLikedResponse.Result.ToString()) == 2 ? 2 : 0;
                }
                else
                {
                    movieDetailsLoadingData.IsLoading = false;

                    //I used the error message from movieDetails because all ErrorMessages from (movieImages, MovieActors,...) they have the same error message
                    movieDetailsIconToastNotif.Glyph = (string)this.Resources["ErrorIcon"];
                    movieDetailsTextToastNotif.Text = movieDetailsResponse.ErrorMessage;

                    movieDetailsToastNotif.Show(6000);
                }

                movieDetailsLoadingData.IsLoading = false;
            }
            else
            {
                movieDetailsLoadingNetworkIssue.IsLoading = true;
            }
        }

        private async void NetworkChanged(object sender, EventArgs e)
        {
            if (MovieDetailsData == null ||
                MovieImages == null ||
                MovieActors == null ||
                UsersComments == null ||
                UsersFeedbackData == null)
            {
                if (NetworkInterface.GetIsNetworkAvailable())
                {
                    movieDetailsLoadingNetworkIssue.IsLoading = false;

                    movieDetailsLoadingData.IsLoading = true;

                    HttpResponse movieDetailsResponse =
                        await Functions.Repositories.RemoteRepo.MoviesRepo.GetMovieDetailsAsync(MovieId);

                    HttpResponse movieImagesResponse =
                        await Functions.Repositories.RemoteRepo.MoviesRepo.GetMovieImages(MovieId);

                    HttpResponse movieActorsResponse =
                        await Functions.Repositories.RemoteRepo.MoviesRepo.GetMovieActorsAsync(MovieId);

                    HttpResponse UsersCommentsResponse =
                        await Functions.Repositories.RemoteRepo.MoviesRepo.GetMovieCommentsAsync(MovieId);

                    HttpResponse UsersFeedbackDataResponse =
                          await Functions.Repositories.RemoteRepo.MoviesRepo.GetMovieCountCommentsLikesRatingsAsync(MovieId);

                    HttpResponse userFavouriteResponse =
                    await Functions.Repositories.RemoteRepo.FavouritesRepo.GetUserFavouriteIdAsync(Functions.Common.User.Id, MovieId);

                    HttpResponse userLikedResponse =
                        await Functions.Repositories.RemoteRepo.UsersRepo.GetLikeAsync(MovieId, Functions.Common.User.Id);

                    if (movieDetailsResponse.Result != null &&
                        movieImagesResponse.Result != null &&
                        movieActorsResponse.Result != null &&
                        UsersCommentsResponse.Result != null &&
                        UsersFeedbackDataResponse.Result != null &&
                        userFavouriteResponse != null &&
                        userLikedResponse != null)
                    {
                        MovieDetailsData =
                          (MovieDetails)movieDetailsResponse.Result;

                        MovieImages =
                          (ObservableCollection<string>)movieImagesResponse.Result;

                        MovieActors =
                          (ObservableCollection<Actor>)movieActorsResponse.Result;

                        UsersComments =
                          (ObservableCollection<UserComment>)UsersCommentsResponse.Result;

                        UsersFeedbackData =
                            (UsersFeedback)UsersFeedbackDataResponse.Result;

                        UserFavourite =
                            (int)userFavouriteResponse.Result;

                        UserLiked =
                            (int)userLikedResponse.Result == 1 ? 1 : 0;

                        UserDisliked =
                            (int)userLikedResponse.Result == 2 ? 2 : 0;
                    }
                    else
                    {
                        movieDetailsLoadingData.IsLoading = false;

                        //I used the error message from movieDetails because all ErrorMessages from (movieImages, MovieActors,...) they have the same error message
                        movieDetailsIconToastNotif.Glyph = (string)this.Resources["ErrorIcon"];
                        movieDetailsTextToastNotif.Text = movieDetailsResponse.ErrorMessage;

                        movieDetailsToastNotif.Show(6000);
                    }

                    movieDetailsLoadingData.IsLoading = false;
                }
                else
                {
                    movieDetailsLoadingNetworkIssue.IsLoading = true;
                }
            }
        }


        #region GoBack

        private void Altleft_Invoked(KeyboardAccelerator sender, KeyboardAcceleratorInvokedEventArgs args)
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

        private void movieTrailerBtn_Tapped(object sender, TappedRoutedEventArgs e)
        {
            if (MovieDetailsData != null)
            {
                this.Frame.Navigate(typeof(Views.DeviceFamily_Desktop.MovieShowCase),
                                    new ObservableCollection<string>() {MovieDetailsData.TrailerPath, "trailer" },
                                    new Windows.UI.Xaml.Media.Animation.DrillInNavigationTransitionInfo());
            }
        }

        private void MoviePlaybtn_Click(object sender, RoutedEventArgs e)
        {
            if (MovieDetailsData != null)
            {
                this.Frame.Navigate(typeof(Views.DeviceFamily_Desktop.MovieShowCase),
                                    new ObservableCollection<string>() {MovieDetailsData.MoviePath, "movie" },
                                    new Windows.UI.Xaml.Media.Animation.DrillInNavigationTransitionInfo());
            }
        }

        private async void commentsBtn_Tapped(object sender, TappedRoutedEventArgs e)
        {
            var userRatingResult =
                await new Dialogs.UserRatingDialog()
                {
                    MovieId = MovieId,
                    UserId = Functions.Common.User.Id
                }.ShowAsync();

            if (!string.IsNullOrEmpty(Result))
            {
                movieDetailsIconToastNotif.Glyph = this.Resources["ErrorIcon"] as string;
                movieDetailsTextToastNotif.Text = Result;

                movieDetailsToastNotif.Show(6000);

            }
        }

        private async void likeGrid_Tapped(object sender, TappedRoutedEventArgs e)
        {
            likeGrid.Scale(scaleX: 0.7f, scaleY: 0.7f, centerX: 0, centerY: 0, duration: 500, delay: 0).Start();
            likeIcon.Scale(scaleX: 0.7f, scaleY: 0.7f, centerX: 0, centerY: 0, duration: 500, delay: 0).Start();

            likeGrid.Scale(scaleX: 1f, scaleY: 1f, centerX: 0, centerY: 0, duration: 500, delay: 100).Start();
            likeIcon.Scale(scaleX: 1f, scaleY: 1f, centerX: 0, centerY: 0, duration: 500, delay: 100).Start();

            if (NetworkInterface.GetIsNetworkAvailable())
            {
                //This is a temporary value to send it to server and if there was error in general by client or server 
                //the main value in UserLike won't change
                int userLikeTemp = UserLiked;
                switch (userLikeTemp)
                {
                    case 0: { userLikeTemp = 1; break; }

                    case 1: { userLikeTemp = 0; break; }

                    default:
                        break;
                }

                HttpResponse userLikeMovie =
                    await Functions.Repositories.RemoteRepo.UsersRepo.AddUpdateLikeAsync(MovieId, Functions.Common.User.Id, userLikeTemp);

                if (userLikeMovie != null)
                {
                    switch (UserLiked)
                    {
                        case 0: { UserLiked = 1; UserDisliked = 0; likesCount.Text = (int.Parse(likesCount.Text) + 1).ToString(); break; }

                        case 1: { UserLiked = 0; UserDisliked = 0; likesCount.Text = (int.Parse(likesCount.Text) - 1).ToString(); break; }

                        default:
                            break;
                    }

                    movieDetailsIconToastNotif.Glyph = this.Resources["ErrorIcon"] as string;
                    movieDetailsTextToastNotif.Text = (string)userLikeMovie.Result;

                    movieDetailsToastNotif.Show(6000);
                }
                else
                {
                    movieDetailsIconToastNotif.Glyph = this.Resources["ErrorIcon"] as string;
                    movieDetailsTextToastNotif.Text = userLikeMovie.ErrorMessage;

                    movieDetailsToastNotif.Show(6000);
                }
            }
            else
            {
                movieDetailsIconToastNotif.Glyph = (string)this.Resources["NetworkIssueIcon"];
                movieDetailsTextToastNotif.Text = "You aren't connected to internet";

                movieDetailsToastNotif.Show(6000);
            }
        }

        private async void dislikeGrid_Tapped(object sender, TappedRoutedEventArgs e)
        {
            dislikeGrid.Scale(scaleX: 0.7f, scaleY: 0.7f, centerX: 0, centerY: 0, duration: 500, delay: 0).Start();
            dislikeIcon.Scale(scaleX: 0.7f, scaleY: 0.7f, centerX: 0, centerY: 0, duration: 500, delay: 0).Start();

            dislikeGrid.Scale(scaleX: 1f, scaleY: 1f, centerX: 0, centerY: 0, duration: 500, delay: 100).Start();
            dislikeIcon.Scale(scaleX: 1f, scaleY: 1f, centerX: 0, centerY: 0, duration: 500, delay: 100).Start();

            if (NetworkInterface.GetIsNetworkAvailable())
            {
                //This is a temporary value to send it to server and if there was error in general by client or server 
                //the main value in UserLike won't change
                int userDislikeTemp = UserDisliked;
                switch (userDislikeTemp)
                {
                    case 0: { userDislikeTemp = 2; break; }

                    case 2: { userDislikeTemp = 0; break; }

                    default:
                        break;
                }

                HttpResponse userDisLikeMovie =
                    await Functions.Repositories.RemoteRepo.UsersRepo.AddUpdateLikeAsync(MovieId, Functions.Common.User.Id, userDislikeTemp);

                if (userDisLikeMovie != null)
                {
                    switch (UserDisliked)
                    {
                        case 0: { UserDisliked = 2; UserLiked = 0; dislikeCount.Text = (int.Parse(dislikeCount.Text) + 1).ToString(); break; }

                        case 2: { UserDisliked = 0; UserLiked = 0; dislikeCount.Text = (int.Parse(dislikeCount.Text) - 1).ToString(); break; }

                        default:
                            break;
                    }

                    movieDetailsIconToastNotif.Glyph = this.Resources["ErrorIcon"] as string;
                    movieDetailsTextToastNotif.Text = (string)userDisLikeMovie.Result;

                    movieDetailsToastNotif.Show(6000);
                }
                else
                {
                    movieDetailsIconToastNotif.Glyph = this.Resources["ErrorIcon"] as string;
                    movieDetailsTextToastNotif.Text = userDisLikeMovie.ErrorMessage;

                    movieDetailsToastNotif.Show(6000);
                }
            }
            else
            {
                movieDetailsIconToastNotif.Glyph = (string)this.Resources["NetworkIssueIcon"];
                movieDetailsTextToastNotif.Text = "You aren't connected to internet";

                movieDetailsToastNotif.Show(6000);
            }
        }

        private async void favGrid_Tapped(object sender, TappedRoutedEventArgs e)
        {
            favGrid.Scale(scaleX: 0.7f, scaleY: 0.7f, centerX: 0, centerY: 0, duration: 500, delay: 0).Start();
            favIcon.Scale(scaleX: 0.7f, scaleY: 0.7f, centerX: 0, centerY: 0, duration: 500, delay: 0).Start();

            favGrid.Scale(scaleX: 1f, scaleY: 1f, centerX: 0, centerY: 0, duration: 500, delay: 100).Start();
            favIcon.Scale(scaleX: 1f, scaleY: 1f, centerX: 0, centerY: 0, duration: 500, delay: 100).Start();

            if (NetworkInterface.GetIsNetworkAvailable())
            {
                //This user doesn't put this movie in his favourites movies list
                if (UserFavourite == 0)
                {
                    HttpResponse userFavMovie =
                        await Functions.Repositories.RemoteRepo.FavouritesRepo.AddUserFavouriteAsync(Functions.Common.User.Id, MovieId);

                    if (userFavMovie != null)
                    {
                        UserFavourite = 1;

                        movieDetailsIconToastNotif.Glyph = this.Resources["ErrorIcon"] as string;
                        movieDetailsTextToastNotif.Text = (string)userFavMovie.Result;

                        movieDetailsToastNotif.Show(6000);
                    }
                    else
                    {
                        movieDetailsIconToastNotif.Glyph = this.Resources["ErrorIcon"] as string;
                        movieDetailsTextToastNotif.Text = userFavMovie.ErrorMessage;

                        movieDetailsToastNotif.Show(6000);
                    }
                }
                else
                {
                    HttpResponse userFavMovie =
                       await Functions.Repositories.RemoteRepo.FavouritesRepo.DeleteUserFavouriteAsync(Functions.Common.User.Id, MovieId);

                    if (userFavMovie != null)
                    {
                        UserFavourite = 0;

                        movieDetailsIconToastNotif.Glyph = this.Resources["ErrorIcon"] as string;
                        movieDetailsTextToastNotif.Text = (string)userFavMovie.Result;

                        movieDetailsToastNotif.Show(6000);
                    }
                    else
                    {
                        movieDetailsIconToastNotif.Glyph = this.Resources["ErrorIcon"] as string;
                        movieDetailsTextToastNotif.Text = userFavMovie.ErrorMessage;

                        movieDetailsToastNotif.Show(6000);
                    }
                }
            }
            else
            {
                movieDetailsIconToastNotif.Glyph = (string)this.Resources["NetworkIssueIcon"];
                movieDetailsTextToastNotif.Text = "You aren't connected to internet";

                movieDetailsToastNotif.Show(6000);
            }
        }
    }
}
