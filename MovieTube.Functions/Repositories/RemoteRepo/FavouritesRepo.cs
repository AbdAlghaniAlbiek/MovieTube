using MovieTube.Functions.Repositories.RepoOperations;
using MovieTube.Functions.Security;
using Newtonsoft.Json;
using Refit;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieTube.Functions.Repositories.RemoteRepo
{
    public class FavouritesRepo
    {
        public async static Task<Models.HttpResponse> GetUserFavouritesAsync(int userId, int movieId)
        {
            var favouritesAPIs =
               RestService.For<IFavouriteRepoOps>(Common.URL);
            Models.HttpResponse response =
               await favouritesAPIs.GetUserFavouriteIdOpAsync(userId, movieId, Common.TOKEN);

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

        public async static Task<Models.HttpResponse> GetUserFavouriteIdAsync(int userId, int movieId)
        {
            var favouritesAPIs =
                RestService.For<IFavouriteRepoOps>(Common.URL);
            Models.HttpResponse response =
               await favouritesAPIs.GetUserFavouriteIdOpAsync(userId, movieId, Common.TOKEN);

            if (response.Result != null)
            {
                response.Result = 
                    AESCryptography.Decrypt(response.Result.ToString());
                
                return response;
            }

            response.ErrorMessage =
                AESCryptography.Decrypt(response.ErrorMessage);

            return response;
        }

        public async static Task<Models.HttpResponse> AddUserFavouriteAsync(int userId, int movieId)
        {
            var favouritesAPIs =
                RestService.For<IFavouriteRepoOps>(Common.URL);
            Models.HttpResponse response =
               await favouritesAPIs.AddUserFavouriteOpAsync(userId, movieId, Common.TOKEN);

            if (response.Result != null)
            {
                response.Result =
                    AESCryptography.Decrypt(response.Result.ToString());
                
                return response;
            }

            response.ErrorMessage =
                AESCryptography.Decrypt(response.ErrorMessage);

            return response;
        }

        public async static Task<Models.HttpResponse> DeleteUserFavouriteAsync(int userId, int movieId)
        {
            var favouritesAPIs =
               RestService.For<IFavouriteRepoOps>(Common.URL);
            Models.HttpResponse response =
               await favouritesAPIs.DeleteUserFavouriteOpAsync(userId, movieId, Common.TOKEN);

            if (response.Result != null)
            {
                response.Result =
                    AESCryptography.Decrypt(response.Result.ToString());
               
                return response;
            }

            response.ErrorMessage =
                AESCryptography.Decrypt(response.ErrorMessage);

            return response;
        }
    }
}
