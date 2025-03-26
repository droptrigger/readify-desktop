using Readify.DTO.Users;

namespace Readify.Services.Base
{
    /// <summary>
    /// Интерфейс для реализацтии <see cref=""/>
    /// </summary>
    public interface IUserService
    {
        /// <summary>
        /// Метод получения информации о пользователе по его Id
        /// </summary>
        /// <param name="id">Id пользователя</param>
        /// <returns>Информация о пользователе</returns>
        Task<UserDTO> GetUserByIdAsync(int id);
    }
}
