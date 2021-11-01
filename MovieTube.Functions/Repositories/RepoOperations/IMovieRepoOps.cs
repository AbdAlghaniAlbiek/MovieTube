using Refit;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieTube.Functions.Repositories.RepoOperations
{
    public interface IMovieRepoOps
    {
        [Get("/movies/get_best_movies")]
        Task<Models.HttpResponse> GetBestMoviesOpAsync([Header("Authorization")] string token);

        [Get("/movies/get_movie_actors")]
        Task<Models.HttpResponse> GetMovieActorsOpAsync([AliasAs("movieId")] int movieId, [Header("Authorization")] string token);

        [Get("/movies/get_movie_comments")]
        Task<Models.HttpResponse> GetMovieCommentsOpAsync([AliasAs("movieId")] int movieId, [Header("Authorization")] string token);

        [Get("/movies/get_movie_count_comments_likes_ratings")]
        Task<Models.HttpResponse> GetMovieCountCommentsLikesRatingsOpAsync([AliasAs("movieId")] int movieId, [Header("Authorization")] string token);

        [Get("/movies/get_movie_description")]
        Task<Models.HttpResponse> GetMovieDetailsOpAsync([AliasAs("movieId")] int movieId, [Header("Authorization")] string token);

        [Get("/movies/get_movie_images")]
        Task<Models.HttpResponse> GetMovieImages([AliasAs("movieId")] int movieId, [Header("Authorization")] string token);
    }
}
