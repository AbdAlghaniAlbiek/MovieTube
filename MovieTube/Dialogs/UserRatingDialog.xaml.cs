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
using MovieTube.Functions.Repositories;


// The Content Dialog item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace MovieTube.UI.Dialogs
{
    public sealed partial class UserRatingDialog : ContentDialog
    {
        public int MovieId { get; set; }
        public int UserId { get; set; }
        public bool IsChanged { get; set; } = false;

        public UserRatingDialog()
        {
            this.InitializeComponent();
        }

        private async void ContentDialog_Loading(FrameworkElement sender, object args)
        {
            progLoadingData.Visibility = Visibility.Visible;

            HttpResponse userRatingsResponse =
              await Functions.Repositories.RemoteRepo.UsersRepo.GetLikeRatingsCommentAsync(MovieId, UserId);

            if (userRatingsResponse.Result != null)
            {
                UserRating userRatings =
                   (UserRating)userRatingsResponse.Result;

                entertainmentRatContr.Value = double.Parse(userRatings.RatingEntertament);
                resolutionRatContr.Value = double.Parse(userRatings.RatingResolution);
                performActorRatContr.Value = double.Parse(userRatings.RatingPerformActors);
                commentTxtBox.Text = userRatings.Comment;

                progLoadingData.Visibility = Visibility.Collapsed;
            }
            else
            {
                progLoadingData.Visibility = Visibility.Collapsed;

                Views.DeviceFamily_Desktop.MovieDetailsView.Result = userRatingsResponse.ErrorMessage;
                CloseButtonClick += CloseButtonEvent;
            }
        }

        private void CloseButtonEvent(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
            
        }

        private async void ContentDialog_PrimaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
            if (resolutionRatContr.Value != 0 || performActorRatContr.Value != 0 || entertainmentRatContr.Value != 0 &&
                !string.IsNullOrEmpty(commentTxtBox.Text))
            {
                HttpResponse response = await Functions.Repositories.RemoteRepo.UsersRepo.
                    AddUpdateRatingAsync(MovieId, UserId, new UserRating()
                    {
                        RatingEntertament = entertainmentRatContr.Value.ToString(),
                        RatingResolution = resolutionRatContr.Value.ToString(),
                        RatingPerformActors = performActorRatContr.Value.ToString(),
                        Comment = commentTxtBox.Text
                    });

                if (response.Result != null)
                {
                    Views.DeviceFamily_Desktop.MovieDetailsView.Result = (string)response.Result;
                }
                else
                {
                    Views.DeviceFamily_Desktop.MovieDetailsView.Result = (string)response.ErrorMessage;
                    CloseButtonClick += CloseButtonEvent;
                }
            }
            else if (resolutionRatContr.Value != 0 || performActorRatContr.Value != 0 || entertainmentRatContr.Value != 0)
            {
                HttpResponse response = await Functions.Repositories.RemoteRepo.UsersRepo.
                  AddUpdateRatingAsync(MovieId, UserId, new UserRating()
                  {
                      RatingEntertament = entertainmentRatContr.Value.ToString(),
                      RatingResolution = resolutionRatContr.Value.ToString(),
                      RatingPerformActors = performActorRatContr.Value.ToString(),
                  });

                if (response.Result != null)
                {
                    Views.DeviceFamily_Desktop.MovieDetailsView.Result = (string)response.Result;
                }
                else
                {
                    Views.DeviceFamily_Desktop.MovieDetailsView.Result = (string)response.ErrorMessage;
                    CloseButtonClick += CloseButtonEvent;
                }
            }
            else if(!string.IsNullOrEmpty(commentTxtBox.Text))
            {
                HttpResponse response = await Functions.Repositories.RemoteRepo.UsersRepo.
                  AddUpdateRatingAsync(MovieId, UserId, new UserRating()
                  {
                      Comment = commentTxtBox.Text
                  });

                if (response.Result != null)
                {
                    Views.DeviceFamily_Desktop.MovieDetailsView.Result = (string)response.Result;
                }
                else
                {
                    Views.DeviceFamily_Desktop.MovieDetailsView.Result = (string)response.ErrorMessage;
                    CloseButtonClick += CloseButtonEvent;
                }
            }
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            IsChanged = true;
            RatingsCommentChanged();
        }

        private void EntertainmentRatingControl_ValueChanged(RatingControl sender, object args)
        {
            IsChanged = true;
            RatingsCommentChanged();
        }

        private void PerformActorRatingControl_ValueChanged(RatingControl sender, object args)
        {
            IsChanged = true;
            RatingsCommentChanged();
        }

        private void ResolutionRatingControl_ValueChanged(RatingControl sender, object args)
        {
            IsChanged = true;
            RatingsCommentChanged();
        }

        private void RatingsCommentChanged()
        {
            if (!string.IsNullOrEmpty(commentTxtBox.Text) ||
            entertainmentRatContr.Value != 0 ||
            resolutionRatContr.Value != 0 ||
            performActorRatContr.Value != 0)
            {
                this.PrimaryButtonText = "Submit";
            }
            else
            {
                this.PrimaryButtonText = "";
            }
        }

    }
}
