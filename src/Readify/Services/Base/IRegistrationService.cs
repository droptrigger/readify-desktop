using Readify.DTO.Users;

namespace Readify.Services.Base
{
    /// <summary>
    /// Интерфейс для реализации <see cref="RegistrationService"/>
    /// </summary>
    public interface IRegistrationService
    {
        /// <summary>
        /// Метод проверки имени пользователя (на существование)
        /// </summary>
        /// <param name="username">Имя пользователя</param>
        /// <returns>True - если существует</returns>
        Task<bool> CheckUsernameAsync(string username);

        /// <summary>
        /// Метод проверки электронной почты пользователя (на существование)
        /// </summary>
        /// <param name="email">Электронная почта</param>
        /// <returns>True - если существует</returns>
        /// <exception cref="Exception">Ошибка с ответом сервера</exception>
        Task<bool> CheckEmailAsync(string email);

        /// <summary>
        /// Метод отправки письма с кодом подтвеждения на почту
        /// </summary>
        /// <param name="email">Почта получателя</param>
        /// <returns>True - если отправка успешно</returns>
        Task<bool> SendMailCodeAsync(string email);

        /// <summary>
        /// Метод регистрации
        /// </summary>
        /// <param name="registrationDTO">Данные, необходимые для регистрации</param>
        /// <returns>True - если успешно</returns>
        Task<bool> RegistrationAsync(RegistrationDTO registrationDTO);

        /// <summary>
        /// Метод обновления данных пользователя
        /// </summary>
        /// <param name="updateUserDTO">Данные, необходимые для обновления данных</param>
        /// <returns>True - если успешно</returns>
        Task<bool> UpdateUserAsync(UpdateUserDTO updateUserDTO);

    }
}
