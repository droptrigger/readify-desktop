using Readify.DTO;
using Readify.DTO.Subscribe;
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

        /// <summary>
        /// Метод подписки на пользователя
        /// </summary>
        /// <param name="subscribeDTO">Данные подписки</param>
        /// <returns>Информация об авторе (на кого подписываются)</returns>
        Task<UserDTO> FollowToUserAsync(SubscribeDTO subscribeDTO);

        /// <summary>
        /// Метод отписки от пользователя
        /// </summary>
        /// <param name="subscribeDTO">Данные подписки</param>
        /// <returns>Информация об авторе (на кого подписываются)</returns>
        Task<UserDTO> UnfollowForUserAsync(SubscribeDTO subscribeDTO);

        /// <summary>
        /// Метод поиска
        /// </summary>
        /// <param name="searchText">Поисковые данные</param>
        /// <returns>Найденные данные</returns>
        Task<SearchDTO> SearchAsync(string searchText);
    }
}
