using Readify.DTO.Books;
using Readify.DTO.Library;
using Readify.Services.Base;
using Readify.Services.Connections;
using Readify.Services.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http.Json;
using System.Windows;

namespace Readify.Services
{
    class BookService : IBookService
    {
        private const string GET_BOOK_ENDPOINT = "/api/books/";

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

            throw new Exception("Ответ сервера: " + response.Content.ToString());
        }
    }
}
