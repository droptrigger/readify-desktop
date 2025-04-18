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
    }
}
