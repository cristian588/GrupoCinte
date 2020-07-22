using GrupoCinte.Common.Dtos;
using System.Threading.Tasks;

namespace GrupoCinte.Common.Services
{
    public interface IApiService
    {

        Task<Response> DeleteAsync(string urlBase, string servicePrefix, string controller, int id, string tokenType, string accessToken);

        Task<Response> GetAsync<T>(string urlBase, string servicePrefix, string controller);

        Task<Response> GetListAsync<T>(string urlBase, string servicePrefix, string controller);

        Task<Response> GetListAsync<T>(string urlBase, string servicePrefix, string controller, string tokenType, string accessToken);

        Task<Response> PostAsync<T>(string urlBase, string servicePrefix, string controller, T model);

        Task<Response> PostAsync<T>(string urlBase, string servicePrefix, string controller, T model, string tokenType, string accessToken);

        Task<Response> PutAsync<T>(string urlBase, string servicePrefix, string controller, int id, T model, string tokenType, string accessToken);

        Task<Response> PutAsync<T>(string urlBase, string servicePrefix, string controller, T model, string tokenType, string accessToken);

    }
}
