using Readify.DTO.Books;
using Readify.DTO.Reviews;

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

        /// <summary>
        /// Метод удаления книги
        /// </summary>
        /// <returns></returns>
        Task<bool> DeleteBookAsync(int id);

        /// <summary>
        /// Метод добавления отзыва
        /// </summary>
        /// <returns></returns>
        Task<bool> AddReviewToBook(AddReviewDTO reviewDTO);

        /// <summary>
        /// Метод удаления отзыва
        /// </summary>
        /// <returns></returns>
        Task<bool> DeleteReviewToBook(int id);
    }
}
