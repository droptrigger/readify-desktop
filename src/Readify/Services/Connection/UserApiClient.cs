using System.Configuration;
using System.Net.Http;

namespace Readify.Services.Connection
{
    public class UserApiClient
    {
        /// <summary>
        /// HTTP-клиент для работы с REST API
        /// </summary>
        public HttpClient HttpClient { get; private set; }

        /// <summary>
        /// Конструктор по умолчанию
        /// </summary>
        public UserApiClient()
        {
            HttpClient = new HttpClient
            {
                BaseAddress = new Uri(ConfigurationManager.AppSettings["BaseApiUrl"]!)
            };

            HttpClient.DefaultRequestHeaders.Accept.Clear();
            HttpClient.DefaultRequestHeaders.Accept.Add(
                new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
        }

        /// <summary>
        /// Реализация IDisposable для корректного освобождения ресурсов
        /// </summary>
        public void Dispose()
        {
            HttpClient?.Dispose();
        }
    }
}
