using Readify.DTO.Books;

namespace Readify.Services.Base
{
    public interface IBookService
    {
        /// <summary>
        /// Метод получения книги по ID
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<BookDTO> GetBookByIdAsync(int id);

        /// <summary>
        /// Метод получения всех жанров
        /// </summary>
        /// <returns></returns>
        Task<List<GenreDTO>> GetAllGenresAsync();

        /// <summary>
        /// Метод публикации книги
        /// </summary>
        /// <returns></returns>
        Task<bool> DeployBookAsync(AddBookDTO addBookDTO);
    }
}
