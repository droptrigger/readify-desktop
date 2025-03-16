using Readify.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Readify.Services.Base
{
    public interface IAuthenticationSerivce
    {
        /// <summary>
        /// Метод, использующийся для регистрации пользователя в базе данных
        /// </summary>
        /// <exception cref="ApplicationException">происходит когда были переданы неправильным регистарционные данные</exception>
        /// <param name="userDTO">Данные, введенные от пользователя</param>
        Task RegisterUserAsync(UserDTO userDTO);

        /// <summary>
        /// Метоод, использующийся для отправки письма с кодом на почту 
        /// </summary>
        /// <exception cref="ApplicationException">происходит, когда была передана некорректная почта</exception>
        /// <param name="email">Почта, на которую необходимо отправить письмо</param>
        Task SendEmailAsync(string email);

        /// <summary>
        /// Метод, использующийся для авторизации пользователя в системе
        /// </summary>
        /// <param name="userDTO">Данные, введенные от пользователя</param>
        /// <returns>Данные авторизованного пользователя</returns>
        Task<UserDTO> LoginUserAsync(UserDTO userDTO);

        /// <summary>
        /// Метод, использующийся для выхода пользователя из системы
        /// </summary>
        /// <param name="userDTO">Данные пользователя</param>
        Task LogoutUserAsync(UserDTO userDTO);

        /// <summary>
        /// Метол, использующийся для получения нового Access-токена
        /// </summary>
        /// <param name="refreshToken">Refresh-токен</param>
        /// <returns>Новый Access-токен</returns>
        Task<string> RefreshTokenAsync(string refreshToken);

        Task<bool> 

    }
}
