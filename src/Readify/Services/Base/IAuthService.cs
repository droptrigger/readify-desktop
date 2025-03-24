using Readify.DTO.Users;

namespace Readify.Services.Base
{
    /// <summary>
    /// Интерфейс для реализации <see cref="AuthService"/>
    /// </summary>
    public interface IAuthService
    {
        /// <summary>
        /// Метод авторизации в приложении
        /// </summary>
        /// <param name="loginDTO">Данные для входа</param>
        /// <returns>True - если авторизация успешна</returns>
        Task<bool> LoginAsync(LoginDTO loginDTO);
        
        /// <summary>
        /// Метод обновления access-токена
        /// </summary>
        /// <returns>True - если обновление успешно</returns>
        Task<bool> RefreshTokenAsync();

        /// <summary>
        /// Метод выхода из аккаунта
        /// </summary>
        Task<bool> LogoutAsync();
    }
}
