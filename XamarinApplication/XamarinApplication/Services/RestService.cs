﻿using Newtonsoft.Json;
using Plugin.Connectivity;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using XamarinApplication.Models;

namespace XamarinApplication.Services
{
    public class RestService
    {
        public async Task<Response> CheckConnection()
        {
            if (!CrossConnectivity.Current.IsConnected)
            {
                return new Response
                {
                    IsSuccess = false,
                    Message = "Please turn on your internet settings.",
                };
            }

            var isReachable = await CrossConnectivity.Current.IsRemoteReachable(
                "google.com");
            if (!isReachable)
            {
                return new Response
                {
                    IsSuccess = false,
                    Message = "Check your internet connection.",
                };
            }

            return new Response
            {
                IsSuccess = true,
                Message = "Ok",
            };
        }
        //Post Request to get data
        public async Task<Response> PostData<T>(
            string urlBase,
            string servicePrefix,
            string controller,
            string accessToken,
            SearchRequest searchRequest)
        {
            try
            {
                var request = JsonConvert.SerializeObject(searchRequest);
                Debug.WriteLine("********------------request------------*************");
                Debug.WriteLine(request);
                var content = new StringContent(
                    request,
                    Encoding.UTF8,
                    "application/json");
                var client = new HttpClient();
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
                client.BaseAddress = new Uri(urlBase);
                var url = string.Format("{0}{1}", servicePrefix, controller);
                var response = await client.PostAsync(url, content);
                Debug.WriteLine("+++++++++++++++++++++++++response++++++++++++++++++++++++");
                Debug.WriteLine(response);
                if (!response.IsSuccessStatusCode)
                {
                    return new Response
                    {
                        IsSuccess = false,
                        Message = response.StatusCode.ToString(),
                    };
                }

                var result = await response.Content.ReadAsStringAsync();
                Debug.WriteLine("+++++++++++++++++++++++++result++++++++++++++++++++++++");
                Debug.WriteLine(result);
               // var array = result.Substring(4);
               // var array2 = array.Remove(array.Length - 1);
                var list = JsonConvert.DeserializeObject<List<T>>(result, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });
                Debug.WriteLine("+++++++++++++++++++++++++list++++++++++++++++++++++++");
                Debug.WriteLine(list);
                return new Response
                {
                    IsSuccess = true,
                    Message = "Record added OK",
                    Result = list,
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
        //get sportif
        public async Task<Response> PostSportif<T>(
            string urlBase,
            string servicePrefix,
            string controller,
            string accessToken,
            SearchRequestByUser searchRequest)
        {
            try
            {
                var request = JsonConvert.SerializeObject(searchRequest);
                Debug.WriteLine("********------------request------------*************");
                Debug.WriteLine(request);
                var content = new StringContent(
                    request,
                    Encoding.UTF8,
                    "application/json");
                var client = new HttpClient();
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
                client.BaseAddress = new Uri(urlBase);
                var url = string.Format("{0}{1}", servicePrefix, controller);
                var response = await client.PostAsync(url, content);
                Debug.WriteLine("+++++++++++++++++++++++++response++++++++++++++++++++++++");
                Debug.WriteLine(response);
                if (!response.IsSuccessStatusCode)
                {
                    return new Response
                    {
                        IsSuccess = false,
                        Message = response.StatusCode.ToString(),
                    };
                }

                var result = await response.Content.ReadAsStringAsync();
                Debug.WriteLine("+++++++++++++++++++++++++result++++++++++++++++++++++++");
                Debug.WriteLine(result);
                var list = JsonConvert.DeserializeObject<List<T>>(result, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });
                Debug.WriteLine("+++++++++++++++++++++++++list++++++++++++++++++++++++");
                Debug.WriteLine(list);
                return new Response
                {
                    IsSuccess = true,
                    Message = "Record added OK",
                    Result = list,
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
        //get sportif of coach
        public async Task<Response> GetSportifOfCoach<T>(
            string urlBase,
            string servicePrefix,
            string controller,
            string accessToken,
            SearchRequestByCoach searchRequest)
        {
            try
            {
                var request = JsonConvert.SerializeObject(searchRequest);
                Debug.WriteLine("********------------request------------*************");
                Debug.WriteLine(request);
                var content = new StringContent(
                    request,
                    Encoding.UTF8,
                    "application/json");
                var client = new HttpClient();
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
                client.BaseAddress = new Uri(urlBase);
                var url = string.Format("{0}{1}", servicePrefix, controller);
                var response = await client.PostAsync(url, content);
                Debug.WriteLine("+++++++++++++++++++++++++response++++++++++++++++++++++++");
                Debug.WriteLine(response);
                if (!response.IsSuccessStatusCode)
                {
                    return new Response
                    {
                        IsSuccess = false,
                        Message = response.StatusCode.ToString(),
                    };
                }

                var result = await response.Content.ReadAsStringAsync();
                Debug.WriteLine("+++++++++++++++++++++++++result++++++++++++++++++++++++");
                Debug.WriteLine(result);
                var list = JsonConvert.DeserializeObject<List<T>>(result, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });
                Debug.WriteLine("+++++++++++++++++++++++++list++++++++++++++++++++++++");
                Debug.WriteLine(list);
                return new Response
                {
                    IsSuccess = true,
                    Message = "Record added OK",
                    Result = list,
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
        public async Task<Response> SearchCoachByGym<T>(
            string urlBase,
            string servicePrefix,
            string controller,
            string accessToken,
            SearchCoachByGym searchRequest)
        {
            try
            {
                var request = JsonConvert.SerializeObject(searchRequest);
                Debug.WriteLine("********------------request------------*************");
                Debug.WriteLine(request);
                var content = new StringContent(
                    request,
                    Encoding.UTF8,
                    "application/json");
                var client = new HttpClient();
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
                client.BaseAddress = new Uri(urlBase);
                var url = string.Format("{0}{1}", servicePrefix, controller);
                var response = await client.PostAsync(url, content);
                Debug.WriteLine("+++++++++++++++++++++++++response++++++++++++++++++++++++");
                Debug.WriteLine(response);
                if (!response.IsSuccessStatusCode)
                {
                    return new Response
                    {
                        IsSuccess = false,
                        Message = response.StatusCode.ToString(),
                    };
                }

                var result = await response.Content.ReadAsStringAsync();
                Debug.WriteLine("+++++++++++++++++++++++++result++++++++++++++++++++++++");
                Debug.WriteLine(result);
                var list = JsonConvert.DeserializeObject<List<T>>(result, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });
                Debug.WriteLine("+++++++++++++++++++++++++list++++++++++++++++++++++++");
                Debug.WriteLine(list);
                return new Response
                {
                    IsSuccess = true,
                    Message = "Record added OK",
                    Result = list,
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
        //paging listview lazy load
        public async Task<Response> LoadMoreData<T>(
           string urlBase,
           string servicePrefix,
           string controller,
           string accessToken,
           SearchRequest searchRequest)
        {
            try
            {
                var request = JsonConvert.SerializeObject(searchRequest);
                var content = new StringContent(
                    request,
                    Encoding.UTF8,
                    "application/json");
                var client = new HttpClient();
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
                client.BaseAddress = new Uri(urlBase);
                var url = string.Format("{0}{1}", servicePrefix, controller);
                var response = await client.PostAsync(url, content);

                if (!response.IsSuccessStatusCode)
                {
                    return new Response
                    {
                        IsSuccess = false,
                        Message = response.StatusCode.ToString(),
                    };
                }

                var result = await response.Content.ReadAsStringAsync();
                // var newRecord = JsonConvert.DeserializeObject<T>(result);
                var list = JsonConvert.DeserializeObject<List<T>>(result, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });

                return new Response
                {
                    IsSuccess = true,
                    Message = "Record added OK",
                    Result = list,
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

        //save
        public async Task<Response> Save<T>(
            string urlBase,
            string servicePrefix,
            string controller,
            string accessToken,
            T model)
        {
            try
            {
                var request = JsonConvert.SerializeObject(model);
                Debug.WriteLine("********request*************");
                Debug.WriteLine(request);
                var content = new StringContent(
                    request,
                    Encoding.UTF8,
                    "application/json");
                var client = new HttpClient();
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
                client.BaseAddress = new Uri(urlBase);
                var url = string.Format("{0}{1}", servicePrefix, controller);
                var response = await client.PostAsync(url, content);
                Debug.WriteLine("********response*************");
                Debug.WriteLine(response);
                if (!response.IsSuccessStatusCode)
                {
                    return new Response
                    {
                        IsSuccess = false,
                        Message = response.StatusCode.ToString(),
                    };
                }

                var result = await response.Content.ReadAsStringAsync();
                var newRecord = JsonConvert.DeserializeObject<T>(result);
                Debug.WriteLine("********newRecord*************");
                Debug.WriteLine(newRecord);
                return new Response
                {
                    IsSuccess = true,
                    Message = "Record added OK",
                    Result = newRecord,
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
        //Get
        public async Task<Response> GetList<T>(
            string urlBase,
            string servicePrefix,
            string controller,
            string accessToken)
        {
            try
            {
                var client = new HttpClient();
               // client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(tokenType, accessToken);
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
                client.BaseAddress = new Uri(urlBase);
                var url = string.Format("{0}{1}", servicePrefix, controller);
                var response = await client.GetAsync(url);
                var result = await response.Content.ReadAsStringAsync();

                if (!response.IsSuccessStatusCode)
                {
                    return new Response
                    {
                        IsSuccess = false,
                        //Message = result,
                    };
                }

                var list = JsonConvert.DeserializeObject<List<T>>(result, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });
                return new Response
                {
                    IsSuccess = true,
                    Message = "Ok",
                    Result = list,
                };
            }
            catch (Exception ex)
            {
                return new Response
                {
                    IsSuccess = false,
                    //Message = ex.Message,
                };
            }
        }

        public async Task<Response> GetList<T>(
            string urlBase,
            string servicePrefix,
            string controller,
            int pageIndex, 
            int pageSize,
            string accessToken)
        {
            try
            {
                var client = new HttpClient();
                // client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(tokenType, accessToken);
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
                client.BaseAddress = new Uri(urlBase);
                var url = string.Format("{0}{1}", servicePrefix, controller);
                var response = await client.GetAsync(url);
                var result = await response.Content.ReadAsStringAsync();

                if (!response.IsSuccessStatusCode)
                {
                    return new Response
                    {
                        IsSuccess = false,
                        //Message = result,
                    };
                }

                var list = JsonConvert.DeserializeObject<List<T>>(result, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });
                await Task.Delay(2000);
                return new Response
                {
                    IsSuccess = true,
                    Message = "Ok",
                    Result = list.Skip(pageIndex * pageSize).Take(pageSize).ToList(),
                };
            }
            catch (Exception ex)
            {
                return new Response
                {
                    IsSuccess = false,
                    //Message = ex.Message,
                };
            }
        }

        /*
       public async Task<List<Machine>> GetMachineAsync(string accessToken)
       {
           var client = new HttpClient();
           client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
           var json = await client.GetStringAsync("http://phoneofficine.it/niini-gim/machine");
           var machines = JsonConvert.DeserializeObject<List<Machine>>(json, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });
           return machines;
       }

       public async Task<TokenResponse> GetToken(
           string urlBase,
           string username,
           string password)
       {
           try
           {
               var client = new HttpClient();
               client.BaseAddress = new Uri(urlBase);
               var response = await client.PostAsync("Token",
                   new StringContent(string.Format(
                   "grant_type=password&username={0}&password={1}",
                   username, password),
                   Encoding.UTF8, "application/x-www-form-urlencoded"));
               var resultJSON = await response.Content.ReadAsStringAsync();
               var result = JsonConvert.DeserializeObject<TokenResponse>(
                   resultJSON);
               return result;
           }
           catch
           {
               return null;
           }
       }
       public async Task<Response> Get<T>(
           string urlBase,
           string servicePrefix,
           string controller,
           string tokenType,
           string accessToken,
           int id)
       {
           try
           {
               var client = new HttpClient();
               client.DefaultRequestHeaders.Authorization =
                   new AuthenticationHeaderValue(tokenType, accessToken);
               client.BaseAddress = new Uri(urlBase);
               var url = string.Format(
                   "{0}{1}/{2}",
                   servicePrefix,
                   controller,
                   id);
               var response = await client.GetAsync(url);

               if (!response.IsSuccessStatusCode)
               {
                   return new Response
                   {
                       IsSuccess = false,
                       Message = response.StatusCode.ToString(),
                   };
               }

               var result = await response.Content.ReadAsStringAsync();
               var model = JsonConvert.DeserializeObject<T>(result);
               return new Response
               {
                   IsSuccess = true,
                   Message = "Ok",
                   Result = model,
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

       public async Task<Response> GetList<T>(
           string urlBase,
           string servicePrefix,
           string controller)
       {
           try
           {
               var client = new HttpClient();
               client.BaseAddress = new Uri(urlBase);
               var url = string.Format("{0}{1}", servicePrefix, controller);
               var response = await client.GetAsync(url);
               var result = await response.Content.ReadAsStringAsync();

               if (!response.IsSuccessStatusCode)
               {
                   return new Response
                   {
                       IsSuccess = false,
                       Message = result,
                   };
               }

               var list = JsonConvert.DeserializeObject<List<T>>(result);
               return new Response
               {
                   IsSuccess = true,
                   Message = "Ok",
                   Result = list,
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
       public async Task<Response> GetList<T>(
          string urlBase,
          string servicePrefix,
          string controller,
          string tokenType,
          string accessToken,
          int id)
      {
          try
          {
              var client = new HttpClient();
              client.DefaultRequestHeaders.Authorization =
                  new AuthenticationHeaderValue(tokenType, accessToken);
              client.BaseAddress = new Uri(urlBase);
              var url = string.Format(
                  "{0}{1}/{2}",
                  servicePrefix,
                  controller,
                  id);
              var response = await client.GetAsync(url);

              if (!response.IsSuccessStatusCode)
              {
                  return new Response
                  {
                      IsSuccess = false,
                      Message = response.StatusCode.ToString(),
                  };
              }

              var result = await response.Content.ReadAsStringAsync();
              var list = JsonConvert.DeserializeObject<List<T>>(result);
              return new Response
              {
                  IsSuccess = true,
                  Message = "Ok",
                  Result = list,
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
       */
        /*
        public async Task<Response> Post<T>(
            string urlBase,
            string servicePrefix,
            string controller,
            string tokenType,
            string accessToken,
            T model)
        {
            try
            {
                var request = JsonConvert.SerializeObject(model);
                var content = new StringContent(
                    request, Encoding.UTF8,
                    "application/json");
                var client = new HttpClient();
                client.DefaultRequestHeaders.Authorization =
                    new AuthenticationHeaderValue(tokenType, accessToken);
                client.BaseAddress = new Uri(urlBase);
                var url = string.Format("{0}{1}", servicePrefix, controller);
                var response = await client.PostAsync(url, content);
                var result = await response.Content.ReadAsStringAsync();

                if (!response.IsSuccessStatusCode)
                {
                    var error = JsonConvert.DeserializeObject<Response>(result);
                    error.IsSuccess = false;
                    return error;
                }

                var newRecord = JsonConvert.DeserializeObject<T>(result);

                return new Response
                {
                    IsSuccess = true,
                    Message = "Record added OK",
                    Result = newRecord,
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

        public async Task<Response> Post<T>(
            string urlBase,
            string servicePrefix,
            string controller,
            T model)
        {
            try
            {
                var request = JsonConvert.SerializeObject(model);
                var content = new StringContent(
                    request,
                    Encoding.UTF8,
                    "application/json");
                var client = new HttpClient();
                client.BaseAddress = new Uri(urlBase);
                var url = string.Format("{0}{1}", servicePrefix, controller);
                var response = await client.PostAsync(url, content);

                if (!response.IsSuccessStatusCode)
                {
                    return new Response
                    {
                        IsSuccess = false,
                        Message = response.StatusCode.ToString(),
                    };
                }

                var result = await response.Content.ReadAsStringAsync();
                var newRecord = JsonConvert.DeserializeObject<T>(result);

                return new Response
                {
                    IsSuccess = true,
                    Message = "Record added OK",
                    Result = newRecord,
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

        public async Task<Response> Put<T>(
            string urlBase,
            string servicePrefix,
            string controller,
            string tokenType,
            string accessToken,
            T model)
        {
            try
            {
                var request = JsonConvert.SerializeObject(model);
                var content = new StringContent(
                    request,
                    Encoding.UTF8, "application/json");
                var client = new HttpClient();
                client.DefaultRequestHeaders.Authorization =
                    new AuthenticationHeaderValue(tokenType, accessToken);
                client.BaseAddress = new Uri(urlBase);
                var url = string.Format(
                    "{0}{1}/{2}",
                    servicePrefix,
                    controller,
                    model.GetHashCode());
                var response = await client.PutAsync(url, content);
                var result = await response.Content.ReadAsStringAsync();

                if (!response.IsSuccessStatusCode)
                {
                    var error = JsonConvert.DeserializeObject<Response>(result);
                    error.IsSuccess = false;
                    return error;
                }

                var newRecord = JsonConvert.DeserializeObject<T>(result);

                return new Response
                {
                    IsSuccess = true,
                    Result = newRecord,
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

        public async Task<Response> Delete<T>(
            string urlBase,
            string servicePrefix,
            string controller,
            string tokenType,
            string accessToken,
            T model)
        {
            try
            {
                var client = new HttpClient();
                client.BaseAddress = new Uri(urlBase);
                client.DefaultRequestHeaders.Authorization =
                    new AuthenticationHeaderValue(tokenType, accessToken);
                var url = string.Format(
                    "{0}{1}/{2}",
                    servicePrefix,
                    controller,
                    model.GetHashCode());
                var response = await client.DeleteAsync(url);
                var result = await response.Content.ReadAsStringAsync();

                if (!response.IsSuccessStatusCode)
                {
                    var error = JsonConvert.DeserializeObject<Response>(result);
                    error.IsSuccess = false;
                    return error;
                }

                return new Response
                {
                    IsSuccess = true,
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
        */
    }
}
