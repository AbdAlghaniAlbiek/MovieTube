using Refit;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieTube.Functions.Repositories.RepoOperations
{
    public interface IFavouriteRepoOps
    {
        [Get("/favourites/")]
        Task<Models.HttpResponse> GetUserFavouritesOpAsync([AliasAs("userId")] int userId, [Header("Authorization")] string token);

        [Get("/favourites/get_favourite_Id")]
        Task<Models.HttpResponse> GetUserFavouriteIdOpAsync([AliasAs("userId")] int userId, [AliasAs("movieId")] int movieId, [Header("Authorization")] string token);

        [Post("/favourites/")]
        Task<Models.HttpResponse> AddUserFavouriteOpAsync([AliasAs("userId")] int userId, [AliasAs("movieId")] int movieId, [Header("Authorization")] string token);

        [Delete("/favourites/")]
        Task<Models.HttpResponse> DeleteUserFavouriteOpAsync([AliasAs("userId")] int userId, [AliasAs("movieId")] int movieId, [Header("Authorization")] string token);
    }
}
