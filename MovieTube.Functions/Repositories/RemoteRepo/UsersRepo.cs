using Refit;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MovieTube.Functions.Repositories;
using MovieTube.Functions.Repositories.RepoOperations;
using MovieTube.Functions.Security;
using Newtonsoft.Json;

namespace MovieTube.Functions.Repositories.RemoteRepo
{
    public class UsersRepo
    {
        public async static Task<Models.HttpResponse> RegisterAsync(Models.User user)
        {
            user.Name = AESCryptography.Encrypt(user.Name);
            user.Email = AESCryptography.Encrypt(user.Email);
            user.Password = AESCryptography.Encrypt(user.Password);

            var usersAPIs = 
                  RestService.For<IUsersRepoOps>(Common.URL);
            Models.HttpResponse response =
               await usersAPIs.RegisterOperationAsync(user);

            if (response.Result != null)
            {
                response.Result = 
                    AESCryptography.Decrypt((string)response.Result);
                return response;
            }

            response.ErrorMessage =
                AESCryptography.Decrypt(response.ErrorMessage);

            return response;
        }

        public async static Task<Models.HttpResponse> VerifyAsync(string verifyCode)
         { 
            verifyCode = AESCryptography.Encrypt(verifyCode);

            var usersAPIs =
                  RestService.For<IUsersRepoOps>(Common.URL);
            Models.HttpResponse response =
               await usersAPIs.VerifyOperationAsync(new Dictionary<string, object>() { {"verifyCode", verifyCode } });

            if (response.Result != null)
            {
                Models.User userReference = 
                    JsonConvert.DeserializeObject<Models.User>(response.Result.ToString());

                string decTokenJson = JWTAuthorization.Decode(userReference.Token, Common.JWT_AUTH_KEY);

                if (!string.IsNullOrEmpty(decTokenJson))
                {
                    Models.Token decToken =
                        JsonConvert.DeserializeObject<Models.Token>(decTokenJson);

                    if (AESCryptography.Decrypt(decToken.SecretKeyword)
                        == AESCryptography.Decrypt(Common.SECRET_KEYWORD))
                    {
                        userReference.Name = AESCryptography.Decrypt(userReference.Name);
                        userReference.Email = AESCryptography.Decrypt(userReference.Email);
                        userReference.Password = AESCryptography.Decrypt(userReference.Password);
                        userReference.DateJoin = AESCryptography.Decrypt(userReference.DateJoin);

                        Common.User = userReference;
                    }
                    else
                    {
                        response.Result = null;
                        response.ErrorMessage = "Invalid Token from server";
                    }
                }
                else
                {
                    response.Result = null;
                    response.ErrorMessage = "Some thing error in Token";
                }

                return response;
            }

            response.ErrorMessage =
                AESCryptography.Decrypt(response.ErrorMessage);

            return response;
        }

        public async static Task<Models.HttpResponse> LoginAsync(Models.User user)
        {
            user.Email = AESCryptography.Encrypt(user.Email);
            user.Password = AESCryptography.Encrypt(user.Password);

            var usersAPIs =
                  RestService.For<IUsersRepoOps>(Common.URL);
            Models.HttpResponse response =
               await usersAPIs.LoginOperationAsync(user);

            if (response.Result != null)
            {
                Models.User userReference =
                    JsonConvert.DeserializeObject<Models.User>(response.Result.ToString());

                string decTokenJson = JWTAuthorization.Decode(userReference.Token, Common.JWT_AUTH_KEY);

                if (!string.IsNullOrEmpty(decTokenJson))
                {
                    Models.Token decToken =
                        JsonConvert.DeserializeObject<Models.Token>(decTokenJson);

                    if (AESCryptography.Decrypt(decToken.SecretKeyword)
                        == AESCryptography.Decrypt(Common.SECRET_KEYWORD))
                    {
                        userReference.Name = AESCryptography.Decrypt(userReference.Name);
                        userReference.Email = AESCryptography.Decrypt(userReference.Email);
                        userReference.Password = AESCryptography.Decrypt(userReference.Password);
                        userReference.DateJoin = AESCryptography.Decrypt(userReference.DateJoin);

                        Common.User = userReference;
                    }
                    else
                    {
                        response.Result = null;
                        response.ErrorMessage = "Invalid Token";
                    }
                }
                else
                {
                    response.Result = null;
                    response.ErrorMessage = "Some thing error in Token";
                }

                return response;
            }

            response.ErrorMessage =
                 AESCryptography.Decrypt(response.ErrorMessage);

            return response;
               
        }

