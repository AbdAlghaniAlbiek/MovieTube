using Refit;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieTube.Functions.Repositories.RepoOperations
{
    public interface ICategoryRepoOps
    {
        [Get("/categories/")]
        Task<Models.HttpResponse> GetCategoriesOpAsync([Header("Authorization")] string token);

        [Get("/categories/get_movies_by_category_Id")]
        Task<Models.HttpResponse> GetMoviesByCategoryIdOpAsync([AliasAs("categoryId")] int categoryId, [Header("Authorization")] string token);
    }
}
