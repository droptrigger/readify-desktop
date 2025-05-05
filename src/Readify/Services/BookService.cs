using Readify.DTO.Books;
using Readify.Services.Base;
using Readify.Services.Connections;
using Readify.Services.Exceptions;
using System.Net.Http;
using System.Net;
using System.Net.Http.Json;
using Readify.DTO.Reviews;

namespace Readify.Services
{
    class BookService : IBookService
    {
        private const string GET_BOOK_ENDPOINT = "/api/books/";
        private const string GET_GENRES_ENDPOINT = "/api/books/genres";
        private const string DEPLOY_BOOK_ENDPOINT = "/api/books";
        private const string DELETE_BOOK_ENDPOINT = "/api/books";

        private const string ADD_REVIEW_ENDPOINT = "/api/books/reviews";
        private const string DELETE_REVIEW_ENDPOINT = "/api/books/reviews";

        /// <summary>
        /// ApiClient для работы с API
        /// </summary>
        private readonly ApiClient _apiClient;

        /// <summary>
        /// Конструктор
        /// </summary>
        public BookService()
        {
            _apiClient = new ApiClient();
        }

        public async Task<bool> AddReviewToBook(AddReviewDTO reviewDTO)
        {
            var response = await _apiClient.SendRequestAsync(HttpMethod.Post, ADD_REVIEW_ENDPOINT, reviewDTO, true);

            if (response.StatusCode == HttpStatusCode.Unauthorized)
            {
                throw new UnauthoizeException("Сессия закончена");
            }

            if (response.IsSuccessStatusCode)
            {
                return true;
            }

            var responseContent = await response.Content.ReadAsStringAsync();
            throw new Exception($"Ответ сервера: {response.StatusCode} - {responseContent}");
        }

        public async Task<bool> DeleteBookAsync(int bookId)
        {
            var response = await _apiClient.SendRequestAsync(HttpMethod.Delete, DELETE_BOOK_ENDPOINT, new
            {
                id = bookId
            }, true);

            if (response.StatusCode == HttpStatusCode.Unauthorized)
            {
                throw new UnauthoizeException("Сессия закончена");
            }

            if (response.IsSuccessStatusCode)
            {
                return true;
            }

            var responseContent = await response.Content.ReadAsStringAsync();
            throw new Exception($"Ответ сервера: {response.StatusCode} - {responseContent}");
        }

        public async Task<bool> DeleteReviewToBook(int id)
        {
            var response = await _apiClient.SendRequestAsync(HttpMethod.Delete, DELETE_REVIEW_ENDPOINT, new
            {
                id = id
            }, true);

            if (response.StatusCode == HttpStatusCode.Unauthorized)
            {
                throw new UnauthoizeException("Сессия закончена");
            }

            if (response.IsSuccessStatusCode)
            {
                return true;
            }

            var responseContent = await response.Content.ReadAsStringAsync();
            throw new Exception($"Ответ сервера: {response.StatusCode} - {responseContent}");
        }

        public async Task<bool> DeployBookAsync(AddBookDTO addBookDTO)
        {
            var response = await _apiClient.SendMultipartFormDataAsync(HttpMethod.Post, DEPLOY_BOOK_ENDPOINT, addBookDTO, true);

            if (response.StatusCode == HttpStatusCode.Unauthorized)
            {
                throw new UnauthoizeException("Сессия закончена");
            }

            if (response.IsSuccessStatusCode)
            {
                return true;
            }

            var responseContent = await response.Content.ReadAsStringAsync();
            throw new Exception($"Ответ сервера: {response.StatusCode} - {responseContent}");
        }

        public async Task<List<GenreDTO>> GetAllGenresAsync()
        {
            var response = await _apiClient.SendRequestAsync(HttpMethod.Get, GET_GENRES_ENDPOINT, new { }, true);

            if (response.StatusCode == HttpStatusCode.Unauthorized)
            {
                throw new UnauthoizeException("Сессия закончена");
            }

            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadFromJsonAsync<List<GenreDTO>>();
                return content!;
            }

            throw new Exception("Ответ сервера: " + response.Content.ToString());
        }

        /// <summary>
        /// Метод получения книги по ID
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<BookDTO> GetBookByIdAsync(int id)
        {
            var response = await _apiClient.SendRequestAsync(HttpMethod.Get, GET_BOOK_ENDPOINT + id, new
            {
                Id = id
            }, true);

            if (response.StatusCode == HttpStatusCode.Unauthorized)
            {
                throw new UnauthoizeException("Сессия закончена");
            }

            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadFromJsonAsync<BookDTO>();
                return content!;
            }

            var responseContent = await response.Content.ReadAsStringAsync();
            throw new Exception($"Ответ сервера: {response.StatusCode} - {responseContent}");
        }
    }
}