        public async static Task<Models.HttpResponse> AddUpdateCommentAsync(int movieId, int userId, string userComment)
        {
            userComment = AESCryptography.Encrypt(userComment);

            var usersAPIs =
                  RestService.For<IUsersRepoOps>(Common.URL);
            Models.HttpResponse response =
               await usersAPIs.
                       AddUpdateCommentOpAsync(new Dictionary<string, object>() { 
                           { "movieId", movieId }, 
                           { "userId", userId },
                           { "userComment", AESCryptography.Encrypt(userComment) } }, 
                           Common.TOKEN);

            if (response.Result != null)
            {
                response.Result =
                    AESCryptography.Decrypt((string)response.Result);

                return response;
            }

            response.ErrorMessage =
                AESCryptography.Decrypt(response.ErrorMessage);

            return response;
        }

        public async static Task<Models.HttpResponse> AddUpdateLikeAsync(int movieId, int userId, int liked)
        {
            var usersAPIs =
                  RestService.For<IUsersRepoOps>(Common.URL);
            Models.HttpResponse response =
               await usersAPIs.AddUpdateLikeOpAsync(new Dictionary<string, object>() { 
                   { "movieId", movieId },
                   { "userId", userId }, 
                   { "liked", AESCryptography.Encrypt(liked.ToString()) } },
                   Common.TOKEN);

            if (response.Result != null)
            {
                response.Result =
                    AESCryptography.Decrypt((string)response.Result);

                return response;
            }

            response.ErrorMessage =
                AESCryptography.Decrypt(response.ErrorMessage);

            return response;
        }

        public async static Task<Models.HttpResponse> AddUpdateRatingAsync(int movieId, int userId, Models.UserRating userRating)
        {
            userRating.RatingEntertament = AESCryptography.Encrypt(userRating.RatingEntertament);
            userRating.RatingPerformActors = AESCryptography.Encrypt(userRating.RatingPerformActors);
            userRating.RatingResolution = AESCryptography.Encrypt(userRating.RatingResolution);

            var usersAPIs =
                  RestService.For<IUsersRepoOps>(Common.URL);
            Models.HttpResponse response =
               await usersAPIs.AddUpdateRatingOpAsync(new Dictionary<string, object>() {
                   { "movieId", movieId }, 
                   { "userId", userId },
                   { "userRatingEntertament", userRating.RatingEntertament },
                   { "userRatingPerformActor", userRating.RatingPerformActors }, 
                   { "userRatingResolution", userRating.RatingResolution } }, 
                   Common.TOKEN);

            if (response.Result != null)
            {
                response.Result =
                    AESCryptography.Decrypt((string)response.Result);

                return response;
            }

            response.ErrorMessage =
                AESCryptography.Decrypt(response.ErrorMessage);

            return response;
        }

        public async static Task<Models.HttpResponse> GetLikeAsync(int movieId, int userId)
        {
            var usersAPIs =
                  RestService.For<IUsersRepoOps>(Common.URL);
            Models.HttpResponse response =
               await usersAPIs.GetLikeOpAsync(movieId, userId, Common.TOKEN);

            if (response.Result != null)
            {
                response.Result =
                    AESCryptography.Decrypt((string)response.Result);

                return response;
            }

            response.ErrorMessage =
                AESCryptography.Decrypt(response.ErrorMessage);

            return response;
        }

        public async static Task<Models.HttpResponse> GetLikeRatingsCommentAsync(int movieId, int userId)
        {
            var usersAPIs =
                  RestService.For<IUsersRepoOps>(Common.URL);
            Models.HttpResponse response =
               await usersAPIs.GetLikeRatingsCommentOpAsync(movieId, userId, Common.TOKEN);

            if (response.Result != null)
            {
                Models.UserRating userRating =
                    JsonConvert.DeserializeObject<Models.UserRating>(response.Result.ToString());

                userRating.Comment = AESCryptography.Decrypt(userRating.Comment);
                userRating.Liked = AESCryptography.Decrypt(userRating.Liked);
                userRating.RatingEntertament = AESCryptography.Decrypt(userRating.RatingEntertament);
                userRating.RatingResolution = AESCryptography.Decrypt(userRating.RatingResolution);
                userRating.RatingPerformActors = AESCryptography.Decrypt(userRating.RatingPerformActors);

                response.Result = userRating;

                return response;
            }

            response.ErrorMessage =
                AESCryptography.Decrypt(response.ErrorMessage);

            return response;
        }

    }
}
