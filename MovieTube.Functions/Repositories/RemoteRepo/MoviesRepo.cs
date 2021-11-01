using MovieTube.Functions.Repositories.RepoOperations;
using MovieTube.Functions.Security;
using Refit;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace MovieTube.Functions.Repositories.RemoteRepo
{
    public class MoviesRepo
    {
        public async static Task<Models.HttpResponse> GetBestMoviesAsync()
        {
            var moviesAPIs =
                RestService.For<IMovieRepoOps>(Common.URL);
            Models.HttpResponse response =
               await moviesAPIs.GetBestMoviesOpAsync(Common.TOKEN);

            if (response.Result != null)
            {
                ObservableCollection<Models.Movie> moviesReference = 
                    JsonConvert.DeserializeObject<ObservableCollection<Models.Movie>>(response.Result.ToString());

                for (int i = 0; i < moviesReference.Count; i++)
                {
                    moviesReference[i].CategoryName = AESCryptography.Decrypt(moviesReference[i].CategoryName);
                    moviesReference[i].Description = AESCryptography.Decrypt(moviesReference[i].Description);
                    moviesReference[i].ImagePath = AESCryptography.Decrypt(moviesReference[i].ImagePath);
                    moviesReference[i].Name = AESCryptography.Decrypt(moviesReference[i].Name);
                    moviesReference[i].Rate = AESCryptography.Decrypt(moviesReference[i].Rate);
                }

                response.Result = moviesReference;

                return response;
            }

            response.ErrorMessage =
                AESCryptography.Decrypt(response.ErrorMessage);

            return response;
        }

        public async static Task<Models.HttpResponse> GetMovieActorsAsync(int movieId)
        {
            var moviesAPIs =
                 RestService.For<IMovieRepoOps>(Common.URL);
            Models.HttpResponse response =
               await moviesAPIs.GetMovieActorsOpAsync(movieId, Common.TOKEN);

            if (response.Result != null)
            {
                ObservableCollection<Models.Actor> movieActorsReference =
                    JsonConvert.DeserializeObject<ObservableCollection<Models.Actor>>(response.Result.ToString());

                for (int i = 0; i < movieActorsReference.Count; i++)
                {
                    movieActorsReference[i].Name = AESCryptography.Decrypt(movieActorsReference[i].Name);
                    movieActorsReference[i].ImagePath = AESCryptography.Decrypt(movieActorsReference[i].ImagePath);
                }

                response.Result = movieActorsReference;

                return response;
            }

            response.ErrorMessage =
                AESCryptography.Decrypt(response.ErrorMessage);

            return response;
        }

        public async static Task<Models.HttpResponse> GetMovieCommentsAsync(int movieId)
        {
            var moviesAPIs =
                  RestService.For<IMovieRepoOps>(Common.URL);
            Models.HttpResponse response =
               await moviesAPIs.GetMovieCommentsOpAsync(movieId, Common.TOKEN);

            if (response.Result != null)
            {
                ObservableCollection<Models.UserComment> movieCommentsReference =
                    JsonConvert.DeserializeObject<ObservableCollection<Models.UserComment>>(response.Result.ToString());

                for (int i = 0; i < movieCommentsReference.Count; i++)
                {
                    movieCommentsReference[i].Name = AESCryptography.Decrypt(movieCommentsReference[i].Name);
                    movieCommentsReference[i].ImagePath = AESCryptography.Decrypt(movieCommentsReference[i].ImagePath);
                    movieCommentsReference[i].Comment = AESCryptography.Decrypt(movieCommentsReference[i].Comment);
                }

                response.Result = movieCommentsReference;
                
                return response;
            }

            response.ErrorMessage =
                AESCryptography.Decrypt(response.ErrorMessage);

            return response;
        }

        public async static Task<Models.HttpResponse> GetMovieDetailsAsync(int movieId)
        {
            var moviesAPIs =
                  RestService.For<IMovieRepoOps>(Common.URL);
            Models.HttpResponse response =
               await moviesAPIs.GetMovieDetailsOpAsync(movieId, Common.TOKEN);

            if (response.Result != null)
            {
                Models.MovieDetails movieDetailsReference =
                     JsonConvert.DeserializeObject<Models.MovieDetails>(response.Result.ToString());

                movieDetailsReference.Name = AESCryptography.Decrypt(movieDetailsReference.Name.ToString());
                movieDetailsReference.Description = AESCryptography.Decrypt(movieDetailsReference.Description);
                movieDetailsReference.Rate = AESCryptography.Decrypt(movieDetailsReference.Rate);
                movieDetailsReference.MoviePath = AESCryptography.Decrypt(movieDetailsReference.MoviePath);
                movieDetailsReference.TrailerPath = AESCryptography.Decrypt(movieDetailsReference.TrailerPath);
                movieDetailsReference.ReleaseDate = AESCryptography.Decrypt(movieDetailsReference.ReleaseDate);
                movieDetailsReference.Poster = AESCryptography.Decrypt(movieDetailsReference.Poster);
                movieDetailsReference.Banner = AESCryptography.Decrypt(movieDetailsReference.Banner);

                response.Result = movieDetailsReference;

                return response;
            }

            response.ErrorMessage =
                AESCryptography.Decrypt(response.ErrorMessage);

            return response;
        }

        public async static Task<Models.HttpResponse> GetMovieImages(int movieId)
        {
            var moviesAPIs =
                  RestService.For<IMovieRepoOps>(Common.URL);
            Models.HttpResponse response =
               await moviesAPIs.GetMovieImages(movieId, Common.TOKEN);

            if (response.Result != null)
            {
                ObservableCollection<string> movieImagesReference =
                     JsonConvert.DeserializeObject<ObservableCollection<string>>(response.Result.ToString());

                for (int i = 0; i < movieImagesReference.Count; i++)
                {
                    movieImagesReference[i] = AESCryptography.Decrypt(movieImagesReference[i]);
                }

                response.Result = movieImagesReference;

                return response;
            }

            response.ErrorMessage =
                AESCryptography.Decrypt(response.ErrorMessage);

            return response;
        }

        public async static Task<Models.HttpResponse> GetMovieCountCommentsLikesRatingsAsync(int movieId)
        {
            var moviesAPIs =
                  RestService.For<IMovieRepoOps>(Common.URL);
            Models.HttpResponse response =
               await moviesAPIs.GetMovieCountCommentsLikesRatingsOpAsync(movieId, Common.TOKEN);

            if (response.Result != null)
            {


                Models.UsersFeedback movieCountCommentsLikesRatings =
                     JsonConvert.DeserializeObject<Models.UsersFeedback> (response.Result.ToString());

                movieCountCommentsLikesRatings.CountComments = AESCryptography.Decrypt(movieCountCommentsLikesRatings.CountComments);
                movieCountCommentsLikesRatings.CountDislikes = AESCryptography.Decrypt(movieCountCommentsLikesRatings.CountDislikes);
                movieCountCommentsLikesRatings.CountLikes = AESCryptography.Decrypt(movieCountCommentsLikesRatings.CountLikes);
                movieCountCommentsLikesRatings.CountRatings = AESCryptography.Decrypt(movieCountCommentsLikesRatings.CountRatings);
                movieCountCommentsLikesRatings.RatingPerformActors = AESCryptography.Decrypt(movieCountCommentsLikesRatings.RatingPerformActors);
                movieCountCommentsLikesRatings.RatingResolution = AESCryptography.Decrypt(movieCountCommentsLikesRatings.RatingResolution);
                movieCountCommentsLikesRatings.AvgRatingEntertament = AESCryptography.Decrypt(movieCountCommentsLikesRatings.AvgRatingEntertament);

                response.Result = movieCountCommentsLikesRatings;

                return response;
            }

            response.ErrorMessage =
                AESCryptography.Decrypt(response.ErrorMessage);

            return response;
        }
    }
}
