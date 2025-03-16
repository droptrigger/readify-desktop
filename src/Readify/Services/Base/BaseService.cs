using Readify.DTO;
using Readify.Services.Connection;
using Readify.Services.Exceptions.Enums;
using System.Net.Http;
using System.Net;
using System.Text.Json;
using System.Text;
using Readify.Services.Exceptions;
using ApplicationException = Readify.Services.Exceptions.ApplicationException;

namespace Readify.Services.Base
{
    /// <summary>
    /// Базовый класс для реализации Service паттерна
    /// </summary>
    /// <typeparam name="Response">Тип DTO-объекта ответа от сервера</typeparam>
    public abstract class BaseService<Response>
        where Response : ResponseDTO, new()
    {
        public const string INCORRECT_PARAMETERS_ERROR_MSG = "Некорректные параметры";
        public const string RESULT_NOT_FOUND_ERROR_MSG = "Ошибка получения данных";

        /// <summary>
        /// API-клиент с предварительно настроенным HttpClient
        /// </summary>
        protected UserApiClient ApiClient => IoC.IoC.Get<UserApiClient>();


        /// <summary>
        /// Универсальный метод для выполнения HTTP-запросов
        /// </summary>
        /// <typeparam name="Params">Тип параметров запроса</typeparam>
        /// <param name="method">HTTP-метод (GET, POST и т.д.)</param>
        /// <param name="endpoint">Конечная точка API</param>
        /// <param name="params">Объект с параметрами запроса</param>
        /// <returns>Десериализованный ответ от сервера</returns>
        protected async Task<Response> ExecuteRequestAsync<Params>(HttpMethod method, string endpoint, Params @params)
        {
            try
            {
                var request = new HttpRequestMessage(method, endpoint);

                if (@params != null && (method == HttpMethod.Post || method == HttpMethod.Put))
                {
                    request.Content = new StringContent(
                        JsonSerializer.Serialize(@params),
                        Encoding.UTF8,
                        "application/json");
                }

                var response = await ApiClient.HttpClient.SendAsync(request);

                return await HandleResponseAsync(response);
            }
            catch (HttpRequestException ex)
            {
                throw new ServerException($"Ошибка сети: {ex.Message}");
            }
        }


        /// <summary>
        /// Обработка HTTP-ответа от сервера
        /// </summary>
        /// <param name="response">Полученный HTTP-ответ</param>
        /// <returns>Обработанный DTO-объект</returns>
        private async Task<Response> HandleResponseAsync(HttpResponseMessage response)
        {
            var content = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
            {
                throw response.StatusCode switch
                {
                    HttpStatusCode.Unauthorized => new ApplicationException("Требуется авторизация"),
                    HttpStatusCode.NotFound => new ServerException("Ресурс не найден"),
                    _ => new ServerException($"HTTP ошибка: {response.StatusCode}")
                };
            }

            var result = JsonSerializer.Deserialize<Response>(content)
                ?? throw new ServerException(RESULT_NOT_FOUND_ERROR_MSG);

            if (!result.IsSuccess)
            {
                throw result.TypeOfException.Value == TypesOfExceptions.UNEXPECTED
                    ? new ServerException(result.ErrorMessage)
                    : new ApplicationException(result.ErrorMessage);
            }

            return result;
        }

    }
}
