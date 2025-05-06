using Readify.DTO;
using Readify.DTO.Subscribe;
using Readify.DTO.Users;
using Readify.Services.Base;
using Readify.Services.Connections;
using Readify.Services.Exceptions;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;

namespace Readify.Services
{
    /// <summary>
    /// Сервис для работы с пользователем
    /// </summary>
    public class UserService : IUserService
    {
        private const string GET_USER_ENDPOINT = $"/api/user/";
        private const string FOLLOW_USER_ENDPOINT = $"/api/user/subscribe";
        private const string UNFOLLOW_USER_ENDPOINT = $"/api/user/unsubscribe";
        private const string SEARCH_ENDPOINT = $"/api/user/search";

        /// <summary>
        /// ApiClient для работы с API
        /// </summary>
        private readonly ApiClient _apiClient;

        /// <summary>
        /// Конструктор
        /// </summary>
        public UserService()
        {
            _apiClient = new ApiClient();
        }

        /// <summary>
        /// Метод получения информации о пользователе по его Id
        /// </summary>
        /// <param name="id">Id пользователя</param>
        /// <returns>Информация о пользователе</returns>
        public async Task<UserDTO> GetUserByIdAsync(int id)
        {
            var response = await _apiClient.SendRequestAsync(HttpMethod.Get, GET_USER_ENDPOINT + id, new
            {
                Id = id
            }, true);

            if (response.StatusCode == HttpStatusCode.Unauthorized)
            {
                throw new UnauthoizeException("Сессия закончена");
            }

            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadFromJsonAsync<UserDTO>();
                return content!;
            }

            throw new Exception("Ответ сервера: " + response.StatusCode.ToString());
        }

        /// <summary>
        /// Метод подписки на пользователя
        /// </summary>
        /// <param name="subscribeDTO">Данные подписки</param>
        /// <returns>Информация об авторе (на кого подписываются)</returns>
        public async Task<UserDTO> FollowToUserAsync(SubscribeDTO subscribeDTO)
        {
            var response = await _apiClient.SendRequestAsync(
                HttpMethod.Post, 
                FOLLOW_USER_ENDPOINT, 
                subscribeDTO, true);

            if (response.StatusCode == HttpStatusCode.Unauthorized)
            {
                throw new UnauthoizeException("Сессия закончена");
            }

            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadFromJsonAsync<UserDTO>();
                return content!;
            }

            throw new Exception("Ответ сервера: " + response.StatusCode.ToString());
        }

        /// <summary>
        /// Метод отписки от пользователя
        /// </summary>
        /// <param name="subscribeDTO">Данные подписки</param>
        /// <returns>Информация об авторе (на кого подписываются)</returns>
        public async Task<UserDTO> UnfollowForUserAsync(SubscribeDTO subscribeDTO)
        {
            var response = await _apiClient.SendRequestAsync(
                HttpMethod.Post,
                UNFOLLOW_USER_ENDPOINT,
                subscribeDTO, true);

            if (response.StatusCode == HttpStatusCode.Unauthorized)
            {
                throw new UnauthoizeException("Сессия закончена");
            }

            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadFromJsonAsync<UserDTO>();
                return content!;
            }

            throw new Exception("Ответ сервера: " + response.StatusCode.ToString());
        }

        /// <summary>
        /// Метод поиска
        /// </summary>
        /// <param name="searchText">Поисковые данные</param>
        /// <returns>Найденные данные</returns>
        public async Task<SearchDTO> SearchAsync(string searchText)
        {
            var response = await _apiClient.SendRequestAsync(
                HttpMethod.Get,
                SEARCH_ENDPOINT,
                new
                {
                    SearchText = searchText
                }, true);

            if (response.StatusCode == HttpStatusCode.Unauthorized)
            {
                throw new UnauthoizeException("Сессия закончена");
            }

            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadFromJsonAsync<SearchDTO>();
                return content!;
            }

            throw new Exception("Ответ сервера: " + response.StatusCode.ToString());
        }
    }
}
