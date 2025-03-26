using Readify.DTO.Users;
using Readify.Services.Base;
using Readify.Services.Connections;
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

        public async Task<UserDTO> GetUserByIdAsync(int id)
        {
            var response = await _apiClient.SendRequestAsync(HttpMethod.Get, GET_USER_ENDPOINT + id, new
            {
                Id = id
            }, true);

            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadFromJsonAsync<UserDTO>();
                return content!;
            }

            throw new Exception("Ответ сервера: " + response.StatusCode.ToString());
        }
    }
}
