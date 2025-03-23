using Microsoft.AspNetCore.Http;
using Readify.DTO;
using Readify.DTO.Users;
using Readify.Services.Exceptions;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Windows;

namespace Readify.Services.Connections
{
    /// <summary>
    /// Класс, отвечающий за отправку http-запросов на сервер
    /// </summary>
    public class ApiClient
    {
        /// <summary>
        /// Клиент, для отправки Http-запросов на сервер
        /// </summary>
        private readonly HttpClient _httpClient;

        /// <summary>
        /// Адрес сервера
        /// </summary>
        private readonly string _baseUrl;

        /// <summary>
        /// Конструктор
        /// </summary>
        public ApiClient()
        {
            _baseUrl = Settings.Default.BaseUrl;
            _httpClient = new HttpClient();
        }

        /// <summary>
        /// Метод отправки запроса на 
        /// </summary>
        /// <param name="method">Метод POST/GET/UPDATE/DELETE</param>
        /// <param name="endpoint">Эндпоинт, на который делается запрос</param>
        /// <param name="data">Данные для отправки</param>
        /// <param name="isAuthorized">True - если отправка идет на защищенный эндпоинт</param>
        /// <returns>Ответ сервера</returns>
        /// <exception cref="UnauthorizedAccessException"></exception>
        public async Task<HttpResponseMessage> SendRequestAsync(
            HttpMethod method,
            string endpoint,
            object data = null!,
            bool isAuthorized = false)
        {
            async Task<HttpResponseMessage> SendRequestInternal()
            {
                var url = $"{_baseUrl}{endpoint}";
                var request = new HttpRequestMessage(method, url);

                if (data != null)
                {
                    var formData = new Dictionary<string, string>();
                    foreach (var prop in data.GetType().GetProperties())
                    {
                        formData.Add(prop.Name, prop.GetValue(data)?.ToString()!);
                    }
                    request.Content = new FormUrlEncodedContent(formData);
                }

                if (isAuthorized)
                {
                    var accessToken = Settings.Default.AccessToken;
                    request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
                }

                return await _httpClient.SendAsync(request);
            }

            var response = await SendRequestInternal();

            if (response.StatusCode == HttpStatusCode.Unauthorized && isAuthorized)
            {
                await RefreshTokenAsync();
                return await SendRequestInternal();
            }

            return response;
        }

        /// <summary>
        /// Асинхронный метод регистрации
        /// </summary>
        /// <param name="loginDTO">Данные для входа <see cref="LoginDTO"/></param>
        /// <returns>True - если авторизация успешна</returns>
        public async Task<bool> LoginAsync(LoginDTO loginDTO)
        {
            var response = await SendRequestAsync(HttpMethod.Post, "/api/auth/login", loginDTO);

            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadFromJsonAsync<LoginResponce>();
                SaveTokens(content!.Token!, content!.RefreshToken!);
                App.CurrentUser = content!.User!;
                return true;
            }

            return false;
        }

        /// <summary>
        /// Асинхронный метод получения refresh-токена
        /// </summary>
        /// <returns>True - если все успешно</returns>
        public async Task<bool> RefreshTokenAsync()
        {
            var appRefreshToken = Settings.Default.RefreshToken;
            var appDevice = Settings.Default.Device;

            if (string.IsNullOrEmpty(appRefreshToken)) 
                return false;

            try
            {
                var response = await SendRequestAsync(HttpMethod.Post, "/api/auth/refresh", new RefreshDTO { 
                    Token = appRefreshToken,
                    DeviceType = appDevice
                });

                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadFromJsonAsync<LoginResponce>();

                    SaveTokens(content?.Token!, content?.RefreshToken!);
                    App.CurrentUser = content!.User!;
                    return true;
                }
            }
            catch (HttpRequestException ex)
            {
                throw new HttpRequestException(ex.Message);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return false;
            }

            ClearTokens();
            return false;
        }

        /// <summary>
        /// Метод сохранения токена в <see cref="Settings"/>
        /// </summary>
        /// <param name="accessToken">Access-токен</param>
        /// <param name="refreshToken">Refresh-токен</param>
        private void SaveTokens(string accessToken, string refreshToken)
        {
            Settings.Default.AccessToken = accessToken;
            Settings.Default.RefreshToken = refreshToken;
            Settings.Default.Save();
            Settings.Default.Reload();
        }

        /// <summary>
        /// Метод удаления токенов из <see cref="Settings"/>
        /// </summary>
        public void ClearTokens()
        {
            Settings.Default.AccessToken = string.Empty;
            Settings.Default.RefreshToken = string.Empty;
            Settings.Default.Save();
        }


        public async Task<HttpResponseMessage> SendMultipartFormDataAsync(
            HttpMethod method,
            string endpoint,
            object data,
            bool isAuthorized = false)
        {
            async Task<HttpResponseMessage> SendRequestInternal()
            {
                var url = $"{_baseUrl}{endpoint}";
                var request = new HttpRequestMessage(method, url);

                var formData = new MultipartFormDataContent();

                if (data != null)
                {
                    foreach (var prop in data.GetType().GetProperties())
                    {
                        var value = prop.GetValue(data);
                        if (value == null) continue;

                        if (value is IFormFile file)
                        {
                            var fileContent = new StreamContent(file.OpenReadStream());
                            formData.Add(fileContent, prop.Name, file.FileName);
                        }
                        else
                        {
                            formData.Add(new StringContent(value.ToString()!), prop.Name);
                        }
                    }
                }

                request.Content = formData;

                if (isAuthorized)
                {
                    var accessToken = Settings.Default.AccessToken;
                    request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
                }

                return await _httpClient.SendAsync(request);
            }

            var response = await SendRequestInternal();

            if (response.StatusCode == HttpStatusCode.Unauthorized && isAuthorized)
            {
                await RefreshTokenAsync();
                return await SendRequestInternal();
            }

            return response;
        }
    }
}
