using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Plugin.Connectivity;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using XamarinApplication.Helpers;
using XamarinApplication.Models;

namespace XamarinApplication.Services
{
    public class ApiServices
    {
        public INavigation Navigation { get; set; }
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
        public async Task<bool> RegisterAsync(string email, string password, string confirmPassword)
         {
             var client = new HttpClient();
             var model = new RegisterBindingModel
             {
                 Email = email,
                 Password = password,
                 ConfirmPassword = confirmPassword
             };
             var json = JsonConvert.SerializeObject(model);
             HttpContent content = new StringContent(json);
             content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
             var response = await client.PostAsync("http://192.168.1.59:8092/api/Account/Register", content); 

            Debug.WriteLine(await response.Content.ReadAsStringAsync());

             return response.IsSuccessStatusCode;

        }

        public async Task<string> LoginGym(string userName, string password)
        {
            var client = new HttpClient();
           /* var keyValues = new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>("username", userName),
                new KeyValuePair<string, string>("password", password)
            };
            var json = JsonConvert.SerializeObject(keyValues);*/

            var json = JsonConvert.SerializeObject(new { username = userName, password = password });
            HttpContent content = new StringContent(json);
            content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            var response = await client.PostAsync("http://phoneofficine.it/niini-gim/login", content);
            
            var token = await response.Content.ReadAsStringAsync();
            //client.DefaultRequestHeaders.Add("Authorization", "Bearer " + response);
            //client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var jwt = response.Headers.GetValues("Authorization").FirstOrDefault();
            Debug.WriteLine(jwt);

            /*if (response.IsSuccessStatusCode)
            {
                var jwt = response.Headers.GetValues("Authorization").FirstOrDefault();
                Debug.WriteLine("********");
                Debug.WriteLine(jwt);
                Debug.WriteLine("********");

                await Application.Current.MainPage.DisplayAlert("Message", "Login successfully", "ok");
                await Application.Current.MainPage.Navigation.PushAsync(new MainPage());
                return jwt;
            }
            else
            {
                await Application.Current.MainPage.DisplayAlert("Message", "Failed to login", "ok");
                 return token;
            }*/
            return jwt;
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
                Debug.WriteLine("********result*************");
                Debug.WriteLine(result);
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
        public async Task SaveRequest<T>(
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

            }
            catch (Exception ex)
            {
                Debug.WriteLine("********Exception*************");
                Debug.WriteLine(ex);
            }
        }
        //Post Request to get Serie
        public async Task<Response> SeriesData<T>(
            string urlBase,
            string servicePrefix,
            string controller,
            string accessToken,
            SeriesExercice exercice)
        {
            try
            {
                var request = JsonConvert.SerializeObject(exercice);
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
                // var newRecord = JsonConvert.DeserializeObject<T>(result);
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
        //Post Request to get Parametres of Sportif
        public async Task<Response> ParametresData<T>(
            string urlBase,
            string servicePrefix,
            string controller,
            string accessToken,
            ParametreClient parametre)
        {
            try
            {
                var request = JsonConvert.SerializeObject(parametre);
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
                // var newRecord = JsonConvert.DeserializeObject<T>(result);
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
        //Post Request to get Lessons of Sportif
        public async Task<Response> LessonsData<T>(
            string urlBase,
            string servicePrefix,
            string controller,
            string accessToken,
            SearchLesson _searchLesson)
        {
            try
            {
                var request = JsonConvert.SerializeObject(_searchLesson);
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
                // var newRecord = JsonConvert.DeserializeObject<T>(result);
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
        //Post GYM to get Machine
        public async Task<Response> GymMachine<T>(
            string urlBase,
            string servicePrefix,
            string controller,
            string accessToken,
            SearchByGym _searchGym)
        {
            try
            {
                var request = JsonConvert.SerializeObject(_searchGym);
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
                // var newRecord = JsonConvert.DeserializeObject<T>(result);
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
        //Post Request By sportif to get Plannings
        public async Task<Response> PlanningData<T>(
            string urlBase,
            string servicePrefix,
            string controller,
            string accessToken,
            SearchBySportif _search)
        {
            try
            {
                var request = JsonConvert.SerializeObject(_search);
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
                // var newRecord = JsonConvert.DeserializeObject<T>(result);
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
        //put
        public async Task<Response> Put<T>(
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
                var response = await client.PutAsync(url, content);
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
                Debug.WriteLine("********result*************");
                Debug.WriteLine(result);
                var newRecord = JsonConvert.DeserializeObject<T>(result);
                Debug.WriteLine("********newRecord*************");
                Debug.WriteLine(newRecord);
                return new Response
                {
                    IsSuccess = true,
                    Message = "Record updated OK",
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
        public async Task PutRequest<T>(
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
                var response = await client.PutAsync(url, content);
                Debug.WriteLine("********response*************");
                Debug.WriteLine(response);

            }
            catch (Exception ex)
            {
                Debug.WriteLine("********Exception*************");
                Debug.WriteLine(ex);
            }
        }
        //delete
        public async Task<Response> Delete<T>(
            string urlBase,
            string servicePrefix,
            string controller,
            int id,
            string accessToken)
        {
            try
            {
                var client = new HttpClient();
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
                client.BaseAddress = new Uri(urlBase);
                /*var url = string.Format(
                    "{0}{1}/{2}",
                    servicePrefix,
                    controller,
                    id);*/
                var url = $"{servicePrefix}{controller}/{id}";
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
        //Search Request
        public async Task<Response> SearchRequest<T>(
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
                // var newRecord = JsonConvert.DeserializeObject<T>(result);
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
        public async Task<Response> SearchExerciceRequest<T>(
            string urlBase,
            string servicePrefix,
            string controller,
            string accessToken,
            SearchExercice searchRequest)
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
                // var newRecord = JsonConvert.DeserializeObject<T>(result);
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
        //autocomplete
        public async Task<Response> SaveTest<T>(
            string urlBase,
            string servicePrefix,
            string controller,
            string accessToken,
            SearchRequestByUser searchRequest)
        {
            try
            {
                var request = JsonConvert.SerializeObject(searchRequest);
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
                Debug.WriteLine("********result*************");
                Debug.WriteLine(result);
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
        public async Task<List<T>> AutocompleteClient<T>(
          string urlBase,
          string servicePrefix,
          string controller,
          string accessToken,
          SearchRequestByUser searchRequest)
        {

            var request = JsonConvert.SerializeObject(searchRequest);
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

            var result = await response.Content.ReadAsStringAsync();
            //var newRecord = JsonConvert.DeserializeObject<T>(result);
            var list = JsonConvert.DeserializeObject<List<T>>(result, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });

            return list;

        }
        //Search Request
        public async Task<Response> SearchUserOfPlanning<T>(
            string urlBase,
            string servicePrefix,
            string controller,
            string accessToken,
            SearchUserOfPlanning searchRequest)
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
                // var newRecord = JsonConvert.DeserializeObject<T>(result);
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
        /*
        public async Task<string> LoginAsync(string userName, string password)
        {
            var keyValues = new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>("username", userName),
                new KeyValuePair<string, string>("password", password),
                new KeyValuePair<string, string>("grant_type", "password")
            };

            var request = new HttpRequestMessage(HttpMethod.Post, "http://192.168.1.59:8092/Token"); 

            request.Content = new FormUrlEncodedContent(keyValues);
            

            var client = new HttpClient();
            var response = await client.SendAsync(request);

            var content = await response.Content.ReadAsStringAsync();

            JObject jwtDynamic = JsonConvert.DeserializeObject<dynamic>(content);

            var accessToken = jwtDynamic.Value<string>("access_token");
            var accessTokenExpiration = jwtDynamic.Value<DateTime>(".expires");
            Settings.AccessTokenExpirationDate = accessTokenExpiration;

            Debug.WriteLine(accessTokenExpiration);

            Debug.WriteLine(content);

            return accessToken;

        }

        public async Task<List<Idea>> GetIdeasAsync(string accessToken)
        {
            var client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
            var json = await client.GetStringAsync("http://192.168.1.59:8092/api/Ideas");
            var ideas = JsonConvert.DeserializeObject<List<Idea>>(json);
            return ideas;
        }

        public async Task PostIdeaAsync(Idea idea, string accessToken)
        {
            var client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

            var json = JsonConvert.SerializeObject(idea);
            HttpContent content = new StringContent(json);
            content.Headers.ContentType = new MediaTypeHeaderValue("application/json");

            var response = await client.PostAsync("http://192.168.1.59:8092/api/Ideas", content);
            if (response.IsSuccessStatusCode)
            {
                //await Navigation.PopAsync();
                await Application.Current.MainPage.DisplayAlert("Message", "Idea Added", "ok");
            }
            else
            {
                //DisplayAlert("Message", "Data Failed To Save", "Ok");
                await Application.Current.MainPage.DisplayAlert("Message", "Failed to add Idea", "ok");
            }
        }

        public async Task PutIdeaAsync(Idea idea, string accessToken)
        {
            var client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

            var json = JsonConvert.SerializeObject(idea);
            HttpContent content = new StringContent(json);
            content.Headers.ContentType = new MediaTypeHeaderValue("application/json");

            var response = await client.PutAsync("http://192.168.1.59:8092/api/Ideas/" + idea.Id, content);

        }

        public async Task DeleteIdeaAsync(int ideaId, string accessToken)
        {
            var client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

            var response = await client.DeleteAsync("http://192.168.1.59:8092/api/Ideas/" + ideaId);
            if (response.IsSuccessStatusCode)
            {
                //await Navigation.PopAsync();
                await Application.Current.MainPage.DisplayAlert("Message", "Idea Deleted", "ok");
            }
            else
            {
                //DisplayAlert("Message", "Data Failed To Save", "Ok");
                await Application.Current.MainPage.DisplayAlert("Message", "Failed to delete Idea", "ok");
            }
        }

        public async Task<List<Idea>> SearchIdeasAsync(string keyword, string accessToken)
        {
            var client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(
                "Bearer", accessToken);

            var json = await client.GetStringAsync("http://192.168.1.59:8092/api/Ideas/Search/" + keyword);

            var ideas = JsonConvert.DeserializeObject<List<Idea>>(json);

            return ideas;
        }*/
    }
}
