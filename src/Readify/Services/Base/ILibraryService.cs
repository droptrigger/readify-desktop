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
    }
}
