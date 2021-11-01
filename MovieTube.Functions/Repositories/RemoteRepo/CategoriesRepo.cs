using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using Refit;
using System.Threading.Tasks;
using MovieTube.Functions.Repositories.RepoOperations;
using MovieTube.Functions.Security;
using Newtonsoft.Json;

namespace MovieTube.Functions.Repositories.RemoteRepo
{
    public class CategoriesRepo
    {
        public async static Task<Models.HttpResponse> GetCategoriesAsync()
        {
            var categoriesAPIs = 
                RestService.For<ICategoryRepoOps>(Common.URL);
            Models.HttpResponse response =
               await categoriesAPIs.GetCategoriesOpAsync(Common.TOKEN);
            
            if (response.Result != null)
            {
                ObservableCollection<Models.Category> categoriesReference =
                    JsonConvert.DeserializeObject<ObservableCollection<Models.Category>>(response.Result.ToString());

                for (int i = 0; i < categoriesReference.Count; i++)
                {
                    categoriesReference[i].Name = AESCryptography.Decrypt(categoriesReference[i].Name);
                    categoriesReference[i].ImagePath = AESCryptography.Decrypt(categoriesReference[i].ImagePath);
                }
               
                response.Result = categoriesReference;
                return response;
            }

            response.ErrorMessage = 
                AESCryptography.Decrypt(response.ErrorMessage);

            return response;
        }

        public async static Task<Models.HttpResponse> GetMoviesByCategoryIdAsync(int movieId)
        {
            var categoriesAPIs =
                RestService.For<ICategoryRepoOps>(Common.URL);
            Models.HttpResponse response =
               await categoriesAPIs.GetMoviesByCategoryIdOpAsync(movieId, Common.TOKEN);

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
    }

}
