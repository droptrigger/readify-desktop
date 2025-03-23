using Readify.DTO.Users;
using Readify.Services.Base;
using Readify.Services.Connections;

namespace Readify.Services
{
    /// <summary>
    /// Сервис, отвечающий за авторизацию в приложении
    /// </summary>
    public class AuthService : IAuthService
    {
        /// <summary>
        /// ApiClient для работы с API
        /// </summary>
        private readonly ApiClient _apiClient;

        /// <summary>
        /// Конструктор
        /// </summary>
        public AuthService()
        {
            _apiClient = new ApiClient();
        }

        // <summary>
        /// Метод авторизации в приложении
        /// </summary>
        /// <param name="loginDTO">Данные для входа</param>
        /// <returns>True - если авторизация успешна</returns>
        public async Task<bool> LoginAsync(LoginDTO loginDTO)
        {
            return await _apiClient.LoginAsync(loginDTO);
        }

        /// <summary>
        /// Метод обновления access-токена
        /// </summary>
        /// <returns>True - если обновление успешно</returns>
        public async Task<bool> RefreshTokenAsync()
        {
            return await _apiClient.RefreshTokenAsync();
        }

        /// <summary>
        /// Метод выхода из аккаунта
        /// </summary>
        public void Logout()
        {
            throw new NotImplementedException();
        }
    }
}
