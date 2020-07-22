using GrupoCinte.Common.Dtos;
using GrupoCinte.Common.Results;
using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace GrupoCinte.Common.Services
{
    public class ApiService : IApiService
    {
        public async Task<Response> GetAsync<T>(
            string urlBase,
            string servicePrefix,
            string controller)
        {
            try
            {
                var client = new HttpClient();
                var url = $"{urlBase}{servicePrefix}{controller}";
                var response = await client.GetAsync(url);
                var result = await response.Content.ReadAsStringAsync();

                if (!response.IsSuccessStatusCode)
                {
                    try
                    {
                        var errorMessage = JsonConvert.DeserializeObject<ErrorMessageResult>(result);
                        result = String.IsNullOrEmpty(errorMessage.ExceptionMessage) ? errorMessage.Message : errorMessage.ExceptionMessage;
                    }
                    catch (Exception) { }
                    return new Response
                    {
                        IsSuccess = false,
                        Message = result,
                    };
                }

                return new Response
                {
                    IsSuccess = true,
                    Result = result
                };
            }
            catch (Exception ex)
            {
                return new Response
                {
                    IsSuccess = false,
                    Message = ex.Message
                };
            }
        }

        public async Task<Response> GetAsync<T>(
            string urlBase,
            string servicePrefix,
            string controller,
            string tokenType,
            string accessToken)
        {
            try
            {
                var client = new HttpClient();
                client.DefaultRequestHeaders.Add("Authorization", $"{tokenType} {accessToken}");
                var url = $"{urlBase}{servicePrefix}{controller}";
                var response = await client.GetAsync(url);
                var result = await response.Content.ReadAsStringAsync();

                if (!response.IsSuccessStatusCode)
                {
                    try
                    {
                        var errorMessage = JsonConvert.DeserializeObject<ErrorMessageResult>(result);
                        result = String.IsNullOrEmpty(errorMessage.ExceptionMessage) ? errorMessage.Message : errorMessage.ExceptionMessage;
                    }
                    catch (Exception) { }
                    return new Response
                    {
                        IsSuccess = false,
                        Message = result,
                    };
                }

                return new Response
                {
                    IsSuccess = true,
                    Result = result
                };
            }
            catch (Exception ex)
            {
                return new Response
                {
                    IsSuccess = false,
                    Message = ex.Message
                };
            }
        }

        public async Task<Response> GetListAsync<T>(
            string urlBase,
            string servicePrefix,
            string controller)
        {
            try
            {
                var client = new HttpClient();
                var url = $"{urlBase}{servicePrefix}{controller}";
                var response = await client.GetAsync(url);
                var result = await response.Content.ReadAsStringAsync();

                if (!response.IsSuccessStatusCode)
                {
                    try
                    {
                        var errorMessage = JsonConvert.DeserializeObject<ErrorMessageResult>(result);
                        result = String.IsNullOrEmpty(errorMessage.ExceptionMessage) ? errorMessage.Message : errorMessage.ExceptionMessage;
                    }
                    catch (Exception) { }
                    return new Response
                    {
                        IsSuccess = false,
                        Message = result,
                    };
                }

                return new Response
                {
                    IsSuccess = true,
                    Result = result
                };
            }
            catch (Exception ex)
            {
                return new Response
                {
                    IsSuccess = false,
                    Message = ex.Message
                };
            }
        }

        public async Task<Response> GetListAsync<T>(
            string urlBase,
            string servicePrefix,
            string controller,
            string tokenType,
            string accessToken)
        {
            try
            {
                var client = new HttpClient();
                client.DefaultRequestHeaders.Add("Authorization", $"{tokenType} {accessToken}");
                var url = $"{urlBase}{servicePrefix}{controller}";
                var response = await client.GetAsync(url);
                var result = await response.Content.ReadAsStringAsync();

                if (!response.IsSuccessStatusCode)
                {
                    try
                    {
                        var errorMessage = JsonConvert.DeserializeObject<ErrorMessageResult>(result);
                        result = String.IsNullOrEmpty(errorMessage.ExceptionMessage) ? errorMessage.Message : errorMessage.ExceptionMessage;
                    }
                    catch (Exception) { }
                    return new Response
                    {
                        IsSuccess = false,
                        Message = result,
                    };
                }

                return new Response
                {
                    IsSuccess = true,
                    Result = result
                };
            }
            catch (Exception ex)
            {
                return new Response
                {
                    IsSuccess = false,
                    Message = ex.Message
                };
            }
        }

        public async Task<Response> PostAsync<T>(
            string urlBase,
            string servicePrefix,
            string controller,
            T model)
        {
            try
            {
                var request = JsonConvert.SerializeObject(model);
                var content = new StringContent(request, Encoding.UTF8, "application/json");
                var client = new HttpClient();
                var url = $"{urlBase}{servicePrefix}{controller}";
                var response = await client.PostAsync(url, content);
                var result = await response.Content.ReadAsStringAsync();
                if (!response.IsSuccessStatusCode)
                {
                    try
                    {
                        var errorMessage = JsonConvert.DeserializeObject<ErrorMessageResult>(result);
                        result = String.IsNullOrEmpty(errorMessage.ExceptionMessage) ? errorMessage.Message : errorMessage.ExceptionMessage;
                    }
                    catch (Exception) { }
                    return new Response
                    {
                        IsSuccess = false,
                        Message = result,
                    };
                }
                return new Response()
                {
                    IsSuccess = true,
                    Result = result
                };
            }
            catch (Exception ex)
            {
                return new Response
                {
                    IsSuccess = false,
                    Message = ex.Message,
                };
            }
        }

        public async Task<Response> PostAsync<T>(
            string urlBase,
            string servicePrefix,
            string controller,
            T model,
            string tokenType,
            string accessToken)
        {
            try
            {
                var request = JsonConvert.SerializeObject(model);
                var content = new StringContent(request, Encoding.UTF8, "application/json");
                var client = new HttpClient();
                client.DefaultRequestHeaders.Add("Authorization", $"{tokenType} {accessToken}");
                var url = $"{urlBase}{servicePrefix}{controller}";
                var response = await client.PostAsync(url, content);
                var result = await response.Content.ReadAsStringAsync();
                if (!response.IsSuccessStatusCode)
                {
                    try
                    {
                        var errorMessage = JsonConvert.DeserializeObject<ErrorMessageResult>(result);
                        result = String.IsNullOrEmpty(errorMessage.ExceptionMessage) ? errorMessage.Message : errorMessage.ExceptionMessage;
                    }
                    catch (Exception) { }
                    return new Response
                    {
                        IsSuccess = false,
                        Message = result,
                    };
                }
                return new Response()
                {
                    IsSuccess = true,
                    Result = result
                };
            }
            catch (Exception ex)
            {
                return new Response
                {
                    IsSuccess = false,
                    Message = ex.Message,
                };
            }
        }

        public async Task<Response> PutAsync<T>(
            string urlBase,
            string servicePrefix,
            string controller,
            int id,
            T model,
            string tokenType,
            string accessToken)
        {
            try
            {
                var request = JsonConvert.SerializeObject(model);
                var content = new StringContent(request, Encoding.UTF8, "application/json");
                var client = new HttpClient
                {
                    BaseAddress = new Uri(urlBase)
                };

                client.DefaultRequestHeaders.Add("Authorization", $"{tokenType} {accessToken}");

                var url = $"{servicePrefix}{controller}/{id}";
                var response = await client.PutAsync(url, content);
                var answer = await response.Content.ReadAsStringAsync();
                if (!response.IsSuccessStatusCode)
                {
                    return new Response
                    {
                        IsSuccess = false,
                        Message = answer,
                    };
                }

                var obj = JsonConvert.DeserializeObject<T>(answer);
                return new Response
                {
                    IsSuccess = true,
                    Result = obj,
                };
            }
            catch (Exception ex)
            {
                return new Response
                {
                    IsSuccess = false,
                    Message = ex.Message,
                };
            }
        }

        public async Task<Response> DeleteAsync(
            string urlBase,
            string servicePrefix,
            string controller,
            int id,
            string tokenType,
            string accessToken)
        {
            try
            {
                var client = new HttpClient
                {
                    BaseAddress = new Uri(urlBase)
                };

                client.DefaultRequestHeaders.Add("Authorization", $"{tokenType} {accessToken}");

                var url = $"{servicePrefix}{controller}/{id}";
                var response = await client.DeleteAsync(url);
                var answer = await response.Content.ReadAsStringAsync();
                if (!response.IsSuccessStatusCode)
                {
                    return new Response
                    {
                        IsSuccess = false,
                        Message = answer,
                    };
                }

                return new Response
                {
                    IsSuccess = true
                };
            }
            catch (Exception ex)
            {
                return new Response
                {
                    IsSuccess = false,
                    Message = ex.Message,
                };
            }
        }


        public async Task<Response> PutAsync<T>(
            string urlBase,
            string servicePrefix,
            string controller,
            T model,
            string tokenType,
            string accessToken)
        {
            try
            {
                var request = JsonConvert.SerializeObject(model);
                var content = new StringContent(request, Encoding.UTF8, "application/json");
                var client = new HttpClient
                {
                    BaseAddress = new Uri(urlBase)
                };

                client.DefaultRequestHeaders.Add("Authorization", $"{tokenType} {accessToken}");
                var url = $"{servicePrefix}{controller}";
                var response = await client.PutAsync(url, content);
                var answer = await response.Content.ReadAsStringAsync();
                if (!response.IsSuccessStatusCode)
                {
                    return new Response
                    {
                        IsSuccess = false,
                        Message = answer,
                    };
                }

                var obj = JsonConvert.DeserializeObject<T>(answer);
                return new Response
                {
                    IsSuccess = true,
                    Result = obj,
                };
            }
            catch (Exception ex)
            {
                return new Response
                {
                    IsSuccess = false,
                    Message = ex.Message,
                };
            }
        }


    }
}
