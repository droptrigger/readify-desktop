using Readify.DTO.Library;

namespace Readify.Services.Base
{
    public interface ILibraryService
    {
        /// <summary>
        /// Метод получения всех книг пользователя
        /// </summary>
        /// <param name="userId">Id пользователя</param>
        /// <returns>Книги пользователя</returns>
        Task<SeeLibrariesDTO> GetBooksByUserIdAsync(int userId);

        /// <summary>
        /// Метод добавления книги в библиотку
        /// </summary>
        /// <param name="library">Данные, необходимые для добавления</param>
        /// <returns>True - если успешно</returns>
        Task<bool> AddLibraryAsync(AddLibraryDTO library);

        /// <summary>
        /// Метод удаления книги из библиотки
        /// </summary>
        /// <param name="library">Данные, необходимые для удаления</param>
        /// <returns>True - если успешно</returns>
        Task<bool> DeleteLibraryAsync(AddLibraryDTO library);
    }
}
