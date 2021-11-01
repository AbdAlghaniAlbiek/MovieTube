using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Refit;

namespace MovieTube.Functions.Repositories.RepoOperations
{
    public interface IUsersRepoOps
    {
        [Post("/users/register")]
        Task<Models.HttpResponse> RegisterOperationAsync([Body(BodySerializationMethod.UrlEncoded)] Models.User user);

        [Post("/users/verify")]
        Task<Models.HttpResponse> VerifyOperationAsync([Body(BodySerializationMethod.UrlEncoded)] Dictionary<string, object> verifyCode);

        [Post("/users/login")]
        Task<Models.HttpResponse> LoginOperationAsync([Body(BodySerializationMethod.UrlEncoded)] Models.User user);

        [Post("/users/add_update_comment")]
        Task<Models.HttpResponse> AddUpdateCommentOpAsync([Body(BodySerializationMethod.UrlEncoded)] Dictionary<string, object> userComment, [Header("Authorization")] string token);

        [Post("/users/add_update_like")]
        Task<Models.HttpResponse> AddUpdateLikeOpAsync([Body(BodySerializationMethod.UrlEncoded)] Dictionary<string, object> userLike, [Header("Authorization")] string token);

        [Post("/users/add_update__rating")]
        Task<Models.HttpResponse> AddUpdateRatingOpAsync([Body(BodySerializationMethod.UrlEncoded)] Dictionary<string, object> userRating, [Header("Authorization")] string token);

        [Get("/users/get_like")]
        Task<Models.HttpResponse> GetLikeOpAsync([AliasAs("movieId")] int movieId, [AliasAs("userId")] int userId, [Header("Authorization")] string token);

        [Get("/users/get_like_ratings_comment")]
        Task<Models.HttpResponse> GetLikeRatingsCommentOpAsync([AliasAs("movieId")] int movieId, [AliasAs("userId")] int userId, [Header("Authorization")] string token);
    }
}
