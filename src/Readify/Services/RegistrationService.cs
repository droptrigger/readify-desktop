using Readify.DTO;
using Readify.DTO.Users;
using Readify.Services.Base;
using Readify.Services.Connections;
using Readify.Services.Exceptions;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Windows;

namespace Readify.Services
{
    /// <summary>
    /// Сервис, отвечающий за регистрацию пользователя в приложении
    /// </summary>
    public class RegistrationService : IRegistrationService
    {
        private const string CHECK_MAIL_ENDPOINT = "/api/auth/checkemail";
        private const string CHECK_USERNAME_ENDPOINT = "/api/auth/checkusername";
        private const string SEND_CONFIRM_CODE_ENDPOINT = "/api/auth/send-mail";
        private const string REGISTRATION_ENDPOINT = "/api/auth/register";
        private const string UPDATE_ENDPOINT = "/api/user/update";

        /// <summary>
        /// ApiClient для работы с API
        /// </summary>
        private readonly ApiClient _apiClient;

        /// <summary>
        /// Конструктор
        /// </summary>
        public RegistrationService() 
        {
            _apiClient = new ApiClient();
        }

        /// <summary>
        /// Метод проверки электронной почты пользователя (на существование)
        /// </summary>
        /// <param name="email">Электронная почта</param>
        /// <returns>True - если существует</returns>
        /// <exception cref="Exception">Ошибка с ответом сервера</exception>
        public async Task<bool> CheckEmailAsync(string email)
        {
            var response = await _apiClient.SendRequestAsync(HttpMethod.Post, CHECK_MAIL_ENDPOINT, new
            {
                Email = email
            });

            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadFromJsonAsync<bool>();
                return content;
            }

            throw new Exception("Ответ сервера: " + response.StatusCode.ToString());
        }

        /// <summary>
        /// Метод проверки имени пользователя (на существование)
        /// </summary>
        /// <param name="username">Имя пользователя</param>
        /// <returns>True - если существует</returns>
        /// <exception cref="Exception">Ошибка с ответом сервера</exception>
        public async Task<bool> CheckUsernameAsync(string username)
        {
            var response = await _apiClient.SendRequestAsync(HttpMethod.Post, CHECK_USERNAME_ENDPOINT, new
            {
                Username = username
            });

            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadFromJsonAsync<bool>();
                return content;
            }

            throw new Exception("Ответ сервера: " + response.StatusCode.ToString());
        }

        /// <summary>
        /// Метод регистрации
        /// </summary>
        /// <param name="registrationDTO">Данные, необходимые для регистрации</param>
        /// <returns>True - если успешно</returns>
        /// <exception cref="Exception"></exception>
        public async Task<bool> RegistrationAsync(RegistrationDTO registrationDTO)
        {
            var response = await _apiClient.SendRequestAsync(HttpMethod.Post, REGISTRATION_ENDPOINT, registrationDTO);

            if (response.IsSuccessStatusCode)
            {
                return true;
            }

            throw new Exception("Ответ сервера: " + response.StatusCode.ToString());
        }

        /// <summary>
        /// Метод отправки письма с кодом подтвеждения на почту
        /// </summary>
        /// <param name="email">Почта получателя</param>
        /// <returns>True - если отправка успешно</returns>
        /// <exception cref="Exception"></exception>
        public async Task<bool> SendMailCodeAsync(string email)
        {
            var response = await _apiClient.SendRequestAsync(HttpMethod.Post, SEND_CONFIRM_CODE_ENDPOINT, new
            {
                Email = email
            });

            if (response.IsSuccessStatusCode)
            {
                return true;
            }

            throw new Exception("Ответ сервера: " + response.StatusCode.ToString());
        }

        /// <summary>
        /// Метод обновления данных пользователя
        /// </summary>
        /// <param name="updateUserDTO">Данные, необходимые для обновления данных</param>
        /// <returns>True - если успешно</returns>
        /// <exception cref="Exception"></exception>
        public async Task<bool> UpdateUserAsync(UpdateUserDTO updateUserDTO)
        {
            var response = await _apiClient.SendMultipartFormDataAsync(
                HttpMethod.Put,
                UPDATE_ENDPOINT,
                updateUserDTO,
                true);

            if (response.StatusCode == HttpStatusCode.Unauthorized)
            {
                throw new UnauthoizeException("Сессия закончена");
            }

            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadFromJsonAsync<UserDTO>();
                App.CurrentUser = content!;
                return true;
            }

            throw new Exception("Ответ сервера: " + response.StatusCode.ToString());
        }
    }
}
