using Readify.DTO.Library;
using Readify.DTO.Users;
using Readify.Services.Base;
using Readify.Services.Connections;
using Readify.Services.Exceptions;
using System.Net.Http;
using System.Net;
using System.Net.Http.Json;

namespace Readify.Services
{
    /// <summary>
    /// Сервис, отвечающий за управлением библиотеки пользователя
    /// </summary>
    class LibraryService : ILibraryService
    {
        private const string GET_LIBRARY_ENDPOINT = "/api/library/";

        /// <summary>
        /// ApiClient для работы с API
        /// </summary>
        private readonly ApiClient _apiClient;

        /// <summary>
        /// Конструктор
        /// </summary>
        public LibraryService()
        {
            _apiClient = new ApiClient();
        }

        /// <summary>
        /// Метод получения всех книг пользователя
        /// </summary>
        /// <param name="userId">Id пользователя</param>
        /// <returns>Книги пользователя</returns>
        /// <exception cref="UnauthoizeException"></exception>
        /// <exception cref="Exception"></exception>
        public async Task<SeeLibrariesDTO> GetBooksByUserIdAsync(int userId)
        {
            var response = await _apiClient.SendRequestAsync(HttpMethod.Get, GET_LIBRARY_ENDPOINT + userId, new
            {
                Id = userId
            }, true);

            if (response.StatusCode == HttpStatusCode.Unauthorized)
            {
                throw new UnauthoizeException("Сессия закончена");
            }

            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadFromJsonAsync<SeeLibrariesDTO>();
                return content!;
            }

            throw new Exception("Ответ сервера: " + response.Content.ToString());
        }
    }
}
